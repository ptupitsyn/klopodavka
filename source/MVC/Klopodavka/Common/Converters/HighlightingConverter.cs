using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections;
using System.Windows.Media;
using System.Windows;

namespace Jnj.ThirdDimension.WPFControls.Converters
{
   /// <summary>
   /// Converts multiple values into background color for item
   /// </summary>
   public class HighlightingConverter : DependencyObject, IMultiValueConverter
   {

      #region ::: Fields :::
      // Using a DependencyProperty as the backing store for HighlightColorBrush.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty HighlightBrushProperty =
          DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(HighlightingConverter), new UIPropertyMetadata(null));

      // Using a DependencyProperty as the backing store for InvertHighlightBrush.  This enables animation, styling, binding, etc...
      public static readonly DependencyProperty InvertHighlightBrushProperty =
          DependencyProperty.Register("InvertHighlightBrush", typeof(Brush), typeof(HighlightingConverter), new UIPropertyMetadata(null));
      
      #endregion


      #region ::: Properties :::
      /// <summary>
      /// Gets or sets the highlighting brush color
      /// </summary>
      public Brush HighlightBrush
      {
         get { return (Brush)GetValue(HighlightBrushProperty); }
         set { SetValue(HighlightBrushProperty, value); }
      }

      /// <summary>
      /// Gets or sets the inverted highlighting brush color
      /// </summary>
      public Brush InvertHighlightBrush
      {
         get { return (Brush)GetValue(InvertHighlightBrushProperty); }
         set { SetValue(InvertHighlightBrushProperty, value); }
      } 
      #endregion


      #region IMultiValueConverter Members

      public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
      {
         //if (values.Length < 4)
         //{
         //   throw new ArgumentException("Not enough arguments for valid conversion");
         //}

         //IList keySelectedItems = values[0] as IList;
         //IList valueSelectedItems = values[1] as IList;
         //bool inverseHighlighting = false;//(bool)values[2];
         //object item = values[3];
         //bool keysSelected = keySelectedItems != null && keySelectedItems.Count != 0;
         //bool valuesSelected = valueSelectedItems != null && valueSelectedItems.Count != 0; 
         //bool highlight = false;

         //if (!keysSelected && !valuesSelected)
         //{
         //   return null;
         //}
         //else if (keysSelected && valuesSelected)
         //{
         //   highlight = keySelectedItems.Contains(item.Key) && valueSelectedItems.Contains(item.Value);
         //}
         //else if (keysSelected)
         //{
         //   highlight = keySelectedItems.Contains(item.Key);
         //}
         //else if (valuesSelected)
         //{
         //   highlight = valueSelectedItems.Contains(item.Value);
         //}

         //if (inverseHighlighting)
         //   highlight = !highlight;

         //return highlight ? HighlightBrush : InvertHighlightBrush;
         return null;
      }

      public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}
