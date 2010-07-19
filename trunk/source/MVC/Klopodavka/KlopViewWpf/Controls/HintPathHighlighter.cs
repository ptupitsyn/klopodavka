using System.ComponentModel;
using System.Linq;
using KlopAi;
using KlopIfaces;

namespace KlopViewWpf.Controls
{
   public class HintPathHighlighter
   {
      #region Fields and Constants

      private readonly IKlopModel _model;

      #endregion

      #region Constructors

      public HintPathHighlighter(IKlopModel model)
      {
         _model = model;
         foreach (IKlopCell cell in _model.Cells)
         {
            cell.PropertyChanged += cell_PropertyChanged;
         }
      }

      #endregion

      #region Public methods

      public void HighlightPath(IKlopCell cell)
      {
         var pathFinder = new KlopPathFinder(_model, _model.CurrentPlayer);
         var path = pathFinder.FindPath(_model.CurrentPlayer.BasePosX, _model.CurrentPlayer.BasePosY, cell.X, cell.Y);
         
         foreach (var klopCell in _model.Cells.Where(c => !path.Contains(c)))
         {
            klopCell.Highlighted = false;
         }

         foreach (var klopCell in path)
         {
            klopCell.Highlighted = true;
         }
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the PropertyChanged event of the cell control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
      private void cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         var cell = sender as IKlopCell;
         if (cell != null && e.PropertyName == "Highlighted")
         {
            //TODO: Highlight path
         }
      }

      #endregion
   }
}