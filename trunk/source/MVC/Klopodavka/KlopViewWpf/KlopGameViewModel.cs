#region Usings

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Common.Commands;
using KlopAi;
using KlopIfaces;
using KlopModel;
using KlopViewWpf.Controls;

#endregion

namespace KlopViewWpf
{
   public class KlopGameViewModel
   {
      #region Fields and Constants

      private readonly int _turnLength;
      private IKlopModel _klopModel;
      private DelegateCommand<IKlopCell> _makeTurnCommand;
      private HintPathHighlighter _pathHighlighter;
      private DelegateCommand<IKlopCell> _setCurrentCellCommand;
      private DelegateCommand _undoCommand;
      private readonly int _baseDist;

      #endregion

      #region Constructors

      public KlopGameViewModel()
      {
         FieldWidth = 40;
         FieldHeight = 40;
         _turnLength = 10;
         _baseDist = 4;
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
               var aiPlayer = new KlopAiPlayer {BasePosX = _baseDist, BasePosY = FieldHeight - _baseDist - 1, Color = Colors.Red, Name = "Луноход 1"};
               var aiPlayer2 = new KlopAiPlayer {BasePosX = FieldWidth - _baseDist - 1, BasePosY = _baseDist, Color = Colors.Green, Name = "Луноход 2"};
               var humanPlayer = new KlopPlayer {BasePosX = FieldWidth - _baseDist - 1, BasePosY = _baseDist, Color = Colors.Blue, Human = true, Name = "Player 1"};

               var players = new List<IKlopPlayer> { humanPlayer, aiPlayer };
               _klopModel = new KlopModel.KlopModel(FieldWidth, FieldHeight, players, _turnLength);

               aiPlayer.SetModel(_klopModel);
               aiPlayer2.SetModel(_klopModel);
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
         get { return _makeTurnCommand ?? (_makeTurnCommand = new DelegateCommand<IKlopCell>(MakeTurn)); }
      }


      public DelegateCommand UndoCommand
      {
         get { return _undoCommand ?? (_undoCommand = new DelegateCommand(() => Model.UndoTurn())); }
      }


      public DelegateCommand<IKlopCell> SetActiveCellCommand
      {
         get { return _setCurrentCellCommand ?? (_setCurrentCellCommand = new DelegateCommand<IKlopCell>(c => ActiveCell = c)); }
      }

      #endregion

      #region Private and protected properties and indexers

      private int FieldWidth { get; set; }
      private int FieldHeight { get; set; }

      #endregion

      #region Private and protected methods

      private void MakeTurn(IKlopCell cell)
      {
         if (cell.Available)
         {
            Model.MakeTurn(cell);
         }
         else if (cell.Highlighted)
         {
            // Cell is highlighted - perform multiple turns:
            while (Model.RemainingKlops > 0)  
            {
               var currentCell = Model.Cells.FirstOrDefault(c => c.Highlighted && c.Available);
               if (currentCell == null) break;
               Model.MakeTurn(currentCell);
               if (currentCell == cell) break; // Destination reached
            }
         }
      }

      #endregion
   }
}