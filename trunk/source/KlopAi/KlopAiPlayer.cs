#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;
using KlopAi.Extentions;
using KlopAi.algo;
using KlopIfaces;

#endregion


namespace KlopAi
{
   public class KlopAiPlayer : IKlopPlayer
   {
      #region Fields and Constants

      private const decimal AttackThreshold = 0.4M;
      private readonly object _syncRoot = new object();
      private int[,] _distanceMap;
      private IKlopModel _model;
      private KlopPathFinder _pathFinder;
      private BackgroundWorker _worker;

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

      /// <summary>
      /// Gets or sets the turn delay. No delay when null. Can be used for demo purposes.
      /// </summary>
      public TimeSpan? TurnDelay { get; set; }


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
      /// Finds the most important cell: cell which most of all affects total path cost.
      /// </summary>
      /// <returns>Tuple of most important cell and path cost difference.</returns>
      private Tuple<IKlopCell, double> FindMostImportantCell(int startX, int startY, int finishX, int finishY, IKlopPlayer klopPlayer)
      {
         var startN = _pathFinder.GetNodeByCoordinates(startX, startY);
         var finishN = _pathFinder.GetNodeByCoordinates(finishX, finishY);
         var initialCost = _pathFinder.FindPath(startN, finishN, klopPlayer, false).Sum(n => n.Cost);
         double maxCost = 0;
         Node resultNode = null;
         foreach (
            var node in
               _model.Cells.Where(c => c.Available && c.Owner == klopPlayer && c.State == ECellState.Alive).Select(
                  c => _pathFinder.GetNodeByCoordinates(c.X, c.Y)))
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


      /// <summary>
      /// Starts the worker.
      /// </summary>
      private void StartWorker()
      {
         if (_model.CurrentPlayer != this) return;

         if (Worker.IsBusy)
         {
            Worker.CancelAsync();
            _worker = null; // Throw away old worker if it is hanging for some reason
         }
         Worker.RunWorkerAsync();
      }


      private void StopWorker()
      {
         Worker.CancelAsync();
      }


      /// <summary>
      /// Finds the next target cell. Core thinking method where gaming logic is situated.
      /// </summary>
      private IKlopCell FindNextTarget(out int maxPathLength)
      {
         if (IsFightStarted())
         {
            // Fight started, rush to base
            maxPathLength = 1;
            return DoFight();
         }

         return PrepareOrAttack(out maxPathLength);
      }


      /// <summary>
      /// Determines whether fight is started - there are dead clops on the field.
      /// </summary>
      private bool IsFightStarted()
      {
         return _model.Cells.Any(c => c.State == ECellState.Dead /*|| _model.Cells.Count(c => c.Owner != null) > _model.FieldHeight*_model.FieldWidth/8*/);
      }


      /// <summary>
      /// Finds enemy cell(s) which are closest to specified cell, or to any player cell if targetCell is null.
      /// if targetCell is not specified, all player-owned cells are used as targets.
      /// </summary>
      /// <returns></returns>
      private IEnumerable<IKlopCell> FindNearestEnemyCells(IKlopCell targetCell = null)
      {
         if (targetCell == null)
         {
            var availableCellsWithDistances = _model.Cells.Where(c => c.Available).Select(c => new { c, d = _distanceMap[c.X, c.Y] }).ToArray();
            var minDistance = availableCellsWithDistances.Select(c => c.d).Min();
            // Go over all available cells with the same minimum distance
            foreach (var c in availableCellsWithDistances.Where(c => c.d == minDistance).SelectMany(c => FindNearestEnemyCells(c.c)))
            {
               yield return c;
            }
            yield break;
         }


         var cell = targetCell;
         var cellDistance = _distanceMap[cell.X, cell.Y];
         if (cellDistance == 0)
         {
            // End of recursion, we have found enemy cell.
            yield return cell;
         }
         else
         {
            foreach (var c in _model.GetNeighborCells(cell).Where(c => _distanceMap[c.X, c.Y] < cellDistance).SelectMany(FindNearestEnemyCells))
            {
               yield return c;
            }
         }
      }


      private IKlopCell FindEnemyCellToAttack(IKlopCell targetCell = null)
      {
         var nearestEnemyCells = FindNearestEnemyCells(targetCell);
         // There could be several enemy cells with equal distances. Find the one closer to enemy base!
         var pathLengths = nearestEnemyCells.Select(c => new {c, pathLength = _pathFinder.FindPath(c.X, c.Y, c.Owner.BasePosX, c.Owner.BasePosY, this).Count});
         return pathLengths.Highest((c1, c2) => c1.pathLength < c2.pathLength).c;
      }


      /// <summary>
      /// Check whether if enemy is close enough and attacks; in other case generates starting pattern.
      /// </summary>
      private IKlopCell PrepareOrAttack(out int maxPathLength)
      {
         if (GetMinEnemyDistance() < _model.RemainingKlops*AttackThreshold)
         {
            maxPathLength = int.MaxValue;
            return FindEnemyCellToAttack();
         }

         // Fight not started, generate pattern
         maxPathLength = 2; // _model.TurnLength / 3;
         return GenerateStartingPattern();
      }


      private IKlopCell GenerateStartingPattern()
      {
         // TODO: Target sometimes falls behing enemy cells, and, however, target cell is not close to enemy, the path is.
         // TODO: "Safe path"?? "Safe evaluator".. or SafePathFinder. How to build safe cells map fast?
         return _model.Cells
                   .Where(c =>
                             {
                                if (c.X < 1 || c.Y < 1 || c.X >= _model.FieldWidth - 2 || c.Y >= _model.FieldHeight - 2) return false;
                                if (_model.GetNeighborCells(c).Any(cc => cc.Owner != null)) return false;
                                var dx1 = Math.Abs(c.X - BasePosX);
                                var dy1 = Math.Abs(c.Y - BasePosY);
                                return dx1 > 1 && dy1 > 1
                                       && ((dx1*dx1 + dy1*dy1) < (Math.Pow(_model.FieldHeight, 2) + Math.Pow(_model.FieldWidth, 2))/3)
                                       && (GetEnemyDistance(c) > _model.TurnLength/1.7);
                             }).Random() ?? _model.Cells.Where(c => c.Owner == null).Random();
      }


      private IKlopCell DoFight()
      {
         var enemy = GetPreferredEnemy();

         if (enemy == null)
         {
            // All enemies have been defeated. Game over.
            return _model.Cells.Where(c => c.Owner == null).FirstOrDefault();
         }

         var target = _model[enemy.BasePosX, enemy.BasePosY];
         var importantCell = FindMostImportantCell(BasePosX, BasePosY, target.X, target.Y, enemy);

         //TODO: Find most important reacheble cell!
         if (importantCell != null /* && importantCell.Item2 > KlopCellEvaluator.TurnEmptyCost*2*/)
         {
            //TODO: FindMostImportantCell should return list of cells, filter it and use.
            target = importantCell.Item1;
         }
         else
         {
            // No good cells to eat - seems like we've been disconnected
            var pathToBase = _pathFinder.FindPath(target.X, target.Y, BasePosX, BasePosY, this);
            // 0) Try to rush to base if there are available cells on the path
            // 1) Seek for available alive enemy
            // 2) Seek for next-to-available alive enemy
            // 3) Seek for any available enemy
            // 4) Seek for any available cell
            target = pathToBase.FirstOrDefault(c => c.Available) ??
                     _model.Cells.Where(c => c.Available && c.Owner != this).FirstOrDefault() ??
                     _model.Cells.Where(c => c.Owner != this && c.State == ECellState.Alive && _model.GetNeighborCells(c).Any(cc => cc.Available)).
                        FirstOrDefault() ??
                     _model.Cells.Where(c => c.Owner != this && c.State == ECellState.Alive).FirstOrDefault() ??
                     _model.Cells.Where(c => c.Available).Random();
         }

         return target;
      }


      /// <summary>
      /// Gets the enemy player which is most suitable to be attacked by us.
      /// </summary>
      private IKlopPlayer GetPreferredEnemy()
      {
         var enemies = _model.Players.Where(p => p != this && !_model.IsPlayerDefeated(p)).ToArray();
         var humanPlayers = enemies.Where(p => p.Human).ToArray();

         // Human enemies are preferred (make the game harder)
         if (humanPlayers.Length > 0)
            enemies = humanPlayers;

         if (enemies.Length == 0) return null;
         if (enemies.Length == 1) return enemies[0];
         return GetPreferredEnemy(enemies);
      }


      /// <summary>
      /// Gets the enemy player which is most suitable to be attacked by us.
      /// </summary>
      private IKlopPlayer GetPreferredEnemy(IEnumerable<IKlopPlayer> enemies)
      {
         // Find the enemy with cheapest path to it's base and attack it
         // We can also try to find weakest enemy
         // Or an enemy with closest cells
         // Or an enemy which attacks us most
         return enemies.OrderByDescending(e => _model.Cells.Where(c => c.Owner == e && c.State == ECellState.Alive).Count()).FirstOrDefault();
      }


      /// <summary>
      /// Gets the distance to the closest enemy cell.
      /// </summary>
      private double GetEnemyDistance(IKlopCell cell)
      {
         return _distanceMap[cell.X, cell.Y];
      }


      /// <summary>
      /// Gets the count of turns needed to reach specified cell.
      /// </summary>
      private int GetTurnsCount(IKlopCell targetCell)
      {
         return _pathFinder.FindPath(BasePosX, BasePosY, targetCell.X, targetCell.Y, this).Count(cc => cc.Owner != this);
      }


      /// <summary>
      /// For each cell calculates it's distance from any enemy cell.
      /// </summary>
      private int[,] BuildEnemyDistanceMap()
      {
         var distanceMap = new int[_model.FieldWidth,_model.FieldHeight];
         var totalCellCount = distanceMap.Length;
         var markedCellCount = 0;
         var maxHeat = 0;

         foreach (var cell in _model.Cells)
         {
            if (cell.Owner != null && cell.Owner != this)
            {
               // Enemy cell default heat = 0;
               markedCellCount++;
            }
            else
            {
               // Non-visited cells are marked with -1
               distanceMap[cell.X, cell.Y] = -1;
            }
         }

         // Then in each pass mark all flagged cells neighbors with flag until we find an enemy.
         while (markedCellCount < totalCellCount)
         {
            maxHeat++;
            var neighborCells = _model.Cells.Where(c => distanceMap[c.X, c.Y] >= 0).SelectMany(c => _model.GetNeighborCells(c)).ToArray();
            foreach (var cell in neighborCells)
            {
               if (distanceMap[cell.X, cell.Y] >= 0) continue; // Cell already visited. We could use Distinct, but it can be slow.
               distanceMap[cell.X, cell.Y] = maxHeat;
               markedCellCount++;
            }
         }

         // Visualize:
         //var s = VisualizeDistanceMap(distanceMap);

         return distanceMap;
      }


      /// <summary>
      /// Gets the distance to the nearest enemy cell.
      /// </summary>
      /// <returns></returns>
      private int GetMinEnemyDistance()
      {
         return _model.Cells.Where(c => c.Available).Select(c => _distanceMap[c.X, c.Y]).Min();
      }


      /// <summary>
      /// Visualizes the distance map.
      /// Just for debugging.
      /// </summary>
      protected string VisualizeDistanceMap(int[,] distanceMap)
      {
         //TODO: Make an extension method; or better - allow on-screen visualization with some overlay.
         var sb = new StringBuilder();
         for (var y = 0; y < _model.FieldHeight; y++)
         {
            for (var x = 0; x < _model.FieldWidth; x++)
            {
               sb.Append(distanceMap[x, y].ToString().PadRight(4));
            }
            sb.AppendLine();
         }
         return sb.ToString();
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

         lock (_syncRoot) // Sometimes workers can overlap
         {
            var path = new List<IKlopCell>();
            _distanceMap = BuildEnemyDistanceMap();
            while (_model.CurrentPlayer == this && _model.Cells.Any(c => c.Available) && !Worker.CancellationPending)
            {
               while (path.Count == 0)
               {
                  int maxPathLength;
                  var target = FindNextTarget(out maxPathLength);

                  if (target == null)
                  {
                     // Game over or we are defeated
                     return;
                  }

                  // Find path FROM target to have correct ordered list
                  path.AddRange(_pathFinder.FindPath(target.X, target.Y, BasePosX, BasePosY, this).Take(maxPathLength));
               }
               var cell = path.First();
               path.Remove(cell);

               if (!cell.Available)
               {
                  // Something went wrong, pathfinder returned unavailable cell. Use simple fallback logic:
                  // This can happen also when base reached. Need to switch strategy.
                  //TODO!!
                  //Debug.Assert(false, "PathFinder returned unavailable cell!");
                  cell = _model.Cells.FirstOrDefault(c => c.Available);
                  path.Clear();
                  if (cell == null) continue;
               }

               DoDelay();
               _model.MakeTurn(cell);
            }
         }
      }

      /// <summary>
      /// Delays processing if TurnDelay is not null.
      /// </summary>
      private void DoDelay()
      {
         if (TurnDelay != null)
         {
            Thread.Sleep(TurnDelay.Value);
         }
      }

      #endregion
   }
}