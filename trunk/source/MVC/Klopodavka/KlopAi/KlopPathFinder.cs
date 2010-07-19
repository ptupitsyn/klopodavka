using System;
using System.Collections.Generic;
using System.Linq;
using KlopAi.algo;
using KlopIfaces;

namespace KlopAi
{
   public class KlopPathFinder
   {
      #region Fields and Constants

      public const int TurnBlockedCost = int.MaxValue; // Цена хода в запрещенную клетку (->inf)
      public const int TurnEatCost = 35; // Цена хода в занятую клетку
      public const int TurnEatEnemyBaseCost = 8; // Цена съедания клопа около вражеской базы
      public const int TurnEatOwnbaseCost = 5; // Цена съедания клопа около своей базы
      public const int TurnEmptyCost = 100; // Цена хода в пустую клетку
      public const int TurnNearBaseCost = 11000; // Цена хода около своей базы
      private readonly Node[,] field;
      private readonly IKlopModel klopModel;
      private readonly IKlopPlayer klopPlayer;

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopPathFinder"/> class.
      /// </summary>
      /// <param name="model">The model.</param>
      /// <param name="player">The player to find path for.</param>
      public KlopPathFinder(IKlopModel model, IKlopPlayer player)
      {
         klopModel = model;
         klopPlayer = player;
         field = new Node[model.FieldWidth,model.FieldHeight];
         foreach (IKlopCell cell in model.Cells)
         {
            var cost = GetCellCost(cell);
            //((KlopCell) cell).Tag = cost;
            field[cell.X, cell.Y] = new Node(cell.X, cell.Y) {Cost = cost};
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

            if (node.Owner == klopPlayer) continue;
            result.Add(node);
         }
         return result;
      }

      #endregion

      #region Private and protected methods

      private double GetCellCost(IKlopCell cell)
      {
         if (cell.Owner == klopPlayer)
         {
            return 0; // Zero cost for owned cell
         }
         
         if (cell.State == ECellState.Dead)
         {
            return TurnBlockedCost; // Can't move into own dead cell or base cell
         }
         
         if (cell.Owner != null && cell.State == ECellState.Alive)
         {
            if (IsCellNearBase(cell, klopPlayer))
            {
               return TurnEatOwnbaseCost;
            }

            if (klopModel.Players.Where(p => p != klopPlayer).Any(enemy => IsCellNearBase(cell, enemy)))
            {
               return TurnEatEnemyBaseCost;
            }

            return TurnEatCost;
         }

         if (IsCellNearBase(cell, klopPlayer))
         {
            return TurnNearBaseCost;
         }

         return TurnEmptyCost; // Default - turn into empty cell.
      }

      private bool IsCellNearBase(IKlopCell cell, IKlopPlayer baseOwner)
      {
         return Math.Max(Math.Abs(cell.X - baseOwner.BasePosX), Math.Abs(cell.Y - baseOwner.BasePosY)) == 1;
      }

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

         // Diagonal turn should cost 1
         //return Math.Max(Math.Abs(dx), Math.Abs(dy)); 

         // Use Sqrt for more natural-looking paths
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
         if ((x >= 0) && (x < field.GetLength(0)) && (y >= 0) && (y < field.GetLength(1)))
            return field[x, y];
         return null;
      }

      #endregion
   }
}