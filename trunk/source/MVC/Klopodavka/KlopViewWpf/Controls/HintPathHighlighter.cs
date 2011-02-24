#region Usings

using System;
using System.Collections.Generic;
using KlopAi;
using KlopIfaces;

#endregion

namespace KlopViewWpf.Controls
{
   public class HintPathHighlighter
   {
      #region Fields and Constants

      private readonly IKlopModel _model;
      private readonly Dictionary<IKlopCell, int> _highlightedCells = new Dictionary<IKlopCell, int>();
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
         // Deselect all
         _highlightedCells.Clear();
         if (!_model.CurrentPlayer.Human) return;

         var path = PathFinder.FindPath(_model.CurrentPlayer.BasePosX, _model.CurrentPlayer.BasePosY, cell.X, cell.Y, _model.CurrentPlayer);
         var i = path.Count;
         foreach (var klopCell in path)
         {
            _highlightedCells[klopCell] = i--;
         }

         InvokeHighlightChanged();
      }

      /// <summary>
      /// Determines whether the specified cell is highlighted. When highlighted, returns positive number indicating path length.
      /// If not highlighted, returns -1.
      /// </summary>
      /// <param name="cell">The cell.</param>
      public int GetPathLength(IKlopCell cell)
      {
         return cell != null && _highlightedCells.ContainsKey(cell) ? _highlightedCells[cell] : -1;
      }

      /// <summary>
      /// Determines whether the specified cell is highlighted.
      /// </summary>
      /// <param name="cell">The cell.</param>
      /// <returns>
      /// 	<c>true</c> if the specified cell is highlighted; otherwise, <c>false</c>.
      /// </returns>
      public bool IsHighlighted(IKlopCell cell)
      {
         return cell != null && _highlightedCells.ContainsKey(cell);
      }

      /// <summary>
      /// Unhighlights cells.
      /// </summary>
      public void Unhighlight()
      {
         _highlightedCells.Clear();
         InvokeHighlightChanged();
      }

      /// <summary>
      /// Invokes the highlight changed.
      /// </summary>
      public void InvokeHighlightChanged()
      {
         EventHandler handler = HighlightChanged;
         if (handler != null) handler(this, new EventArgs());
      }

      #endregion

      #region Private and protected properties and indexers

      private KlopPathFinder PathFinder
      {
         get { return _pathFinder ?? (_pathFinder = new KlopPathFinder(_model)); }
      }

      #endregion

      #region Events

      public event EventHandler HighlightChanged;

      #endregion
   }
}