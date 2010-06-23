#region Usings

using System;
using System.Globalization;
using System.Windows.Data;
using KlopIfaces;

#endregion

namespace KlopViewWpf.Converters
{
   /// <summary>
   /// Maintains grid cells aspect ratio.
   /// </summary>
   internal class ClopGridWidthConverter : IMultiValueConverter
   {
      #region IMultiValueConverter implementation

      public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
      {
         if (values.Length < 2 || !(values[0] is double && values[1] is IKlopModel)) return values;
         var model = values[1] as IKlopModel;
         var height = (double) values[0];
         return height*model.FieldWidth/model.FieldHeight;
      }

      public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}