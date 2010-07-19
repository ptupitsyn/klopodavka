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
      #region Nested type: NodeComparer

      private class NodeComparer : IComparer<Node>
      {
         #region Fields and Constants

         private static NodeComparer _instance;

         #endregion

         #region Constructors

         private NodeComparer()
         {
         }

         #endregion

         #region IComparer<Node> implementation

         public int Compare(Node x, Node y)
         {
            return Math.Sign(y.Fval - x.Fval);
         }

         #endregion

         #region Public properties and indexers

         public static NodeComparer Instance
         {
            get { return _instance ?? (_instance = new NodeComparer()); }
         }

         #endregion
      }

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="PriorityQueue"/> class.
      /// </summary>
      public PriorityQueue() // : base(NodeComparer.Instance)
      {
      }

      #endregion

      #region Public methods

      /// <summary>
      /// Get the lowest element in queue.
      /// OLD VERSION.
      /// </summary>
      public Node Pop()
      {
         Node o = null; // lowest object in queue
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
      /// Get the lowest element in queue.
      /// </summary>
      /// <returns></returns>
      public Node Pop1()
      {
         // Works when derived from SortedSet. However, seems to work incorrect.
         var node = this.Last();
         Remove(node);
         return node;
      }

      #endregion
   }
}