using System;
using System.Globalization;
using System.Windows.Data;


namespace KlopViewWpf.Converters
{
   /// <summary>
   /// Multiplies value by parameter. Works with Double.
   /// </summary>
   public class MultiplicationConverter : IValueConverter
   {
      #region IValueConverter implementation

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (!(value is double)) return 0;
         var val = (double) value;
         if (parameter is int)
         {
            return val*((int) parameter);
         }
         if (parameter is double)
         {
            return val*((double) parameter);
         }
         double par;
         if (parameter is string && double.TryParse((string)parameter, out par))
         {
            return val * par;
         }

         return val;
      }


      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (!(value is double)) return 0;
         var val = (double) value;
         if (parameter is int)
         {
            return val/((int) parameter);
         }
         if (parameter is double)
         {
            return val/((double) parameter);
         }
         double par;
         if (parameter is string && double.TryParse((string)parameter, out par))
         {
            return val / par;
         }

         return val;
      }

      #endregion
   }
}