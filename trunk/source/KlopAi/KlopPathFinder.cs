#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using KlopAi.algo;
using KlopIfaces;

#endregion

namespace KlopAi
{
   public class KlopPathFinder
   {
      #region Fields and Constants

      private readonly AStar _aStar;
      private readonly KlopCellEvaluator _cellEvaluator;
      private readonly Node[,] _field;
      private readonly IKlopModel _klopModel;

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopPathFinder"/> class.
      /// </summary>
      /// <param name="model">The model.</param>
      public KlopPathFinder(IKlopModel model)
      {
         _klopModel = model;
         _cellEvaluator = new KlopCellEvaluator(model);
         _aStar = new AStar();
         _field = new Node[model.FieldWidth,model.FieldHeight];
         foreach (IKlopCell cell in _klopModel.Cells)
         {
            _field[cell.X, cell.Y] = new Node(cell.X, cell.Y);
         }
      }

      #endregion

      #region Public methods

       /// <summary>
      /// Finds the path betweed specified nodes for specified player.
      /// </summary>
      public List<IKlopCell> FindPath(int startX, int startY, int finishX, int finishY, IKlopPlayer klopPlayer, bool inverted = false)
      {
         return FindPath(GetNodeByCoordinates(startX, startY), GetNodeByCoordinates(finishX, finishY), klopPlayer, inverted)
            .Select(n => _klopModel[n.X, n.Y]).Where(c => c.Owner != klopPlayer).ToList();
      }


       /// <summary>
       /// Finds the path between two nodes.
       /// </summary>
       public IEnumerable<Node> FindPath(Node startNode, Node finishNode, IKlopPlayer klopPlayer, bool inverted, bool skipEvaluate = false)
       {
           // Init field
           if (!skipEvaluate) EvaluateCells(klopPlayer);

           // Get result
           var lastNode = _aStar.FindPath(startNode, finishNode, GetDistance, GetNodeByCoordinates, inverted);

           while (lastNode != null)
           {
               yield return lastNode;
               lastNode = lastNode.Parent;
           }
       }


       /// <summary>
      /// Gets the distance between two nodes.
      /// </summary>
      private static double GetDistance(Node n1, Node n2)
      {
         return GetDistance(n1.X, n1.Y, n2.X, n2.Y);
      }

      /// <summary>
      /// Gets the distance between two nodes.
      /// </summary>
      private static double GetDistance(int x1, int y1, int x2, int y2)
      {
         var dx = x1 - x2;
         var dy = y1 - y2;

         // Diagonal turn should cost 1
         return Math.Max(Math.Abs(dx), Math.Abs(dy));

         // Use Sqrt for more natural-looking paths
         //return Math.Sqrt(dx*dx + dy*dy);

         //return (Math.Max(Math.Abs(dx), Math.Abs(dy)) + Math.Sqrt(dx*dx + dy*dy))/2;
      }

      /// <summary>
      /// Gets the node by coordinates.
      /// </summary>
      /// <param name="x">The x.</param>
      /// <param name="y">The y.</param>
      /// <returns></returns>
      public Node GetNodeByCoordinates(int x, int y)
      {
         if ((x >= 0) && (x < _field.GetLength(0)) && (y >= 0) && (y < _field.GetLength(1)))
            return _field[x, y];
         return null;
      }

      #endregion

      /// <summary>
      /// Evaluates the cells for specified player.
      /// </summary>
      /// <param name="klopPlayer">The klop player.</param>
      private void EvaluateCells(IKlopPlayer klopPlayer)
      {
         _cellEvaluator.EvaluateCells(klopPlayer, (cell, cost) =>
                                                     {
                                                        var node = _field[cell.X, cell.Y];
                                                        node.Reset();
                                                        node.Cost = cost;
                                                     });
      }
   }
}