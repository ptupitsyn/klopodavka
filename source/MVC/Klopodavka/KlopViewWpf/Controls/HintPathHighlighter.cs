using System.Linq;
using KlopAi;
using KlopIfaces;

namespace KlopViewWpf.Controls
{
   public class HintPathHighlighter
   {
      #region Fields and Constants

      private readonly IKlopModel _model;
      private KlopPathFinder _pathFinder;

      #endregion

      #region Constructors

      public HintPathHighlighter(IKlopModel model)
      {
         _model = model;
         _model.PropertyChanged += (a, e) => { if (e.PropertyName == "CurrentPlayer") Unhighlight(); };
      }

      #endregion

      #region Public methods

      public void HighlightPath(IKlopCell cell)
      {
         if (!_model.CurrentPlayer.Human)
         {
            // Deselect all
            Unhighlight();
            return;
         }

         var path = PathFinder.FindPath(_model.CurrentPlayer.BasePosX, _model.CurrentPlayer.BasePosY, cell.X, cell.Y, _model.CurrentPlayer);

         foreach (var klopCell in _model.Cells.Where(c => !path.Contains(c)))
         {
            klopCell.Highlighted = false;
         }

         foreach (var klopCell in path)
         {
            klopCell.Highlighted = true;
         }
      }

      private void Unhighlight()
      {
         foreach (var klopCell in _model.Cells.Where(c => c.Highlighted))
         {
            klopCell.Highlighted = false;
         }
      }

      #endregion

      #region Private and protected properties and indexers

      private KlopPathFinder PathFinder
      {
         get { return _pathFinder ?? (_pathFinder = new KlopPathFinder(_model)); }
      }

      #endregion

   }
}