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
         foreach (
            var node in
               _model.Cells.Where(c => c.Available && c.Owner == klopPlayer && c.State == ECellState.Alive).Select(c => _pathFinder.GetNodeByCoordinates(c.X, c.Y))
            )
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
               _worker.DoWork += DoThinking;
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
      private void DoThinking(object sender, DoWorkEventArgs doWorkEventArgs)
      {
         Thread.CurrentThread.Priority = ThreadPriority.Lowest;
         //TODO: Catch exceptions!

         lock (_syncRoot)  // Sometimes workers can overlap
         {
            List<IKlopCell> path = null;

            while (_model.CurrentPlayer == this && _model.Cells.Any(c => c.Available) && !Worker.CancellationPending)
            {
               while (path == null || path.Count == 0)
               {
                  IKlopCell target;
                  var maxPathLength = int.MaxValue;

                  if (_model.Cells.Any(c => c.State == ECellState.Dead) || _model.Cells.Count(c => c.Owner != null) > _model.FieldHeight*_model.FieldWidth/8)
                  {
                     // Fight started, rush to base
                     var enemies = _model.Players.Where(p => p != _model.CurrentPlayer);
                     var enemy = enemies.FirstOrDefault(p => p.Human) ?? enemies.Random();
                     target = _model[enemy.BasePosX, enemy.BasePosY];
                     maxPathLength = 1;
                     var importantCell = FindMostImportantCell(_model.CurrentPlayer.BasePosX, _model.CurrentPlayer.BasePosY, target.X, target.Y, enemy);

                     //TODO: Find most important reacheble cell!
                     if (importantCell != null && importantCell.Item2 > KlopCellEvaluator.TurnEmptyCost*2)
                     {
                        //TODO: FindMostImportantCell should return list of cells, filter it and use.
                        target = importantCell.Item1;
                     }
                     else
                     {
                        target = FindCheapestCell(_model.CurrentPlayer);
                     }
                  }
                  else
                  {
                     // Fight not started, generate pattern
                     maxPathLength = _model.TurnLength/3;
                     target = _model.Cells.Where(c =>
                                                   {
                                                      //TODO: c.GetNeighborCount == 0
                                                      if (c.X < 1 || c.Y < 1 || c.X >= _model.FieldWidth - 2 || c.Y >= _model.FieldHeight - 2) return false;
                                                      //var d = KlopPathFinder.GetDistance(c.X, c.Y, model.CurrentPlayer.BasePosX, model.CurrentPlayer.BasePosY);
                                                      var dx = Math.Abs(c.X - _model.CurrentPlayer.BasePosX);
                                                      var dy = Math.Abs(c.Y - _model.CurrentPlayer.BasePosY);
                                                      return dx > 1 && dy > 1 && (dx*dx + dy*dy) < (Math.Pow(_model.FieldHeight, 2) + Math.Pow(_model.FieldWidth, 2))/4;
                                                   }).Random() ?? _model.Cells.Where(c => c.Owner == null).Random();
                  }

                  // Find path FROM target to have correct ordered list
                  path = _pathFinder.FindPath(target.X, target.Y, _model.CurrentPlayer.BasePosX, _model.CurrentPlayer.BasePosY, _model.CurrentPlayer).Take(maxPathLength).ToList();
               }
               var cell = path.First();
               path.Remove(cell);

               if (!cell.Available)
               {
                  // Something went wrong, pathfinder returned unavailable cell. Use simple fallback logic:
                  // This can happen also when base reached. Need to switch strategy.
                  cell = _model.Cells.FirstOrDefault(c => c.Available);
                  if (cell == null) continue; // There are no available cells...
                  path = null; // Invalidate path
               }

               _model.MakeTurn(cell);
            }
         }
      }

      #endregion
   }
}