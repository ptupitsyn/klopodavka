#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using KlopIfaces;
using KlopModel;

#endregion

namespace KlopViewWpf
{
   public class KlopGameViewModel
   {
      #region Fields and Constants

      private IKlopModel _klopModel;
      private DelegateCommand<IKlopCell> _makeTurnCommand;
      private DelegateCommand<object> _startDemoCommand;
      private bool _isDemoRunning;

      #endregion

      #region Constructors

      public KlopGameViewModel()
      {
         FieldWidth = 30;
         FieldHeight = 30;
      }

      #endregion

      #region Public properties and indexers

      public IKlopModel Model
      {
         get
         {
            if (_klopModel == null)
            {
               var players = new List<IKlopPlayer>
                                {
                                   new KlopPlayer {BasePosX = FieldWidth - 3, BasePosY = 2, Color = Colors.Blue, Human = true, Name = "Player 1"},
                                   new KlopPlayer {BasePosX = 2, BasePosY = FieldHeight - 3, Color = Colors.Red, Human = false, Name = "Player 2"},
                                };
               _klopModel = new KlopModel.KlopModel(FieldWidth, FieldHeight, players);
            }
            return _klopModel;
         }
      }

      public DelegateCommand<IKlopCell> MakeTurnCommand
      {
         get
         {
            return _makeTurnCommand ?? (_makeTurnCommand = new DelegateCommand<IKlopCell>(cell => Model.MakeTurn(cell.X, cell.Y)));
         }
      }

      public DelegateCommand<object> StartDemoCommand
      {
         get
         {
            return _startDemoCommand ?? (_startDemoCommand
                                         = new DelegateCommand<object>(
                                              o =>
                                                 {
                                                    _isDemoRunning = true;
                                                    _startDemoCommand.RaiseCanExecuteChanged();
                                                    var timer = new DispatcherTimer() {Interval = TimeSpan.FromSeconds(0.5)};
                                                    timer.Tick += (a, e) =>
                                                                     {
                                                                        var avail = Model.Cells.Where(c => c.Available).FirstOrDefault();
                                                                        if (avail == null)
                                                                        {
                                                                           timer.Stop();
                                                                           _isDemoRunning = false;
                                                                           _startDemoCommand.RaiseCanExecuteChanged();
                                                                           return;
                                                                        }
                                                                        Model.MakeTurn(avail);
                                                                     };

                                                 }, o => !_isDemoRunning));
         }
      }

      #endregion

      #region Private and protected properties and indexers

      private int FieldWidth { get; set; }
      private int FieldHeight { get; set; }

      #endregion
   }
}