using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Media;
using KlopIfaces;

namespace KlopAi
{
    /// <summary>
    /// Base class for AI players for different rule sets.
    /// </summary>
    public abstract class KlopAiPlayerBase : IKlopPlayer
    {
        public int BasePosX { get; set; }

        public int BasePosY { get; set; }

        public Color Color { get; set; }

        public bool Human
        {
            get { return false; }
        }

        public string Name { get; set; }

        /// <summary>
        /// Sets the model. Must be called to activate CPU player.
        /// </summary>
        /// <param name="klopModel">The klop model.</param>
        public virtual void SetModel(IKlopModel klopModel)
        {
            if (klopModel == null)
                throw new ArgumentNullException();

            Model = klopModel;
            Model.PropertyChanged += ModelPropertyChanged;
            StartWorker();
        }

        /// <summary>
        /// Gets the worker.
        /// </summary>
        /// <value>The worker.</value>
        protected BackgroundWorker Worker
        {
            get
            {
                if (_worker == null)
                {
                    _worker = new BackgroundWorker { WorkerSupportsCancellation = true };
                    _worker.DoWork += WorkerMethod;
                }
                return _worker;
            }
        }

        protected abstract void MakeTurn();

        /// <summary>
        /// Handles the PropertyChanged event of the model.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentPlayer")
            {
                if (Model.CurrentPlayer == this)
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
        /// Starts the worker.
        /// </summary>
        private void StartWorker()
        {
            if (Model.CurrentPlayer != this) return;

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
        /// AI Worker method.
        /// </summary>
        private void WorkerMethod(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Lowest;

            lock (SyncRoot) // Sometimes workers can overlap
            {
                MakeTurn();
            }
        }

        protected IKlopModel Model;
        private static readonly object SyncRoot = new object();
        private BackgroundWorker _worker;
    }
}
