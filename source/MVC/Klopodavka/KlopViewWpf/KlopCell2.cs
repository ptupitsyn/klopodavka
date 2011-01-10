﻿#region Usings

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using KlopIfaces;
using KlopViewWpf.Converters;

#endregion


namespace KlopViewWpf
{
   /// <summary>
   /// KlopCell control. Written without XAML to improve performance (InitializeComponent is very slow).
   /// </summary>
   public class KlopCell2 : FrameworkElement
   {
      #region Fields and Constants

      private static readonly Brush AvailableBrush;

      public static readonly DependencyProperty BackgroundProperty =
         DependencyProperty.Register("Background", typeof (Brush), typeof (KlopCell2),
                                     new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

      private static readonly Pen BorderPen = new Pen(Brushes.Gray, 0.5);

      public static readonly DependencyProperty CellProperty =
         DependencyProperty.Register("Cell", typeof (IKlopCell), typeof (KlopCell2), new UIPropertyMetadata(null, OnKlopCellChanged));

      public static readonly DependencyProperty ForegroundProperty =
         DependencyProperty.Register("Foreground", typeof (Brush), typeof (KlopCell2),
                                     new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

      private static readonly Brush HoverBrush = Brushes.Yellow;


      public static readonly DependencyProperty ModelProperty =
         DependencyProperty.Register("Model", typeof (IKlopModel), typeof (KlopCell2), new UIPropertyMetadata(null, OnModelChanged));

      private Brush _background;
      private IKlopCell _cell;
      private Brush _foreground;
      private IKlopModel _model;
      private Storyboard _opacityStoryboard;
      private Storyboard _zoomStoryboard;

      #endregion


      #region Constructors

      static KlopCell2()
      {
         AvailableBrush = new SolidColorBrush(Colors.Gray) {Opacity = 0.3};
         AvailableBrush.Freeze();
         BorderPen.Freeze();
         HoverBrush.Freeze();
      }


      public KlopCell2()
      {
         FocusVisualStyle = null;
      }

      #endregion


      #region Public properties and indexers

      public IKlopModel Model
      {
         get { return (IKlopModel) GetValue(ModelProperty); }
         set { SetValue(ModelProperty, value); }
      }

      public IKlopCell Cell
      {
         get { return (IKlopCell) GetValue(CellProperty); }
         set { SetValue(CellProperty, value); }
      }

      public Brush Background
      {
         get { return _background ?? (_background = (Brush) GetValue(BackgroundProperty)); }
         set
         {
            SetValue(BackgroundProperty, value);
            _background = value;
         }
      }

      public Brush Foreground
      {
         get { return _foreground ?? (_foreground = (Brush) GetValue(ForegroundProperty)); }
         set
         {
            SetValue(ForegroundProperty, value);
            _foreground = value;
         }
      }

      #endregion


      #region Private and protected properties and indexers

      private Storyboard OpacityStoryboard
      {
         get
         {
            if (_opacityStoryboard == null)
            {
               _opacityStoryboard = new Storyboard();
               var animation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.3)));
               _opacityStoryboard.Children.Add(animation);
               Storyboard.SetTarget(animation, this);
               Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
            }
            return _opacityStoryboard;
         }
      }

      private Storyboard ZoomStoryboard
      {
         get
         {
            if (_zoomStoryboard == null)
            {
               _zoomStoryboard = new Storyboard();
               RenderTransform = new ScaleTransform();
               RenderTransformOrigin = new Point(0.5, 0.5);

               var duration = new Duration(TimeSpan.FromSeconds(0.5));
               var animX = new DoubleAnimation(2, 1, duration);
               var animY = new DoubleAnimation(2, 1, duration);

               animX.EasingFunction = animY.EasingFunction = new BounceEase {Bounces = 2, Bounciness = 1.8, EasingMode = EasingMode.EaseIn};
               
               _zoomStoryboard.Children.Add(animX);
               _zoomStoryboard.Children.Add(animY);

               Storyboard.SetTarget(animX, this);
               Storyboard.SetTarget(animY, this);

               Storyboard.SetTargetProperty(animX, new PropertyPath("RenderTransform.ScaleX"));
               Storyboard.SetTargetProperty(animY, new PropertyPath("RenderTransform.ScaleY"));
            }
            return _zoomStoryboard;
         }
      }

      #endregion


      #region Private and protected methods

      private static void OnModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((KlopCell2) d).OnModelChanged(e);
      }


      private void OnModelChanged(DependencyPropertyChangedEventArgs e)
      {
         _model = Model; // Cache for faster access
         UpdateBrushes();
      }


      private static void OnKlopCellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         ((KlopCell2) d).OnKlopCellChanged(e);
      }


      private void OnKlopCellChanged(DependencyPropertyChangedEventArgs e)
      {
         var oldCell = e.OldValue as IKlopCell;
         if (oldCell != null)
         {
            oldCell.PropertyChanged -= Cell_PropertyChanged;
         }

         _cell = Cell; // Cache value for faster access
         _cell.PropertyChanged += Cell_PropertyChanged;
         UpdateBrushes();
      }


      private void UpdateBrushes()
      {
         Brush bg = Brushes.Transparent;
         var fg = bg;

         if (_cell != null && _model != null)
         {
            if (_cell.Owner != null)
            {
               switch (_cell.State)
               {
                  case ECellState.Base:
                     fg = ColorToKlopBrushConverter.GetBrush(_cell.Owner.Color, false);
                     bg = Brushes.Gray;
                     break;
                  case ECellState.Alive:
                     fg = ColorToKlopBrushConverter.GetBrush(_cell.Owner.Color, false);
                     break;
                  case ECellState.Dead:
                     fg = ColorToKlopBrushConverter.GetBrush(_cell.Owner.Color, true);
                     break;
               }
            }

            if (_cell.Available && _model.CurrentPlayer.Human)
            {
               bg = AvailableBrush;
            }

            if (_cell.Highlighted)
            {
               bg = HoverBrush;
               Cursor = Cursors.Hand;
            }
            else
            {
               Cursor = Cursors.Arrow;
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
         if (_cell == null) return;
         _cell.Highlighted = true;
      }


      protected override void OnMouseLeave(MouseEventArgs e)
      {
         if (_cell == null) return;
         _cell.Highlighted = false;
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
         if (e.PropertyName == "State")
         {
            // TODO: Delay animations through static cache?
            OpacityStoryboard.Begin();
            ZoomStoryboard.Begin();
         }
         UpdateBrushes();
      }

      #endregion
   }
}