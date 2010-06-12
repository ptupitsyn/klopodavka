#region Usings

using System.Collections.Generic;
using System.Windows.Media;
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

      #endregion

      #region Constructors

      public KlopGameViewModel()
      {
         FieldWidth = 15;
         FieldHeight = 20;
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
                                   new KlopPlayer {BasePosX = 2, BasePosY = FieldHeight - 3, Color = Colors.Red, Human = true, Name = "Player 2"}
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

      #endregion

      #region Private and protected properties and indexers

      private int FieldWidth { get; set; }
      private int FieldHeight { get; set; }

      #endregion
   }
}