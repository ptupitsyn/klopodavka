using System;
using Clops.Ai.Algo;
using KlopIfaces;

namespace KlopAi
{
    class ClopPathFinder
    {
        private readonly Node[,] Field;

        public ClopPathFinder(IKlopModel model)
        {
            Field = new Node[model.FieldWidth, model.FieldHeight];
           foreach (IKlopCell cell in model.Cells)
           {
              Field[cell.X, cell.Y] = new Node(cell.X, cell.Y);
           }
            //for (int i = 0; i <= model.FieldWidth; i++)
            //    for (int j = 0; j <= model.FieldHeight; j++)
        }

        private static double dist(Node n1, Node n2)
        {
           int dx = n1.px - n2.px;
           int dy = n1.py - n2.py;
           return Math.Sqrt(dx * dx + dy * dy);
        }

        private Node nodexy(int x, int y)
        {
            if ((x >= 0) & (x < Field.GetLength(0)) & (y >= 0) & (y < Field.GetLength(1)))
                return Field[x, y];
            else
                return null;
        }

        public Node FindPath(int sx, int sy, int fx, int fy)
        {
            return (new a_star()).FindPath(nodexy(sx, sy), nodexy(fx, fy), dist, nodexy);
        }

    }
}
