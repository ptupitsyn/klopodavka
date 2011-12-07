using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using KlopIfaces;


namespace KlopViewWpf.Converters
{
   /// <summary>
   /// Takes Player and Model objects.
   /// Returns Visible when player is defeated.
   /// </summary>
   internal class DefeatedPlayerVisibilityConverter : IMultiValueConverter
   {
      #region IMultiValueConverter implementation

      public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
      {
         var defeatedPlayers = values.OfType<IEnumerable<IKlopPlayer>>().FirstOrDefault();
         var player = values.OfType<IKlopPlayer>().FirstOrDefault();
         return player != null && defeatedPlayers != null && defeatedPlayers.Contains(player) ? Visibility.Visible : Visibility.Collapsed;
      }


      public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }

      #endregion
   }
}