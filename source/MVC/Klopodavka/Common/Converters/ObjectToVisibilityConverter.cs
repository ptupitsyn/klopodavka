using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Converts object instance to visibility
   /// </summary>
   public class ObjectToVisibilityConverter : IValueConverter
   {
      #region IValueConverter Members

      /// <summary>
      /// Converts value to visibility
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns>Visibility.Visible if value != null. Visibility.Collapsed otherwise</returns>
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         return (value == null) ? Visibility.Collapsed : Visibility.Visible;
      }

      /// <summary>
      /// Not implemented
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns></returns>
      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
