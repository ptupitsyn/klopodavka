#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

#endregion

namespace KlopViewWpf.Converters
{
   public class ColorToKlopBrushConverter : IValueConverter
   {
      #region Fields and Constants

      private static readonly Dictionary<Tuple<Color, bool>, Brush> ClopBrushes = new Dictionary<Tuple<Color, bool>, Brush>();

      #endregion

      #region IValueConverter implementation

      /// <summary>
      /// Converts a value.
      /// </summary>
      /// <param name="value">The value produced by the binding source.</param>
      /// <param name="targetType">The type of the binding target property.</param>
      /// <param name="parameter">The converter parameter to use.</param>
      /// <param name="culture">The culture to use in the converter.</param>
      /// <returns>
      /// A converted value. If the method returns null, the valid null value is used.
      /// </returns>
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var color = value is Color ? (Color) value : Colors.Transparent;
         var dead = parameter != null;
         var key = new Tuple<Color, bool>(color, dead);

         if (ClopBrushes.ContainsKey(key))
            return ClopBrushes[key];

         FrameworkElement image = new KlopImage {KlopColor = color};
         if (ClopBrushes.Count%2 == 0)
         {
            //Nice hack to orientate enemy image
            image.RenderTransform = new RotateTransform(180);
         }

         if (dead)
         {
            image.Effect = new DropShadowEffect {ShadowDepth = 0, BlurRadius = 40, Color = Colors.Black, Opacity = 1};
            var border = new Border
                            {
                               Child = image,
                               Effect = new BlurEffect {Radius = 60},
                            };
            image = border;
         }

         var brush = new VisualBrush {Visual = image};
         ClopBrushes[key] = brush;

         return brush;
      }

      /// <summary>
      /// Converts a value.
      /// </summary>
      /// <param name="value">The value that is produced by the binding target.</param>
      /// <param name="targetType">The type to convert to.</param>
      /// <param name="parameter">The converter parameter to use.</param>
      /// <param name="culture">The culture to use in the converter.</param>
      /// <returns>
      /// A converted value. If the method returns null, the valid null value is used.
      /// </returns>
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}