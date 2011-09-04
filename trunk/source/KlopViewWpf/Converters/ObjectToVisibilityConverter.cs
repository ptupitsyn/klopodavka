using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace KlopViewWpf.Converters
{
   /// <summary>
   /// Converts True booleans, non-empty strings and non-null objects to Visibility.Visible; others to Visibility.Collapsed.
   /// </summary>
   public class ObjectToVisibilityConverter : IValueConverter
   {
      #region IValueConverter implementation

      /// <summary>
      /// Modifies the source data before passing it to the target for display in the UI.
      /// </summary>
      /// <param name="value">The source data being passed to the target.</param>
      /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the target dependency property.</param>
      /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
      /// <param name="culture">The culture of the conversion.</param>
      /// <returns>
      /// The value to be passed to the target dependency property.
      /// </returns>
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         var inverted = parameter != null;
         return (ObjectToBool(value) ^ inverted) ? Visibility.Visible : Visibility.Collapsed;
      }


      /// <summary>
      /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> bindings.
      /// </summary>
      /// <param name="value">The target data being passed to the source.</param>
      /// <param name="targetType">The <see cref="T:System.Type"/> of data expected by the source object.</param>
      /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
      /// <param name="culture">The culture of the conversion.</param>
      /// <returns>
      /// The value to be passed to the source object.
      /// </returns>
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return null;
      }

      #endregion


      #region Public methods

      /// <summary>
      /// Converts object to boolean representation.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <returns></returns>
      public static bool ObjectToBool(object value)
      {
         if (value == null) return false;
         if (value is bool) return (bool) value;
         if (value is string) return !string.IsNullOrEmpty(value as string);

         return true;
      }

      #endregion
   }
}