using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using KlopAi.Extentions;
using KlopIfaces;

namespace KlopAi
{
   public class KlopAiPlayer : IKlopPlayer
   {
      #region Fields and Constants

      private IKlopModel model;
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
         model.PropertyChanged += ModelPropertyChanged;
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

      private void DoThinking(object sender, DoWorkEventArgs doWorkEventArgs)
      {
         //TODO: Catch exceptions!

         List<IKlopCell> path = null;
         var pathFinder = new KlopPathFinder(model);

         while (model.CurrentPlayer == this && model.Cells.Any(c => c.Available) && !Worker.CancellationPending)
         {
            while (path == null || path.Count == 0)
            {
               IKlopCell target;
               var maxPathLength = int.MaxValue;

               if (model.Cells.Any(c => c.State == ECellState.Dead) || model.Cells.Count(c=>c.Owner != null) > model.FieldHeight * model.FieldWidth / 7)
               {
                  // Fight started, rush to base
                  var enemy = model.Players.First(p => p != model.CurrentPlayer);
                  target = model[enemy.BasePosX, enemy.BasePosY];
               }
               else
               {
                  // Fight not started, generate pattern
                  maxPathLength = model.TurnLength / 3;
                  target = model.Cells.Where(c =>
                                                {
                                                   var d = KlopPathFinder.GetDistance(c.X, c.Y, model.CurrentPlayer.BasePosX, model.CurrentPlayer.BasePosY);
                                                   return d > 2 && d < (model.FieldHeight + model.FieldWidth)/3.7;
                                                }).Random();
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