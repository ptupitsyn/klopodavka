using System;
using System.Collections;

namespace Clops_
{
	/// <summary>
	/// Summary description for a_star.
	/// </summary>
	public class a_star
	{
		public a_star()
		{
			Dist=null;
			NodeXY=null;
		}

		private Priority_queue Open = new Priority_queue();
		private ArrayList Closed = new ArrayList();

		public delegate double GetDist(node n1, node n2); 
		public GetDist Dist;
		
		public delegate node GetNodeByXY(int x, int y); 
		public GetNodeByXY NodeXY;

        public node FindPath(node s,node f)
		{
			//Some checks
			if ((Dist==null)|(NodeXY==null))
			{
				throw new ArgumentException("Delegate Function(s) not set! (Functions Dist and NodeXY)");
			}

			//Reset
			Open.Clear();
			Closed.Clear();

			//GO!
//			s.g = 0  // s is the start node
			s.gdist=0;
//			s.h = GoalDistEstimate( s )
			s.hdist=Dist(s,f);
//			s.f = s.g + s.h
			//OK
//			s.parent = null
			s.parent=null;
//			push s on Open
			Open.Add(s);
//			while Open is not empty
			while (Open.Count>0)
			{
//				pop node n from Open  // n has the lowest f
				//System.Windows.Forms.MessageBox.Show(Open.Count.ToString());
				node n=(node)Open.Pop();
//				if n is a goal node
				if (n.Equals(f))
				{
					//					construct path
					//					return success
					return n;
				}
//				for each successor n' of n
				int[] dx=new int[8] { -1, -1, -1, 1 , 1 , 1 , 0 , 0 };
				int[] dy=new int[8] { -1, 0 , 1 , -1, 0 , 1 , -1, 1 };
				for (int i=0;i<=7;i++)
				{
					node nn=NodeXY(n.px+dx[i],n.py+dy[i]);
					if (nn!=null)
					{
						//					newg = n.g + cost(n,n')
						double newg = n.gdist+nn.cost;
						//					if n' is in Open or Closed,and n'.g <= newg {	skip }
						if (!((Open.Contains(nn)|Closed.Contains(nn)) & (nn.gdist<=newg)))
						{
							//n'.parent = n
							nn.parent=n;
							//n'.g = newg
							nn.gdist=newg;
							//n'.h = GoalDistEstimate( n' )
							nn.hdist=Dist(nn,f);
							//n'.f = n'.g + n'.h
							//OK
							//if n' is in Closed	remove it from Closed
							//nn.visited=false;
							if (Closed.Contains(nn)) Closed.Remove(nn);
							//if n' is not yet in Open  push n' on Open
							if (!(Open.Contains(nn))) Open.Add(nn);
						}
					}
				}
//				push n onto Closed
				//n.visited=true;
				Closed.Add(n);
			}
//			return failure // if no path found			
			return null;
		}
	}
}
