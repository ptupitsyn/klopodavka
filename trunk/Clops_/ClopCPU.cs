/////////////////////////////
/// code by TPS
/// tps0@hotmail.com
/////////////////////////////

using System;
using System.Collections;


namespace Clops_
{
	/// <summary>
	/// Acts as CPU player
	/// Artificial intelligence
	/// Interacts with ClopWar class
	/// </summary>
	public class ClopCPU
	{

		#region Class header

		//Strategy file
		public string StrategyFile = null;
		//Options
		public bool AdvancedDefence = false;
		public bool AdvancedPath = true;
		//Vars
		private ClopWar ClpWar;
		private int side;				//Side
		private int xside;				//Opposite side
		private int bx;					//Base coords
		private int by;
		private int ebx;				//Enemy base
		private int eby;
		private ArrayList bestpath;		//Store best path sequence
		private bool beginning;			//Indicates state of the game

		//PathFinder
		a_star PathFinder = new a_star();

		//Const
		private const int TURN_EMPTY = 100;				//Цена хода в пустую клетку
		private const int TURN_EAT = 35;				//Цена хода в занятую клетку
		private const int TURN_EATENEMYBASE = 8;		//Цена съедания клопа около вражеской базы
		private const int TURN_EATOWNBASE = 5;			//Цена съедания клопа около своей базы
		private const int TURN_NEARBASE = 11000;		//Цена хода около своей базы
		private const int TURN_BLOCKED = 1000000;		//Цена хода в запрещенную клетку (->inf)
		//Same for values (not costs)
		private const int TURN_EAT2 = -100;				//Цена хода в занятую клетку
		private const int TURN_EATENEMYBASE2 = -1000;	//Цена съедания клопа около вражеской базы
		private const int TURN_EATOWNBASE2 = -1100;		//Цена съедания клопа около своей базы
		private const int TURN_NEARBASE2 = 1000;		//Цена хода около своей базы
		
		//Constructor
		public ClopCPU(ClopWar cw)
		{
			ClpWar=cw;

			//Prepare pathfinder
			PathFinder.Dist=new Clops_.a_star.GetDist(dist);
			PathFinder.NodeXY=new Clops_.a_star.GetNodeByXY(nodexy);

		}

		#endregion

		private void EvalCells()
		{
			int i;
			int j;
			double v;
			cell c;

			//Find price for every cell
			for (i=0;i<ClopWar.FieldW;i++)
				for (j=0;j<ClopWar.FieldH;j++)
				{
					//A*
					ClpWar.field[i,j].cost=TURN_EMPTY;
					ClpWar.field[i,j].parent=null;
					ClpWar.field[i,j].inpath=false;
					ClpWar.field[i,j].visited=false;

					if (ClpWar.field[i,j].owner==side)
					{
						v=Double.MaxValue;
						//A*
						ClpWar.field[i,j].cost=0;	//Ход в свою клетку бесплатный
					}
					else
					{
						c=ClpWar.field[i,j];					
						v=Distance(ebx,eby,i,j);
						if (c.owner==xside) {v+=TURN_EAT2;};
						//A*
						if (c.owner==xside)
							ClpWar.field[i,j].cost=TURN_EAT;
						if ((c.owner==xside)&((c.state==cell.DEAD)|(c.state==cell.BASE)))
							ClpWar.field[i,j].cost=TURN_BLOCKED;

					}

					ClpWar.field[i,j].val=v;
				}

			//Check cells near enemy base - eat everything near it.
			int[,] carr = new int[8,2] {{ebx-1,eby-1},{ebx-1,eby},{ebx-1,eby+1},{ebx,eby-1},{ebx,eby+1},{ebx+1,eby-1},{ebx+1,eby+1},{ebx+1,eby}};
			for (int k=0;k<8;k++)
			{
				i=carr[k,0];
				j=carr[k,1];
				if (ClpWar.field[i,j].owner==xside) 
					ClpWar.field[i,j].val=TURN_EATENEMYBASE2; //Best turn except of eating near own base
				else
					ClpWar.field[i,j].val=TURN_NEARBASE2; //NEVER move here - it only helps your opponent

				//A*
				if ((ClpWar.field[i,j].owner==xside) & (ClpWar.field[i,j].state!=cell.DEAD)) 
					ClpWar.field[i,j].cost=TURN_EATENEMYBASE; //Best turn except of eating near own base
				else
				{
					if (ClpWar.field[i,j].owner==cell.EMPTY)				
						ClpWar.field[i,j].cost=TURN_NEARBASE; //NEVER move here - it only helps your opponent
				}
			}
			//----

			//Try NOT to move to any cell near own base
			int[,] carr2 = new int[8,2] {{bx-1,by-1},{bx-1,by},{bx-1,by+1},{bx,by-1},{bx,by+1},{bx+1,by-1},{bx+1,by+1},{bx+1,by}};
			for (int k=0;k<8;k++)
			{
				i=carr2[k,0];
				j=carr2[k,1];
				if (ClpWar.field[i,j].owner==xside) 
					ClpWar.field[i,j].val=TURN_EATOWNBASE2; //Really the Best turn
				else
					ClpWar.field[i,j].val=TURN_NEARBASE2; //NEVER move here except of when there is nowhere to move

				//A*
				if ((ClpWar.field[i,j].owner==xside) & (ClpWar.field[i,j].state!=cell.DEAD))
					ClpWar.field[i,j].cost=TURN_EATOWNBASE; //Really the Best turn
				else
				{
					if (ClpWar.field[i,j].owner==cell.EMPTY)				
						ClpWar.field[i,j].cost=TURN_NEARBASE; //NEVER move here except of when there is nowhere to move
				}
			}
			//----
		}

		
		#region AuxFnc
		//Functions required for A_STAR
		private double dist(node n1, node n2)
		{
			return Distance(n1.px,n1.py,n2.px,n2.py);
		}

		private node nodexy(int x, int y)
		{
			if ((x>=0)&(x<ClopWar.FieldW)&(y>=0)&(y<ClopWar.FieldH))
				return ClpWar.field[x,y];
			else
				return null;
		}

		private bool CheckNeighbours(int x, int y, int depth)
		{
			for (int i=x-depth; i<=x+depth; i++)
				for (int j=y-depth; j<=y+depth; j++)
					if (ClpWar.CheckBound(i,j))
						if (ClpWar.field[i,j].owner==xside)
							return true;
			return false;
		}

		public double Distance(int x1, int y1, int x2, int y2)
		{
			int dx=x1-x2;
			int dy=y1-y2;
			return Math.Sqrt(dx*dx+dy*dy);
		}
		#endregion

		#region PathFinder
		//Find path using A-Star Pathfinder
		private void FindPath()
		{
			//GO!
			node bestn=PathFinder.FindPath(nodexy(ebx,eby),nodexy(bx,by));
			
			//Write found path into bestpath
			bestpath=new ArrayList();
			node n=bestn;
			while (n!=null)
			{
				n.inpath=true;
				bestpath.Add(n);
				n=n.parent;
			}
		}

		private double FindPathLength()
		{
			//GO!
			node bestn=PathFinder.FindPath(nodexy(ebx,eby),nodexy(bx,by));
			
			//Count path length
			//int len = 0;
			node n=bestn;
			/*while (n!=null)
			{
				n=n.parent;
				len++;
			}*/

			return n.gdist;
		}
		#endregion

		public void Stage1_Array()
		{
			#region Pre-battle array forming
			beginning = true;
			while ((ClpWar.turn==side) & beginning)
			{
				//Analyse field: what's happening (beginning of the game or battle)			
				beginning=true;
				for (int i=0; (i<ClopWar.FieldW); i++)
					for (int j=0; (j<ClopWar.FieldH); j++)
					{
						//if (((ClpWar.field[i,j].state==cell.DEAD) & (ClpWar.field[i,j].owner!=cell.EMPTY)) | ((ClpWar.field[i,j].avail)&(ClpWar.field[i,j].owner==xside)))
						if ((ClpWar.field[i,j].owner==side) & CheckNeighbours(i,j,(int)(ClpWar.clopNum/3)))
						{
							beginning=false;
						}
					}

				//Prepare pre-battle array
				if (beginning)
				{
					if (StrategyFile!=null)
					{
						if (ClpWar.PlayRecord(StrategyFile,1)==0) break;
					}
					else break;
				}

				/**/
			}
			#endregion
		}

		public bool Stage2_Defence()
		{
			#region Advanced Defence
			bool rs=false;

			if (AdvancedDefence)
			{
				int os=side;
				int ox=xside;
				side=ox;
				xside=os;

				//Maximum for this phase (to speed up thinking and to leave some clops for next phase
				//int maxmov=(int)(ClpWar.clopNum/2);  

				if ((!beginning) & (ClpWar.clopLeft>=1))
				{
					//maxmov--;

					double pathl=double.MinValue;
					int bestx=-1;
					int besty=-1;
					double pl;
					double mpl=double.MaxValue;

					for (int i=0; (i<ClopWar.FieldW); i++)
						for (int j=0; (j<ClopWar.FieldH); j++)
							if (ClpWar.field[i,j].avail & (ClpWar.field[i,j].owner==side))
							{
								ClpWar.field[i,j].owner=xside;
								ClpWar.field[i,j].state=cell.DEAD;
								//
								EvalCells(); //ВОТ ГДЕ ТОРМОЗ!

								pl=FindPathLength();                            

								if (pl>pathl) //Find max
								{
									pathl=pl;
									bestx=i;
									besty=j;
								}

								if (pl<mpl) //Find min
									mpl=pl;

								ClpWar.field[i,j].owner=side;
								ClpWar.field[i,j].state=cell.CLOP;
							}

					if (ClpWar.CheckBound(bestx,besty) & ((int)(pathl-mpl)>=(TURN_EAT)))
					{
						ClpWar.MakeMove(bestx,besty);
						rs=true;
					}
					else
					{
						rs=false;
					}

				}

				side=os;
				xside=ox;
				EvalCells();
			}
			return rs;
			#endregion
		}

		public bool Stage3_Path()
		{
			#region Path finding

			bool rs=false;

			if (AdvancedPath /*& (!AdvancedDefence)*/)
			{
				//Find path
				FindPath();

				//Make turn according to the path
				IEnumerator e = bestpath.GetEnumerator();
				e.Reset();
				while ((ClpWar.turn==side)&(e.MoveNext())&(rs==false))
				{
					rs=ClpWar.MakeMove(((node)e.Current).px,((node)e.Current).py);
				} 
			}

			return rs;
			#endregion
           
		}

		public bool Stage4_Kill()
		{
			#region Simply Best Turns

			bool rs=false;
			
			int tx = -1; int ty = -1;	//Find Best Turn Coords
			double tv;
						
			//Do other logic
			if (ClpWar.turn==side)
			{
				tv=double.MaxValue;
				for (int i=0; (i<ClopWar.FieldW); i++)
					for (int j=0; (j<ClopWar.FieldH); j++)
					{
						if ((ClpWar.field[i,j].val<=tv) & ClpWar.field[i,j].avail)
						{
							tx=i; ty=j;
							tv=ClpWar.field[i,j].val;
						}
					}
					
				//Make the best found move
				rs=ClpWar.MakeMove(tx,ty);
			}

			return rs;

			#endregion
		}

		//Make turn
		public void Turn()
		{
			#region Init
			side = ClpWar.turn;		//Our side
			xside = cell.RED;		//Opposite side
			if (side==cell.RED) xside=cell.BLUE;
			
			//Find bases coords
			if (side==cell.RED)
			{
				bx  = ClopWar.RedBaseX;
				by  = ClopWar.RedBaseY;
				ebx = ClopWar.BlueBaseX;
				eby = ClopWar.BlueBaseY;
			}
			else
			{
				bx  = ClopWar.BlueBaseX;
				by  = ClopWar.BlueBaseY;
				ebx = ClopWar.RedBaseX;
				eby = ClopWar.RedBaseY;
			}
			#endregion

			//Pre-battle array
			Stage1_Array();

			//Evaluate cells
			EvalCells();

            //Logic cycle			
			while (ClpWar.turn==side)
			{
				while (Stage2_Defence()){};
				Stage3_Path();
                if (!Stage2_Defence())
					if (!Stage3_Path())
						Stage4_Kill();
			}		
			
		}

	}
}
