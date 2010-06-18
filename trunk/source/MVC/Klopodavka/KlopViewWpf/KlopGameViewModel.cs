#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using Common.Commands;
using KlopIfaces;
using KlopModel;

#endregion

namespace KlopViewWpf
{
   public class KlopGameViewModel
   {
      #region Fields and Constants

      private DispatcherTimer _demoTimer;
      private bool _isDemoRunning;
      private IKlopModel _klopModel;
      private DelegateCommand<IKlopCell> _makeTurnCommand;
      private HintPathHighlighter _pathHighlighter;
      private DelegateCommand _startDemoCommand;
      private DelegateCommand _stopDemoCommand;
      private DelegateCommand _undoCommand;

      #endregion

      #region Constructors

      public KlopGameViewModel()
      {
         FieldWidth = 15;
         FieldHeight = 18;
      }

      #endregion

      #region Public properties and indexers

      public IKlopCell ActiveCell
      {
         set { PathHighlighter.HighlightPath(value); }
      }

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

      public HintPathHighlighter PathHighlighter
      {
         get { return _pathHighlighter ?? (_pathHighlighter = new HintPathHighlighter(Model)); }
      }

      public DelegateCommand<IKlopCell> MakeTurnCommand
      {
         get { return _makeTurnCommand ?? (_makeTurnCommand = new DelegateCommand<IKlopCell>(cell => Model.MakeTurn(cell.X, cell.Y))); }
      }

      public DelegateCommand StartDemoCommand
      {
         get
         {
            return _startDemoCommand ?? (_startDemoCommand
                                         = new DelegateCommand(
                                              () =>
                                                 {
                                                    _isDemoRunning = true;
                                                    _startDemoCommand.RaiseCanExecuteChanged();
                                                    _stopDemoCommand.RaiseCanExecuteChanged();
                                                    DemoTimer.Start();
                                                 }, () => !_isDemoRunning));
         }
      }

      public DelegateCommand StopDemoCommand
      {
         get
         {
            return _stopDemoCommand ?? (_stopDemoCommand = new DelegateCommand(() =>
                                                                                  {
                                                                                     _demoTimer.Stop();
                                                                                     _isDemoRunning = false;
                                                                                     _startDemoCommand.RaiseCanExecuteChanged();
                                                                                     _stopDemoCommand.RaiseCanExecuteChanged();
                                                                                  }, () => _isDemoRunning));
         }
      }

      public DelegateCommand UndoCommand
      {
         get { return _undoCommand ?? (_undoCommand = new DelegateCommand(() => Model.UndoTurn())); }
      }

      #endregion

      #region Private and protected properties and indexers

      private DispatcherTimer DemoTimer
      {
         get
         {
            if (_demoTimer == null)
            {
               _demoTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(0.1)};
               _demoTimer.Tick += (a, e) =>
                                     {
                                        var avail = Model.Cells.Where(c => c.Available).ToList();
                                        if (avail.Count == 0)
                                        {
                                           _demoTimer.Stop();
                                           _isDemoRunning = false;
                                           _startDemoCommand.RaiseCanExecuteChanged();
                                           _stopDemoCommand.RaiseCanExecuteChanged();
                                           return;
                                        }
                                        Model.MakeTurn(avail[new Random().Next(avail.Count - 1)]);
                                     };
            }
            return _demoTimer;
         }
      }

      private int FieldWidth { get; set; }
      private int FieldHeight { get; set; }

      #endregion
   }
}