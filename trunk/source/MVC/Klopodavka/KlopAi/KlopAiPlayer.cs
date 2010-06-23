﻿using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Media;
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
         while (model.CurrentPlayer == this && model.Cells.Any(c => c.Available) && !Worker.CancellationPending)
         {
            Thread.Sleep(200); // Simulate processing :)
            var cell = model.Cells.Where(c => c.Available).First();
            Worker.ReportProgress(0, cell); // Make calls to model in ReportProgress - on UI thread.
         }
      }

      #endregion
   }
}