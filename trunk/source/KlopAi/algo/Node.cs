#region Usings

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace KlopAi.algo
{
   /// <summary>
   /// Represents path node.
   /// </summary>
   public class Node : IComparable<Node>
   {
      #region Fields and Constants

      private static readonly int[] Dx = {-1, -1, -1, 1, 1, 1, 0, 0};
      private static readonly int[] Dy = {-1, 0, 1, -1, 0, 1, -1, 1};
      private double _gdist;
      private double _hdist;

      #endregion

      #region Constructors

      public Node(int x, int y)
      {
         Parent = null;
         Hdist = 0;
         Gdist = 0;
         Cost = 0;
         X = x;
         Y = y;
      }

      #endregion

      #region IComparable<Node> implementation

      public int CompareTo(Node other)
      {
         return _fval.CompareTo(other._fval);
      }

      #endregion

      #region Public properties and indexers

      /// <summary>
      /// Parent node; used to construct path.
      /// </summary>
      /// <value>The parent.</value>
      public Node Parent { get; set; }

      /// <summary>
      /// Cost of move from start Node to this Node.
      /// </summary>
      public double Gdist
      {
         get { return _gdist; }
         set
         {
            _gdist = value;
            _fval = _gdist + _hdist;
         }
      }

      /// <summary>
      /// Heuristic cost of move from this Node to finish Node.
      /// </summary>
      public double Hdist
      {
         get { return _hdist; }
         set
         {
            _hdist = value;
            _fval = _gdist + _hdist;
         }
      }

      /// <summary>
      /// Cost of move to this Node.
      /// </summary>
      public double Cost { get; set; }

      /// <summary>
      /// Gets or sets the X position of Node.
      /// </summary>
      public int X { get; private set; }

      /// <summary>
      /// Gets or sets the Y position of Node.
      /// </summary>
      public int Y { get; private set; }

      /// <summary>
      /// Resulting cost of moving to this Node. (Equals Gdist + Hdist)
      /// </summary>
      private double _fval;  // Field is faster than autoproperty (at least in debug mode)

      #endregion

      #region Public methods

      public IEnumerable<Node> GetNeighborNodes(Func<int, int, Node> getNodeByXy)
      {
         return Dx.Select((t, i) => getNodeByXy(X + Dx[i], Y + Dy[i]));
      }

      public void Reset()
      {
         Parent = null;
         _gdist = 0;
         _hdist = 0;
         Cost = 0;
      }

      /// <summary>
      /// Returns a <see cref="System.String"/> that represents this instance.
      /// </summary>
      /// <returns>
      /// A <see cref="System.String"/> that represents this instance.
      /// </returns>
      public override string ToString()
      {
         return string.Format("[{0}, {1}]", X, Y);
      }

      #endregion
   }
}