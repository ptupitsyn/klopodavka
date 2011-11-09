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
      /// Check whether if enemy is close enough and attacks; in other case generates starting pattern.
      /// </summary>
      private IKlopCell PrepareOrAttack(IKlopPlayer player, out int maxPathLength)
      {
         IKlopCell target;
         // TODO: Some crafty algorithm to do this faster? Like start marking nearby cells while not hit enemy cell..

         // Find closest enemy cell, compare to available turns, take attack decision (GetEnemyDistance is incorrect here)
         var closestEnemy = _model.Cells.Where(c => c.Owner != null && c.Owner != player)
            .Select(c => new Tuple<int, int, int>(_pathFinder.FindPath(player.BasePosX, player.BasePosY, c.X, c.Y, player).Count(cc => cc.Owner != player), c.X, c.Y))
            .Min();

         if (closestEnemy.Item1 < _model.RemainingKlops*AttackThreshold) //TODO: Constants (AttackThreshold, alias: Aggression)
         {
            target = _model[closestEnemy.Item2, closestEnemy.Item3];
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
         IKlopCell target;
         target = _model.Cells
                     .Where(c =>
                               {
                                  if (c.X < 1 || c.Y < 1 || c.X >= _model.FieldWidth - 2 || c.Y >= _model.FieldHeight - 2) return false;
                                  if (_model.GetNeighborCells(c).Any(cc => cc.Owner != null)) return false;
                                  var dx = Math.Abs(c.X - player.BasePosX);
                                  var dy = Math.Abs(c.Y - player.BasePosY);
                                  return dx > 1 && dy > 1
                                         && ((dx*dx + dy*dy) < (Math.Pow(_model.FieldHeight, 2) + Math.Pow(_model.FieldWidth, 2))/3)
                                         && (GetEnemyDistance(c, player) > _model.TurnLength/1.7);
                               }).Random() ?? _model.Cells.Where(c => c.Owner == null).Random();
         // TODO: Target sometimes falls behing enemy cells, and, however, target cell is not close to enemy, the path is.
         // TODO: "Safe path"?? "Safe evaluator".. or SafePathFinder. How to build safe cells map fast?
         return target;
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
      /// <param name="cell">The cell.</param>
      /// <param name="player"></param>
      /// <returns></returns>
      private double GetEnemyDistance(IKlopCell cell, IKlopPlayer player)
      {
         var cells = _model.Cells.Where(c => c.Owner != null && c.Owner != player).ToArray();
         var doubles = cells.Select(c => KlopPathFinder.GetDistance(c, cell)).ToArray();
         return doubles.Min();
      }

      #endregion
   }
}