#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

      private IKlopModel model;
      private KlopPathFinder pathFinder;
      private BackgroundWorker worker;

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
         model = klopModel;
         pathFinder = new KlopPathFinder(model);
         model.PropertyChanged += ModelPropertyChanged;
         StartWorker();
      }

      /// <summary>
      /// Finds the most important cell: cell which most of all affects total path cost.
      /// </summary>
      /// <returns>Tuple of most important cell and path cost difference.</returns>
      public Tuple<IKlopCell, double> FindMostImportantCell(int startX, int startY, int finishX, int finishY, IKlopPlayer klopPlayer)
      {
         var startN = pathFinder.GetNodeByCoordinates(startX, startY);
         var finishN = pathFinder.GetNodeByCoordinates(finishX, finishY);
         var initialCost = pathFinder.FindPath(startN, finishN, klopPlayer, false).Sum(n => n.Cost);
         double maxCost = 0;
         Node resultNode = null;
         foreach (
            var node in
               model.Cells.Where(c => c.Available && c.Owner == klopPlayer && c.State == ECellState.Alive).Select(c => pathFinder.GetNodeByCoordinates(c.X, c.Y))
            )
         {
            var oldCost = node.Cost;
            node.Cost = KlopPathFinder.TurnBlockedCost;

            var cost = pathFinder.FindPath(startN, finishN, klopPlayer, false, true).Sum(n => n.Cost);
            if (cost > maxCost)
            {
               maxCost = cost;
               resultNode = node;
            }

            node.Cost = oldCost;
         }
         return resultNode == null ? null : new Tuple<IKlopCell, double>(model[resultNode.X, resultNode.Y], maxCost - initialCost);
      }


      public IKlopCell FindCheapestCell(IKlopPlayer klopPlayer)
      {
         pathFinder.EvaluateCells(klopPlayer);
         return model.Cells.Where(c => c.Owner != klopPlayer)
            .Select(c => new {c, node = pathFinder.GetNodeByCoordinates(c.X, c.Y)}).Highest(c => -c.node.Cost).c;
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
            if (worker == null)
            {
               worker = new BackgroundWorker {WorkerSupportsCancellation = true, WorkerReportsProgress = true};
               worker.DoWork += DoThinking;
               worker.ProgressChanged += DoTurn;
            }
            return worker;
         }
      }

      #endregion

      #region Private and protected methods

      /// <summary>
      /// Starts the worker.
      /// </summary>
      private void StartWorker()
      {
         if (model.CurrentPlayer != this) return;
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
         if (cell != null && model.CurrentPlayer == this)
         {
            model.MakeTurn(cell);
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
            if (model.CurrentPlayer == this)
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
         //TODO: Catch exceptions!
         List<IKlopCell> path = null;

         while (model.CurrentPlayer == this && model.Cells.Any(c => c.Available) && !Worker.CancellationPending)
         {
            while (path == null || path.Count == 0)
            {
               IKlopCell target;
               var maxPathLength = int.MaxValue;

               if (model.Cells.Any(c => c.State == ECellState.Dead) || model.Cells.Count(c => c.Owner != null) > model.FieldHeight*model.FieldWidth/8)
               {
                  // Fight started, rush to base
                  var enemy = model.Players.First(p => p != model.CurrentPlayer);
                  target = model[enemy.BasePosX, enemy.BasePosY];
                  maxPathLength = 1;
                  var importantCell = FindMostImportantCell(model.CurrentPlayer.BasePosX, model.CurrentPlayer.BasePosY, target.X, target.Y, enemy);

                  //TODO: Find most important reacheble cell!
                  if (importantCell != null && importantCell.Item2 > KlopPathFinder.TurnEmptyCost*2)
                  {
                     //TODO: FindMostImportantCell should return list of cells, filter it and use.
                     target = importantCell.Item1;
                  }
                  else
                  {
                     target = FindCheapestCell(model.CurrentPlayer);
                  }
               }
               else
               {
                  // Fight not started, generate pattern
                  maxPathLength = model.TurnLength/3;
                  target = model.Cells.Where(c =>
                                                {
                                                   //TODO: c.GetNeighborCount == 0
                                                   if (c.X < 1 || c.Y < 1 || c.X >= model.FieldWidth - 2 || c.Y >= model.FieldHeight - 2) return false;
                                                   //var d = KlopPathFinder.GetDistance(c.X, c.Y, model.CurrentPlayer.BasePosX, model.CurrentPlayer.BasePosY);
                                                   var dx = Math.Abs(c.X - model.CurrentPlayer.BasePosX);
                                                   var dy = Math.Abs(c.Y - model.CurrentPlayer.BasePosY);
                                                   return dx > 1 && dy > 1 && (dx*dx + dy*dy) < (Math.Pow(model.FieldHeight, 2) + Math.Pow(model.FieldWidth, 2))/4;
                                                }).Random() ?? model.Cells.Where(c => c.Owner == null).Random();
               }

               // Find path FROM target to have correct ordered list
               path = pathFinder.FindPath(target.X, target.Y, model.CurrentPlayer.BasePosX, model.CurrentPlayer.BasePosY, model.CurrentPlayer).Take(maxPathLength).ToList();
            }
            var cell = path.First();
            path.Remove(cell);

            if (!cell.Available)
            {
               // Something went wrong, pathfinder returned unavailable cell. Use simple fallback logic:
               // This can happen also when base reached. Need to switch strategy.
               cell = model.Cells.First(c => c.Available);
               path = null; // Invalidate path
            }

            model.MakeTurn(cell);
         }
      }

      #endregion
   }
}