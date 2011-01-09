#region Usings

using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using KlopIfaces;
using KlopViewWpf.Converters;

#endregion

namespace KlopViewWpf
{
   /// <summary>
   /// Interaction logic for KlopCell.xaml
   /// </summary>
   public class KlopCell2 : FrameworkElement
   {
      #region Fields and Constants

      public static readonly DependencyProperty BackgroundProperty =
         DependencyProperty.Register("Background", typeof(Brush), typeof(KlopCell2),
                                     new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));


      private static readonly Pen BorderPen = new Pen(Brushes.Gray, 0.5);

      public static readonly DependencyProperty CellProperty =
         DependencyProperty.Register("Cell", typeof(IKlopCell), typeof(KlopCell2), new UIPropertyMetadata(null, OnKlopCellChanged));

      public static readonly DependencyProperty ForegroundProperty =
         DependencyProperty.Register("Foreground", typeof(Brush), typeof(KlopCell2),
                                     new FrameworkPropertyMetadata(Brushes.Transparent, FrameworkPropertyMetadataOptions.AffectsRender));

      #endregion

      #region Constructors

      public KlopCell2()
      {
         BorderPen.Freeze();
      }

      #endregion

      #region Public properties and indexers

      public IKlopCell Cell
      {
         get { return (IKlopCell)GetValue(CellProperty); }
         set { SetValue(CellProperty, value); }
      }

      public Brush Background
      {
         get { return (Brush)GetValue(BackgroundProperty); }
         set { SetValue(BackgroundProperty, value); }
      }

      public Brush Foreground
      {
         get { return (Brush)GetValue(ForegroundProperty); }
         set { SetValue(ForegroundProperty, value); }
      }

      #endregion

      #region Private and protected methods

      private static void OnKlopCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((KlopCell2)d).OnKlopCellChanged(e);
      }

      private void OnKlopCellChanged(DependencyPropertyChangedEventArgs e)
      {
         var oldCell = e.OldValue as IKlopCell;
         if (oldCell != null)
         {
            Cell.PropertyChanged -= Cell_PropertyChanged;
         }

         Cell.PropertyChanged += Cell_PropertyChanged;
         UpdateBrushes();
      }

      private void UpdateBrushes()
      {
         Brush bg = Brushes.Transparent;
         var fg = bg;

         if (Cell != null && Cell.Owner != null)
         {
            switch (Cell.State)
            {
               case ECellState.Base:
                  fg = ColorToKlopBrushConverter.GetBrush(Cell.Owner.Color, false);
                  bg = Brushes.Gray;
                  break;
               case ECellState.Alive:
                  fg = ColorToKlopBrushConverter.GetBrush(Cell.Owner.Color, false);
                  break;
               case ECellState.Dead:
                  fg = ColorToKlopBrushConverter.GetBrush(Cell.Owner.Color, true);
                  break;
            }
         }

         if (fg != Foreground)
         {
            Foreground = fg;
         }

         if (bg != Background)
         {
            Background = bg;
         }
      }

      protected override GeometryHitTestResult HitTestCore(GeometryHitTestParameters hitTestParameters)
      {
         return new GeometryHitTestResult(this, IntersectionDetail.FullyContains);
      }

      protected override void OnRender(DrawingContext drawingContext)
      {
         if (Background != null && Background != Brushes.Transparent)
         {
            drawingContext.DrawRectangle(Background, null, new Rect(RenderSize));
         }
         drawingContext.DrawRectangle(Foreground, BorderPen, new Rect(RenderSize));
      }

      protected override Size ArrangeOverride(Size finalSize)
      {
         return finalSize;
      }

      protected override Size MeasureOverride(Size availableSize)
      {
         return availableSize;
      }

      protected override void OnMouseEnter(MouseEventArgs e)
      {
         if (Cell == null) return;
         Cell.Highlighted = true;
      }

      protected override void OnMouseLeave(MouseEventArgs e)
      {
         if (Cell == null) return;
         Cell.Highlighted = false;
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the PropertyChanged event of the Cell control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
      private void Cell_PropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         UpdateBrushes();
      }

      #endregion
   }
}