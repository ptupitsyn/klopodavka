using System.Collections.Generic;
using System.Linq;
using KlopIfaces;

namespace KlopModel
{
    /// <summary>
    /// Implements game rules where turn can be made from any alive cell.
    /// </summary>
    public class KlopModelAllowDisconnected : KlopModelBase
    {
        public KlopModelAllowDisconnected(int width, int height, IEnumerable<IKlopPlayer> players, int turnLenght) : base(width, height, players, turnLenght)
        {
        }

        protected override IEnumerable<IKlopCell> FindAvailableCells(KlopCell baseCell)
        {
            return Cells.Where(
                cell =>
                    (cell.State == ECellState.Free || (cell.State == ECellState.Alive && cell.Owner != CurrentPlayer))
                    && GetNeighborCells(cell).Any(x => x.Owner == CurrentPlayer));
        }
    }
}