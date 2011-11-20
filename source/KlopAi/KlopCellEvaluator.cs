#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using KlopAi.Extentions;
using KlopIfaces;

#endregion

namespace KlopAi
{
   /// <summary>
   /// Cell cost provider.
   /// </summary>
   internal class KlopCellEvaluator
   {
      #region Fields and Constants

      public const double TurnBlockedCost = int.MaxValue; // Цена хода в запрещенную клетку (->inf)
      public const double TurnEatCost = 35; // Цена хода в занятую клетку
      public const double TurnEatEnemyBaseCost = 8; // Цена съедания клопа около вражеской базы
      public const double TurnEatOwnbaseCost = 5; // Цена съедания клопа около своей базы
      public const double TurnEmptyCost = 100; // Цена хода в пустую клетку
      public const double TurnNearEnemyBaseCost = 5000; // Цена хода около чужой базы
      public const double TurnNearEnemyEmptyCost = 130; // Цена хода в пустую клетку рядом с врагом
      public const double TurnNearOwnBaseCost = 2000; // Цена хода около своей базы

      private static readonly Dictionary<Tuple<IKlopPlayer, IKlopCell>, Tuple<double, ECellState>> CellValueCache =
         new Dictionary<Tuple<IKlopPlayer, IKlopCell>, Tuple<double, ECellState>>();

      private readonly IKlopModel _klopModel;

      #endregion

      #region Constructors

      /// <summary>
      /// Initializes a new instance of the <see cref="KlopCellEvaluator"/> class.
      /// </summary>
      /// <param name="model">The model.</param>
      public KlopCellEvaluator(IKlopModel model)
      {
         _klopModel = model;
      }

      #endregion

      #region Public methods

      /// <summary>
      /// Calculates cells Cost for specified player.
      /// </summary>
      /// <param name="klopPlayer">The klop player.</param>
      /// <param name="callback">Callback for evaluation result.</param>
      public void EvaluateCells(IKlopPlayer klopPlayer, Action<IKlopCell, double> callback)
      {
         var toRemove = new HashSet<Tuple<IKlopPlayer, IKlopCell>>();
         foreach (IKlopCell cell in _klopModel.Cells)
         {
            var cacheKey = new Tuple<IKlopPlayer, IKlopCell>(klopPlayer, cell);
            if (!CellValueCache.ContainsKey(cacheKey)) continue;

            var cachedCell = CellValueCache[cacheKey];
            if (cachedCell.Item2 == cell.State) continue;

            // Remove cell and adjanced cells from cache (do not remove immediately, needed for adjanced cells check)
            toRemove.Add(cacheKey);
            toRemove.AddRange(_klopModel.GetNeighborCells(cell).Select(c => new Tuple<IKlopPlayer, IKlopCell>(klopPlayer, c)));
         }
         CellValueCache.RemoveRange(toRemove);

         foreach (IKlopCell cell in _klopModel.Cells)
         {
            var cacheKey = new Tuple<IKlopPlayer, IKlopCell>(klopPlayer, cell);
            double cost;
            if (CellValueCache.ContainsKey(cacheKey))
            {
               cost = CellValueCache[cacheKey].Item1;
               //Debug.Assert(f.Cost == GetCellCost(cell, klopPlayer));  // Cache integrity test
            }
            else
            {
               cost = GetCellCost(cell, klopPlayer);
               CellValueCache[cacheKey] = new Tuple<double, ECellState>(cost, cell.State);
            }
            callback(cell, cost);
         }
      }

      #endregion

      #region Private and protected methods

      /// <summary>
      /// Gets the cell cost.
      /// </summary>
      /// <param name="cell">The cell.</param>
      /// <param name="klopPlayer">The klop player.</param>
      /// <returns></returns>
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
            return TurnNearEnemyEmptyCost*(1 + (double) enemyCount/2); // Turn near enemy klop costs a bit more.
         }

         var neighborCount = neighbors.Count(c => c.Owner != null);

         return TurnEmptyCost*(1 + (double) neighborCount/2); // Default - turn into empty cell.
      }

      private static bool IsCellNearBase(IKlopCell cell, IKlopPlayer baseOwner)
      {
         return Math.Max(Math.Abs(cell.X - baseOwner.BasePosX), Math.Abs(cell.Y - baseOwner.BasePosY)) == 1;
      }

      #endregion
   }
}