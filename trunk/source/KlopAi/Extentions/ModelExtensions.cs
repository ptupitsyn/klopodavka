using System;
using System.Linq;
using KlopIfaces;

namespace KlopAi.Extentions
{
    internal static class ModelExtensions
    {
        /// <summary>
        /// Generates the starting pattern: returns next position for the initial standoff.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="player">The player.</param>
        /// <param name="enemyDistanceFunc">The enemy distance function.</param>
        public static IKlopCell GenerateStartingPattern(this IKlopModel model, IKlopPlayer player, Func<IKlopCell, double> enemyDistanceFunc)
        {
            if (model == null || player == null || enemyDistanceFunc == null)
                throw new ArgumentNullException();

            // TODO: Target sometimes falls behing enemy cells, and, however, target cell is not close to enemy, the path is.
            // TODO: "Safe path"?? "Safe evaluator".. or SafePathFinder. How to build safe cells map fast?
            return model.Cells
                .Where(c =>
                {
                    if (c.X < 1 || c.Y < 1 || c.X >= model.FieldWidth - 2 || c.Y >= model.FieldHeight - 2) return false;
                    if (model.GetNeighborCells(c).Any(cc => cc.Owner != null)) return false;
                    var dx1 = Math.Abs(c.X - player.BasePosX);
                    var dy1 = Math.Abs(c.Y - player.BasePosY);
                    return dx1 > 1 && dy1 > 1
                           && ((dx1 * dx1 + dy1 * dy1) < (Math.Pow(model.FieldHeight, 2) + Math.Pow(model.FieldWidth, 2)) / 3)
                           && (enemyDistanceFunc(c) > model.TurnLength / 1.7);
                }).Random() ?? model.Cells.Where(c => c.Owner == null).Random();
        }

        /// <summary>
        /// Determines whether fight is started - there are dead clops on the field.
        /// </summary>
        public static bool IsFightStarted(this IKlopModel model)
        {
            return model.Cells.Any(c => c.State == ECellState.Dead /*|| _model.Cells.Count(c => c.Owner != null) > _model.FieldHeight*_model.FieldWidth/8*/);
        }
    }
}
