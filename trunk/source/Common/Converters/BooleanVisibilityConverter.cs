using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.Converters
{
   /// <summary>
   /// Extended boolean-visibility converter.
   /// </summary>
   public class BooleanVisibilityConverter : IValueConverter
   {
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
      public object Convert(object value, Type targetType,
                            object parameter, CultureInfo culture)
      {
         return Inverted
                   ? BoolToVisibility(value)
                   : VisibilityToBool(value);
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
      public object ConvertBack(object value, Type targetType,
                                object parameter, CultureInfo culture)
      {
         return Inverted
                   ? VisibilityToBool(value)
                   : BoolToVisibility(value);
      }


      #endregion


      #region Public properties and indexers


      /// <summary>
      /// Gets or sets a value indicating whether this <see cref="BooleanVisibilityConverter"/> is inverted.
      /// </summary>
      /// <value><c>true</c> if inverted; otherwise, <c>false</c>.</value>
      public bool Inverted { get; set; }


      /// <summary>
      /// Gets or sets a value indicating whether this <see cref="BooleanVisibilityConverter"/> is not.
      /// </summary>
      /// <value><c>true</c> if not; otherwise, <c>false</c>.</value>
      public bool Not { get; set; }


      #endregion


      #region Private and protected methods


      /// <summary>
      /// Visibilities to bool.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <returns></returns>
      private object VisibilityToBool(object value)
      {
         if (!(value is Visibility))

            return DependencyProperty.UnsetValue;


         return (((Visibility) value) == Visibility.Visible) ^ Not;
      }


      /// <summary>
      /// Bools to visibility.
      /// </summary>
      /// <param name="value">The value.</param>
      /// <returns></returns>
      private object BoolToVisibility(object value)
      {
         if (!(value is bool))

            return DependencyProperty.UnsetValue;


         return ((bool) value ^ Not)
                   ? Visibility.Visible
                   : Visibility.Collapsed;
      }


      #endregion
   }
}