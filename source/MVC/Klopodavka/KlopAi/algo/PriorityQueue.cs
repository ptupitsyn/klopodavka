using System;
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
         Node o = null; 
         var d = double.MaxValue;

         foreach (Node n in this)
         {
            if (n.Fval < d)
            {
               o = n;
               d = o.Fval;
            }
         }

         Remove(o);
         return o;
      }

      /// <summary>
      /// Get the highest element in queue.
      /// </summary>
      /// <returns></returns>
      public Node PopHighest()
      {
         Node o = null;
         var d = double.MinValue;

         foreach (Node n in this)
         {
            if (n.Fval > d)
            {
               o = n;
               d = o.Fval;
            }
         }

         Remove(o);
         return o;
      }

      #endregion
   }
}