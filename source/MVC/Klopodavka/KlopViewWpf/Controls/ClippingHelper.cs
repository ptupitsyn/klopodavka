using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KlopViewWpf.Controls
{
   /// <summary>
   /// Helper for FrameworkElement-derived controls clipping.
   /// </summary>
   public class ClippingHelper
   {
      #region Fields and Constants

      public static readonly DependencyProperty ToBoundsProperty =
         DependencyProperty.RegisterAttached("ToBounds", typeof(bool),
                                             typeof(ClippingHelper), new PropertyMetadata(false, OnToBoundsPropertyChanged));

      #endregion

      #region Public methods

      /// <summary>
      /// Gets to bounds.
      /// </summary>
      /// <param name="depObj">The dep obj.</param>
      /// <returns></returns>
      public static bool GetToBounds(DependencyObject depObj)
      {
         return (bool)depObj.GetValue(ToBoundsProperty);
      }

      /// <summary>
      /// Sets to bounds.
      /// </summary>
      /// <param name="depObj">The dep obj.</param>
      /// <param name="clipToBounds">if set to <c>true</c> [clip to bounds].</param>
      public static void SetToBounds(DependencyObject depObj, bool clipToBounds)
      {
         depObj.SetValue(ToBoundsProperty, clipToBounds);
      }

      #endregion

      #region Private and protected methods

      /// <summary>
      /// Called when [to bounds property changed].
      /// </summary>
      /// <param name="d">The d.</param>
      /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
      private static void OnToBoundsPropertyChanged(DependencyObject d,
                                                    DependencyPropertyChangedEventArgs e)
      {
         var element = d as FrameworkElement;
         if (element == null) return;
         ClipToBounds(element);

         element.Loaded += ElementLoaded;
         element.SizeChanged += ElementSizeChanged;
      }

      /// <summary>
      /// Clips to bounds.
      /// </summary>
      /// <param name="element">The element.</param>
      private static void ClipToBounds(FrameworkElement element)
      {
         if (GetToBounds(element))
         {
            var rectangleGeometry = new RectangleGeometry
                                       {
                                          Rect = new Rect(0, 0, element.ActualWidth, element.ActualHeight)
                                       };
            var border = element as Border;
            if (border != null)
            {
               rectangleGeometry.RadiusX = rectangleGeometry.RadiusY = border.CornerRadius.BottomLeft;  // Take first one and hope all are equal :)
            }
            element.Clip = rectangleGeometry;
         }
         else
         {
            element.Clip = null;
         }
      }

      #endregion

      #region Event handlers

      /// <summary>
      /// Handles the SizeChanged event of the Element control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.SizeChangedEventArgs"/> instance containing the event data.</param>
      private static void ElementSizeChanged(object sender, SizeChangedEventArgs e)
      {
         ClipToBounds(sender as FrameworkElement);
      }

      /// <summary>
      /// Handles the Loaded event of the Element control.
      /// </summary>
      /// <param name="sender">The source of the event.</param>
      /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
      private static void ElementLoaded(object sender, RoutedEventArgs e)
      {
         ClipToBounds(sender as FrameworkElement);
      }

      #endregion
   }
}
