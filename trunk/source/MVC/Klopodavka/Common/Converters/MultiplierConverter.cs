using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Converts a value by multiplying it with a value passed as converter parameter
   /// </summary>
   class MultiplierConverter : IValueConverter
   {
      #region IValueConverter Members

      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         if (value != null)
         {
            double valueToConvert = System.Convert.ToDouble(value);
            double parameterValue;
            string parameterString = parameter as string;
            if (double.TryParse(parameterString, out parameterValue))
            {
               return valueToConvert * parameterValue;
            }
         }            
         
         return value;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
