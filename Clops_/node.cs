using System;

namespace Clops_
{
	/// <summary>
	/// Summary description for node.
	/// </summary>
	public class node :IComparable
	{
		//
		public node parent;
		//
		public double gdist;
		public double hdist;
		public double cost;
		public bool inpath;
		public bool visited;
		public int px;
		public int py;
		public double fval
		{
			get
			{
				return gdist+hdist;
			}
		}
		//
		public node(int x, int y)
		{
			parent=null;
			hdist=0;
			gdist=0;
			cost=0;
			inpath=false;
			visited=false;
			px=x; py=y;
		}
		public node()
		{
			parent=null;
			hdist=0;
			gdist=0;
			cost=0;
			inpath=false;
			visited=false;
			px=0; py=0;
		}
		#region IComparable Members

		public int CompareTo(object obj)
		{
			// TODO:  Add node.CompareTo implementation
			//return fval.CompareTo();
			if(obj is node) 
			{
				node n = (node) obj;

				return fval.CompareTo(n.fval);
			}
        
			throw new ArgumentException("object is not a NODE");
		}

		#endregion
	}
}
