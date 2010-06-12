#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace KlopViewWpf
{
   public class ColorToKlopBrushConverter : IValueConverter
   {
      #region Fields and Constants

      private static readonly Dictionary<Color, Brush> ClopBrushes = new Dictionary<Color, Brush>();

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

         if (ClopBrushes.ContainsKey(color))
            return ClopBrushes[color];

         var image = new KlopImage {KlopColor = color};
         if (ClopBrushes.Count == 0)
         {
            //Nice hack to orientate enemy image
            image.RenderTransform = new RotateTransform(180);
         }

         var brush = new VisualBrush {Visual = image};
         ClopBrushes[color] = brush;

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