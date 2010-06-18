using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;


namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Multivalue converter for converting multiple boolean values to Visibility enumaration value
   /// </summary>
   public class MultiBoolToVisibilityConverter : IMultiValueConverter
   {
      #region IMultiValueConverter Members


      public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
      {
         if (values != null)
         {
            ///Use Enumerable.Any if parameter is Or, Enumerable.All otherwise
            Func<bool> funcAny = () => values.Any(value => value is bool && (bool) value);
            Func<bool> funcAll = () => values.All(value => value is bool && (bool) value);
            Func<bool> func = parameter as string == "Or" ? funcAny : funcAll;

            return func() ? Visibility.Visible : Visibility.Hidden;
         }

         return Visibility.Hidden;
      }


      public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }


      #endregion
   }
}