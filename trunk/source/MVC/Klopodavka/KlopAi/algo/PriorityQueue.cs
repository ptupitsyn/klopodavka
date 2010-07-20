using System.Linq;
using System.Collections.Generic;

namespace KlopAi.algo
{
   /// <summary>
   /// Summary description for priority_queue.
   /// </summary>
   public class PriorityQueue : HashSet<Node>
   {
      #region Public methods

      /// <summary>
      /// Get the lowest element in queue.
      /// </summary>
      public Node Pop()
      {
         var min = this.Min(); 
         Remove(min);
         return min;
      }

      /// <summary>
      /// Get the highest element in queue.
      /// </summary>
      /// <returns></returns>
      public Node PopHighest()
      {
         var max = this.Max();
         Remove(max);
         return max;
      }

      #endregion
   }
}