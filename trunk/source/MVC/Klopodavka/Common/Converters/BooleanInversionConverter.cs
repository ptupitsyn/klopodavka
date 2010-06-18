#region Copyright (C) 1994-2009, Johnson & Johnson PRD, LLC.

//---------------------------------------------------------------------------*
//
//    BooleanInversionConverter.cs: Inverts boolean value.
//
//---
//
//    Copyright (C) 1994-2010, Johnson & Johnson PRD, LLC.
//    All Rights Reserved.
//
//    Pavel Tupitsin, 03/2010
//
//---------------------------------------------------------------------------*/

#endregion

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Inverts boolean value.
   /// </summary>
   public class BooleanInversionConverter : IValueConverter
   {
      #region IValueConverter implementation


      /// <summary>
      /// Converts a value. 
      /// </summary>
      /// <returns>
      /// A converted value. If the method returns null, the valid null value is used.
      /// </returns>
      /// <param name="value">The value produced by the binding source.
      ///                 </param><param name="targetType">The type of the binding target property.
      ///                 </param><param name="parameter">The converter parameter to use.
      ///                 </param><param name="culture">The culture to use in the converter.
      ///                 </param>
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         if (!(value is bool))

            return DependencyProperty.UnsetValue;


         return !(bool) value;
      }


      /// <summary>
      /// Converts a value. 
      /// </summary>
      /// <returns>
      /// A converted value. If the method returns null, the valid null value is used.
      /// </returns>
      /// <param name="value">The value that is produced by the binding target.
      ///                 </param><param name="targetType">The type to convert to.
      ///                 </param><param name="parameter">The converter parameter to use.
      ///                 </param><param name="culture">The culture to use in the converter.
      ///                 </param>
      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }


      #endregion
   }
}