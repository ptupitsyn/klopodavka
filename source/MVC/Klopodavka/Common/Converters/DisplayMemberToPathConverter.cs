using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Linq.Expressions;
using Jnj.ThirdDimension.WPFControls.Management;

namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Converts Expression<Func<object,string>> to display member path string
   /// </summary>
   public class DisplayMemberToPathConverter : IValueConverter
   {
      #region IValueConverter Members

      public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         var selectorExpr = value as LambdaExpression;
         if (selectorExpr != null)
         {
            return DisplayMemberHelper.GetMemberName(selectorExpr.Body as MemberExpression);
         }

         return string.Empty;
      }

      public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
