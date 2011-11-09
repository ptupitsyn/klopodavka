#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using KlopAi.algo;
using KlopAi.Extentions;
using KlopIfaces;

#endregion

namespace KlopAi
{
   public class KlopAiPlayer : IKlopPlayer
   {
      #region Fields and Constants

      private IKlopModel _model;
      private KlopPathFinder _pathFinder;
      private BackgroundWorker _worker;
      private readonly object _syncRoot = new object();
      private const decimal AttackThreshold = 0.3M;

      #endregion

      #region IKlopPlayer implementation

      public string Name { get; set; }

      public int BasePosX { get; set; }
      public int BasePosY { get; set; }

      public bool Human
      {
         get { return false; }
      }

      public Color Color { get; set; }

      #endregion

      #region Public methods

      /// <summary>
      /// Sets the model. Must be called to activate CPU player.
      /// </summary>
      /// <param name="klopModel">The klop model.</param>
      public void SetModel(IKlopModel klopModel)
      {
         _model = klopModel;
         _pathFinder = new KlopPathFinder(_model);
         _model.PropertyChanged += ModelPropertyChanged;
         StartWorker();
      }

      /// <summary>
      /// Finds the most important cell: cell which most of all affects total path cost.
      /// </summary>
      /// <returns>Tuple of most important cell and path cost difference.</returns>
      public Tuple<IKlopCell, double> FindMostImportantCell(int startX, int startY, int finishX, int finishY, IKlopPlayer klopPlayer)
      {
         var startN = _pathFinder.GetNodeByCoordinates(startX, startY);
         var finishN = _pathFinder.GetNodeByCoordinates(finishX, finishY);
         var initialCost = _pathFinder.FindPath(startN, finishN, klopPlayer, false).Sum(n => n.Cost);
         double maxCost = 0;
         Node resultNode = null;
         foreach (var node in _model.Cells.Where(c => c.Available && c.Owner == klopPlayer && c.State == ECellState.Alive).Select(c => _pathFinder.GetNodeByCoordinates(c.X, c.Y)))
         {
            var oldCost = node.Cost;
            node.Cost = KlopCellEvaluator.TurnBlockedCost;

            var cost = _pathFinder.FindPath(startN, finishN, klopPlayer, false, true).Sum(n => n.Cost);
            if (cost > maxCost)
            {
               maxCost = cost;
               resultNode = node;
            }

            node.Cost = oldCost;
         }
         return resultNode == null ? null : new Tuple<IKlopCell, double>(_model[resultNode.X, resultNode.Y], maxCost - initialCost);
      }


      public IKlopCell FindCheapestCell(IKlopPlayer klopPlayer)
      {
         _pathFinder.EvaluateCells(klopPlayer);
         return _model.Cells.Where(c => c.Owner != klopPlayer)
            .Select(c => new {c, node = _pathFinder.GetNodeByCoordinates(c.X, c.Y)}).Highest(c => -c.node.Cost).c;
      }

      #endregion

      #region Private and protected properties and indexers

      /// <summary>
      /// Gets the worker.
      /// </summary>
      /// <value>The worker.</value>
      private BackgroundWorker Worker
      {
         get
         {
            if (_worker == null)
            {
               _worker = new BackgroundWorker {WorkerSupportsCancellation = true, WorkerReportsProgress = true};
               _worker.DoWork += DoThinkingMain;
               _worker.ProgressChanged += DoTurn;
            }
            return _worker;
         }
      }

      #endregion

      #region Private and protected methods

      /// <summary>
      /// Starts the worker.
      /// </summary>
      private void StartWorker()
      {
         if (_model.CurrentPlayer != this) return;

         if (Worker.IsBusy)
         {
            Worker.CancelAsync();
            _worker = null;  // Throw away old worker if it is hanging for some reason
         }
         Worker.RunWorkerAsync();
      }

      private void StopWorker()
      {
         Worker.CancelAsync();
      }

      #endregion

      #region Event handlers

      private void DoTurn(object sender, ProgressChangedEventArgs e)
      {
         var cell = e.UserState as IKlopCell;
         if (cell != null && _model.CurrentPlayer == this)
         {
            _model.MakeTurn(cell);
         }
      }

      /// <summary>
      /// Handles the PropertyChanged event of the model.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
      private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName == "CurrentPlayer")
         {
            if (_model.CurrentPlayer == this)
            {
               StartWorker();
            }
            else
            {
               StopWorker();
            }
         }
      }


      /// <summary>
      /// AI Worker method.
      /// </summary>
      private void DoThinkingMain(object sender, DoWorkEventArgs doWorkEventArgs)
      {
         Thread.CurrentThread.Priority = ThreadPriority.Lowest;
         //TODO: Catch exceptions?

         lock (_syncRoot)  // Sometimes workers can overlap
         {
            var path = new List<IKlopCell>();
            while (_model.CurrentPlayer == this && _model.Cells.Any(c => c.Available) && !Worker.CancellationPending)
            {
               var player = _model.CurrentPlayer;
               while (path.Count == 0)
               {
                  int maxPathLength;
                  var target = FindNextTarget(player, out maxPathLength);

                  // Find path FROM target to have correct ordered list
                  path.AddRange(_pathFinder.FindPath(target.X, target.Y, player.BasePosX, player.BasePosY, player).Take(maxPathLength));
               }
               var cell = path.First();
               path.Remove(cell);

               if (!cell.Available)
               {
                  // Something went wrong, pathfinder returned unavailable cell. Use simple fallback logic:
                  // This can happen also when base reached. Need to switch strategy.
                  cell = _model.Cells.FirstOrDefault(c => c.Available);
                  path.Clear();
                  if (cell == null) continue;
               }

               _model.MakeTurn(cell);
            }
         }
      }


      /// <summary>
      /// Finds the next target cell. Core thinking method where gaming logic is situated.
      /// </summary>
      private IKlopCell FindNextTarget(IKlopPlayer player, out int maxPathLength)
      {
         if (IsFightStarted())
         {
            // Fight started, rush to base
            maxPathLength = 1;
            return DoFight(player);
         }
            
         return PrepareOrAttack(player, out maxPathLength);
      }


      /// <summary>
      /// Determines whether fight is started - there are dead clops on the field.
      /// </summary>
      private bool IsFightStarted()
      {
         return _model.Cells.Any(c => c.State == ECellState.Dead /*|| _model.Cells.Count(c => c.Owner != null) > _model.FieldHeight*_model.FieldWidth/8*/);
      }


      /// <summary>
      /// Finds the nearest enemy cell.
      /// if targetCell is not specified, all player-owned cells are used as targets.
      /// </summary>
      /// <returns></returns>
      private IKlopCell FindNearestEnemyCell(IKlopPlayer player, IKlopCell targetCell = null)
      {
         var flags = new bool[_model.FieldWidth,_model.FieldHeight];

         if (targetCell != null)
         {
            // Starting cell specified.
            flags[targetCell.X, targetCell.Y] = true;
         }
         else
         {
            // Starting cell not specified - mark all own cells with flags.
            foreach (var cell in _model.Cells.Where(c => c.Owner == player))
            {
               flags[cell.X, cell.Y] = true;
            }
         }

         // Then in each pass mark all flagged cells neighbors with flag until we find an enemy.
         while (true)
         {
            var neighborCells = _model.Cells.Where(c => flags[c.X, c.Y]).SelectMany(c => _model.GetNeighborCells(c)).Where(c => !flags[c.X, c.Y]).ToArray();
            foreach (var cell in neighborCells)
            {
               if (cell.Owner != null && cell.Owner != player)
               {
                  // Found foreigner cell - return it
                  return cell;
               }
               flags[cell.X, cell.Y] = true;
            }
         }
      }


      /// <summary>
      /// Check whether if enemy is close enough and attacks; in other case generates starting pattern.
      /// </summary>
      private IKlopCell PrepareOrAttack(IKlopPlayer player, out int maxPathLength)
      {
         IKlopCell target;

         var closestEnemy = FindNearestEnemyCell(player);
         var minEnemyDistance = GetTurnsCount(player, closestEnemy);

         if (minEnemyDistance < _model.RemainingKlops*AttackThreshold)
         {
            target = closestEnemy;
            maxPathLength = int.MaxValue;
         }
         else
         {
            // Fight not started, generate pattern
            target = GenerateStartingPattern(player);
            maxPathLength = 2; // _model.TurnLength / 3;
         }
         return target;
      }


      private IKlopCell GenerateStartingPattern(IKlopPlayer player)
      {
         // TODO: Target sometimes falls behing enemy cells, and, however, target cell is not close to enemy, the path is.
         // TODO: "Safe path"?? "Safe evaluator".. or SafePathFinder. How to build safe cells map fast?
         return _model.Cells
                   .Where(c =>
                             {
                                if (c.X < 1 || c.Y < 1 || c.X >= _model.FieldWidth - 2 || c.Y >= _model.FieldHeight - 2) return false;
                                if (_model.GetNeighborCells(c).Any(cc => cc.Owner != null)) return false;
                                var dx1 = Math.Abs(c.X - player.BasePosX);
                                var dy1 = Math.Abs(c.Y - player.BasePosY);
                                return dx1 > 1 && dy1 > 1
                                       && ((dx1*dx1 + dy1*dy1) < (Math.Pow(_model.FieldHeight, 2) + Math.Pow(_model.FieldWidth, 2))/3)
                                       && (GetEnemyDistance(player, c) > _model.TurnLength/1.7);
                             }).Random() ?? _model.Cells.Where(c => c.Owner == null).Random();
      }


      private IKlopCell DoFight(IKlopPlayer player)
      {
         IKlopCell target;
         var enemies = _model.Players.Where(p => p != player).ToArray();
         var enemy = enemies.FirstOrDefault(p => p.Human) ?? enemies.Random();
         target = _model[enemy.BasePosX, enemy.BasePosY];
         var importantCell = FindMostImportantCell(player.BasePosX, player.BasePosY, target.X, target.Y, enemy);

         //TODO: Find most important reacheble cell!
         if (importantCell != null && importantCell.Item2 > KlopCellEvaluator.TurnEmptyCost*2)
         {
            //TODO: FindMostImportantCell should return list of cells, filter it and use.
            target = importantCell.Item1;
         }
         else
         {
            target = FindCheapestCell(player); //TODO: Bug when no good cells to eat - it goes along the border
         }
         return target;
      }


      /// <summary>
      /// Gets the distance to the closest enemy cell.
      /// </summary>
      private double GetEnemyDistance(IKlopPlayer player, IKlopCell cell)
      {
         //TODO: Create a map of distances and cache it until new turn. Something like heat map.
         // BuildEnemyHeatMap! That's what we need.
         //return GetTurnsCount(player, FindNearestEnemyCell(player, cell));

         // This implementation is rather fast, the above is very slow.
         return _model.Cells.Where(c => c.Owner != null && c.Owner != player).Select(c => KlopPathFinder.GetDistance(c, cell)).Min();
      }


      /// <summary>
      /// Gets the count of turns needed to reach specified cell.
      /// </summary>
      private int GetTurnsCount(IKlopPlayer player, IKlopCell targetCell)
      {
         return _pathFinder.FindPath(player.BasePosX, player.BasePosY, targetCell.X, targetCell.Y, player).Count(cc => cc.Owner != player);
      }

      #endregion
   }
}