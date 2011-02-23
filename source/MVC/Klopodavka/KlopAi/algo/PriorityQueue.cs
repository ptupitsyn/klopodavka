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
         // This (or PopHighest) take 80% of PathFinder performance. 
         // I've spent several hours on trying to improve the performance of the following three lines to ABSOLUTELY NO SUCCESS:
         // Sorted* classes do not help, manual enumeration does not help
         // NOTE: node value can change after enqueuing!
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