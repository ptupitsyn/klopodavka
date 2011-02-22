using System;
using System.Collections.Generic;
using System.Linq;
using KlopAi.algo;
using KlopIfaces;
using KlopModel;

namespace KlopAi
{
   public class KlopPathFinder
   {
      #region Fields and Constants

      public const double TurnBlockedCost = int.MaxValue; // Цена хода в запрещенную клетку (->inf)
      public const double TurnEatCost = 35; // Цена хода в занятую клетку
      public const double TurnEatEnemyBaseCost = 8; // Цена съедания клопа около вражеской базы
      public const double TurnEatOwnbaseCost = 5; // Цена съедания клопа около своей базы
      public const double TurnEmptyCost = 100; // Цена хода в пустую клетку
      public const double TurnNearOwnBaseCost = 2000; // Цена хода около своей базы
      public const double TurnNearEnemyBaseCost = 5000; // Цена хода около чужой базы
      public const double TurnNearEnemyEmptyCost = 140; // Цена хода в пустую клетку рядом с врагом
      private readonly Node[,] _field;
      private readonly IKlopModel _klopModel;
      private AStar _aStar;

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopPathFinder"/> class.
      /// </summary>
      /// <param name="model">The model.</param>
      public KlopPathFinder(IKlopModel model)
      {
         _klopModel = model;
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
      public List<IKlopCell> FindPath(int startX, int startY, int finishX, int finishY, IKlopPlayer klopPlayer)
      {
         return FindPath(startX, startY, finishX, finishY, klopPlayer, false);
      }

      /// <summary>
      /// Finds the path betweed specified nodes for specified player.
      /// </summary>
      public List<IKlopCell> FindPath(int startX, int startY, int finishX, int finishY, IKlopPlayer klopPlayer, bool inverted)
      {
         return FindPath(GetNodeByCoordinates(startX, startY), GetNodeByCoordinates(finishX, finishY), klopPlayer, inverted)
            .Select(n => _klopModel[n.X, n.Y]).Where(c => c.Owner != klopPlayer).ToList();
      }


      /// <summary>
      /// Finds the path between two nodes.
      /// </summary>
      public List<Node> FindPath(Node startNode, Node finishNode, IKlopPlayer klopPlayer, bool inverted)
      {
         return FindPath(startNode, finishNode, klopPlayer, inverted, false);
      }

      /// <summary>
      /// Finds the path between two nodes.
      /// </summary>
      public List<Node> FindPath(Node startNode, Node finishNode, IKlopPlayer klopPlayer, bool inverted, bool skipEvaluate)
      {
         // Init field
         if (!skipEvaluate)
            EvaluateCells(klopPlayer);

         // Get result
         var lastNode = PathFinder.FindPath(startNode, finishNode, GetDistance, GetNodeByCoordinates, inverted);

         var result = new List<Node>();
         while (lastNode != null)
         {
            result.Add(lastNode);
            lastNode = lastNode.Parent;
         }
         return result;
      }

      private static readonly Dictionary<Tuple<IKlopPlayer, IKlopCell>, Tuple<double, ECellState>> CellValueCache =
         new Dictionary<Tuple<IKlopPlayer, IKlopCell>, Tuple<double, ECellState>>();

      /// <summary>
      /// Calculates cells Cost.
      /// </summary>
      /// <param name="klopPlayer">The klop player.</param>
      public void EvaluateCells(IKlopPlayer klopPlayer)
      {
         //TODO: Refactor, Extract CellEvaluator class

         foreach (IKlopCell cell in _klopModel.Cells)
         {
            var cacheKey = new Tuple<IKlopPlayer, IKlopCell>(klopPlayer, cell);
            if (!CellValueCache.ContainsKey(cacheKey)) continue;
            
            var cachedCell = CellValueCache[cacheKey];
            if (cachedCell.Item2 == cell.State) continue;
            
            // Remove cell and adjanced cells from cache
            CellValueCache.Remove(cacheKey);
            foreach (IKlopCell neighborCell in _klopModel.GetNeighborCells(cell))
            {
               CellValueCache.Remove(new Tuple<IKlopPlayer, IKlopCell>(klopPlayer, neighborCell));
            }
         }

         foreach (IKlopCell cell in _klopModel.Cells)
         {
            var f = _field[cell.X, cell.Y];
            f.Reset();

            var cacheKey = new Tuple<IKlopPlayer, IKlopCell>(klopPlayer, cell);
            if (CellValueCache.ContainsKey(cacheKey))
            {
               f.Cost = CellValueCache[cacheKey].Item1;
            }
            else
            {
               f.Cost = GetCellCost(cell, klopPlayer);
               CellValueCache[cacheKey] = new Tuple<double, ECellState>(f.Cost, cell.State);
            }

            //if (f.Cost != TurnEmptyCost)
            //{
            //   ((KlopCell)cell).Tag = f.Cost;
            //}
         }
      }

      /// <summary>
      /// Gets the distance between two nodes.
      /// </summary>
      public static double GetDistance(Node n1, Node n2)
      {
         return GetDistance(n1.X, n1.Y, n2.X, n2.Y);
      }

      /// <summary>
      /// Gets the distance between two nodes.
      /// </summary>
      public static double GetDistance(IKlopCell cell1, IKlopCell cell2)
      {
         return GetDistance(cell1.X, cell1.Y, cell2.X, cell2.Y);
      }

      /// <summary>
      /// Gets the distance between two nodes.
      /// </summary>
      public static double GetDistance(int x1, int y1, int x2, int y2)
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

      #region Private and protected properties and indexers

      private AStar PathFinder
      {
         get { return _aStar ?? (_aStar = new AStar()); }
      }

      #endregion

      #region Private and protected methods

      private double GetCellCost(IKlopCell cell, IKlopPlayer klopPlayer)
      {
         if (cell.Owner == klopPlayer)
         {
            return 0; // Zero cost for owned cell
         }

         if (cell.State == ECellState.Dead)
         {
            return TurnBlockedCost; // Can't move into own dead cell or base cell
         }

         //TODO: Additive cost! E.g. near own clop + near enemy clop!!
         if (cell.Owner != null && cell.State == ECellState.Alive)
         {
            if (IsCellNearBase(cell, klopPlayer))
            {
               return TurnEatOwnbaseCost;
            }

            if (_klopModel.Players.Where(p => p != klopPlayer).Any(enemy => IsCellNearBase(cell, enemy)))
            {
               return TurnEatEnemyBaseCost;
            }

            return TurnEatCost;
         }

         if (IsCellNearBase(cell, klopPlayer))
         {
            return TurnNearOwnBaseCost;
         }

         var neighbors = _klopModel.GetNeighborCells(cell);
         if (neighbors.Any(c => c.State == ECellState.Base))
         {
            return TurnNearEnemyBaseCost;
         }

         var enemyCount = neighbors.Count(c => c.Owner != null && c.Owner != klopPlayer);
         if (enemyCount > 0)
         {
            return TurnNearEnemyEmptyCost * (1 + (double)enemyCount / 2); // Turn near enemy klop costs a bit more.
         }

         var neighborCount = neighbors.Count(c => c.Owner != null);

         return TurnEmptyCost * (1 + (double)neighborCount / 2); // Default - turn into empty cell.
      }

      private static bool IsCellNearBase(IKlopCell cell, IKlopPlayer baseOwner)
      {
         return Math.Max(Math.Abs(cell.X - baseOwner.BasePosX), Math.Abs(cell.Y - baseOwner.BasePosY)) == 1;
      }

      #endregion
   }
}