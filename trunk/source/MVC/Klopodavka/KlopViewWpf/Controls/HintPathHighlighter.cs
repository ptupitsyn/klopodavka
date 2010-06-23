using System.ComponentModel;
using KlopIfaces;

namespace KlopViewWpf
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
         cell.Highlighted = true;
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