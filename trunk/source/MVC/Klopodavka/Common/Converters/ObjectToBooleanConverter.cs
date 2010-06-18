using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Converts object instance to (instance != null) value
   /// </summary>
   public class ObjectToBooleanConverter : IValueConverter
   {
      #region IValueConverter Members
      /// <summary>
      /// Converts value to boolean value
      /// </summary>
      /// <param name="value"></param>
      /// <param name="targetType"></param>
      /// <param name="parameter"></param>
      /// <param name="culture"></param>
      /// <returns>True if value != null. False otherwise</returns>
      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         string strVal = value as string;
         if (strVal != null) return !string.IsNullOrEmpty(strVal);

         return value != null;
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
