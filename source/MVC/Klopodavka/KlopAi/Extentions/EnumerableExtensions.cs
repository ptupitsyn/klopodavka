using System;
using System.Collections.Generic;
using System.Linq;

namespace KlopAi.Extentions
{
   public static class EnumerableExtensions
   {
      #region Fields and Constants

      private static readonly Random _random = new Random();

      #endregion

      #region Public methods

      /// <summary>
      /// Returns random item from list.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="list">The list.</param>
      /// <returns></returns>
      public static T Random<T>(this IList<T> list)
      {
         return list.Count == 0 ? default(T) : list[_random.Next(list.Count - 1)];
      }

      /// <summary>
      /// Returns random item from enumerable.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="enumerable">The enumerable.</param>
      /// <returns></returns>
      public static T Random<T>(this IEnumerable<T> enumerable)
      {
         return enumerable.ToList().Random();
      }


      /// <summary>
      /// Get the highest ranked element, judging by compareBy function results.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="enumerable">The enumerable.</param>
      /// <param name="compareBy">The compare by.</param>
      /// <returns></returns>
      public static T Highest<T>(this IEnumerable<T> enumerable, Func<T,IComparable> compareBy)
      {
         IComparable max = null;
         T result = default(T);
         foreach (var o in enumerable)
         {
            var val = compareBy(o);
            if (max == null || max.CompareTo(val) < 0)
            {
               max = val;
               result = o;
            }
         }
         return result;
      }


      #endregion
   }
}