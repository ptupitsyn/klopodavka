using System;
using Clops.Ifaces;

namespace Clops.Ai.Algo
{
    /// <summary>
    /// Summary description for node.
    /// </summary>
    public class Node : IClopNode
    {
        //
        public IClopNode Parent { get; set; }
        //
        public double gdist { get; set; }
        public double hdist { get; set; }
        public double cost { get; set; }
        public bool inpath { get; set; }
        public bool visited { get; set; }
        public int px { get; set; }
        public int py { get; set; }
        public double fval
        {
            get
            {
                return gdist+hdist;
            }
        }
        //
        public Node(int x, int y)
        {
            Parent=null;
            hdist=0;
            gdist=0;
            cost=0;
            inpath=false;
            visited=false;
            px=x; py=y;
        }
        public Node() : this(0, 0)
        {
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            // TODO:  Add node.CompareTo implementation
            //return fval.CompareTo();
            Node n = obj as Node;
            if (n != null)
                return fval.CompareTo(n.fval);
            throw new ArgumentException("object is not a NODE");
        }

        #endregion

        public IClopNode Clone()
        {
            var newNode = new Node(px, py)
                              {
                                  Parent = Parent,
                                  hdist = hdist,
                                  gdist = gdist,
                                  cost = cost,
                                  inpath = inpath,
                                  visited = visited,
                                  px = px,
                                  py = py
                              };
            return newNode;
        }
    }
}