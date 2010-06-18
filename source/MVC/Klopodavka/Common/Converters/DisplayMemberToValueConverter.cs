using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Windows.Data;
using FuncExpression = System.Linq.Expressions.Expression<System.Func<object, string>>;
using Func = System.Func<object, string>;


namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Converts object and Func<object,string> to string
   /// </summary>
   public class DisplayMemberToValueConverter : IMultiValueConverter
   {
      #region IMultiValueConverter Members


      /// <summary>
      /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
      /// </summary>
      /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
      /// <param name="targetType">The type of the binding target property.</param>
      /// <param name="parameter">The converter parameter to use.</param>
      /// <param name="culture">The culture to use in the converter.</param>
      /// <returns>
      /// A converted value.
      /// If the method returns null, the valid null value is used.
      /// A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will use the default value.
      /// A return value of <see cref="T:System.Windows.Data.Binding"/>.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
      /// </returns>
      public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
      {
         //Check that values array has valid length
         bool hasValidLength = values.Length == 2;

         if (!hasValidLength || values[0] == null || values[1] == null)
            return values;

         return EvaluateExpression(values[0], values[1]);
      }


      /// <summary>
      /// Converts a binding target value to the source binding values.
      /// </summary>
      /// <param name="value">The value that the binding target produces.</param>
      /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
      /// <param name="parameter">The converter parameter to use.</param>
      /// <param name="culture">The culture to use in the converter.</param>
      /// <returns>
      /// An array of values that have been converted from the target value back to the source values.
      /// </returns>
      public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }


      #region Public methods


      /// <summary>
      /// Evaluates the expression.
      /// </summary>
      /// <param name="argument">The argument.</param>
      /// <param name="expression">The expression.</param>
      /// <returns></returns>
      public static object EvaluateExpression(object argument, object expression)
      {
         var selectorExpr = expression as Delegate;
         if (selectorExpr != null)
         {
            return selectorExpr.DynamicInvoke(argument);
         }

         var lambda = expression as LambdaExpression;
         if (lambda != null)
         {
            return lambda.Compile().DynamicInvoke(argument);
         }

         //Get member selector
         Func<object, string> memberSelector = expression is FuncExpression
                                                  ? (expression as FuncExpression).Compile()
                                                  : (expression as Func);

         if (memberSelector == null)
         {
            return argument;
         }

         return memberSelector(argument);
      }


      #endregion


      #endregion
   }
}