using System;
using System.Collections.Generic;
using KlopAi.algo;
using KlopIfaces;

namespace KlopAi
{
   internal class KlopPathFinder
   {
      #region Fields and Constants

      private readonly Node[,] field;
      private readonly IKlopModel klopModel;

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopPathFinder"/> class.
      /// </summary>
      /// <param name="model">The model.</param>
      public KlopPathFinder(IKlopModel model)
      {
         klopModel = model;
         field = new Node[model.FieldWidth,model.FieldHeight];
         foreach (IKlopCell cell in model.Cells)
         {
            //TODO: Compute cost
            field[cell.X, cell.Y] = new Node(cell.X, cell.Y) {Cost = 1};
         }
      }

      #endregion

      #region Public methods

      /// <summary>
      /// Finds the path betweed specified nodes.
      /// </summary>
      /// <returns></returns>
      public List<IKlopCell> FindPath(int startX, int startY, int finishX, int finishY)
      {
         var lastNode = (new AStar()).FindPath(GetNodeByCoordinates(startX, startY), GetNodeByCoordinates(finishX, finishY), GetDistance, GetNodeByCoordinates);
         var result = new List<IKlopCell>();
         while (lastNode != null)
         {
            var node = klopModel[lastNode.X, lastNode.Y];
            lastNode = lastNode.Parent;

            if (node.Owner == klopModel.CurrentPlayer) continue;
            result.Add(node);
         }
         return result;
      }

      #endregion

      #region Private and protected methods

      /// <summary>
      /// Gets the distance between two nodes.
      /// </summary>
      /// <param name="n1">The n1.</param>
      /// <param name="n2">The n2.</param>
      /// <returns></returns>
      private static double GetDistance(Node n1, Node n2)
      {
         var dx = n1.X - n2.X;
         var dy = n1.Y - n2.Y;
         return Math.Sqrt(dx*dx + dy*dy);
      }

      /// <summary>
      /// Gets the node by coordinates.
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      /// <returns></returns>
      private Node GetNodeByCoordinates(int x, int y)
      {
         if ((x >= 0) & (x < field.GetLength(0)) & (y >= 0) & (y < field.GetLength(1)))
            return field[x, y];
         return null;
      }

      #endregion
   }
}