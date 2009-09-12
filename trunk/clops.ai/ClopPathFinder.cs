using Clops.Ai.Algo;
using Clops.Ifaces;

namespace Clops.Ai
{
    class ClopPathFinder
    {
        private readonly IClopNode[,] Field;

        public ClopPathFinder(IClopCell[,] field)
        {
            Field = new Node[field.GetUpperBound(0) + 1, field.GetUpperBound(1) + 1];
            for (int i = 0; i <= field.GetUpperBound(0); i++)
                for (int j = 0; j <= field.GetUpperBound(1); j++)
                    Field[i, j] = field[i, j].Clone();
        }

        private static double dist(IClopNode n1, IClopNode n2)
        {
            return ClopCPU.Distance(n1.px, n1.py, n2.px, n2.py);
        }

        private IClopNode nodexy(int x, int y)
        {
            if ((x >= 0) & (x < ClopWar.FieldW) & (y >= 0) & (y < ClopWar.FieldH))
                return Field[x, y];
            else
                return null;
        }

        public IClopNode FindPath(int sx, int sy, int fx, int fy)
        {
            return (new a_star()).FindPath(nodexy(sx, sy), nodexy(fx, fy), dist, nodexy);
        }

    }
}
