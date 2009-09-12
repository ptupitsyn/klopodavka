/////////////////////////////
/// code by TPS
/// tps0@hotmail.com
/////////////////////////////

using System;
using System.IO;


namespace Clops_
{
	/// <summary>
	/// Class ClopWar
	/// Handles all processes on the field of CLOP WARS.
	/// Does turns from any source (AI, Human)
	/// </summary>
	
	#region Struct cell
	public class cell : node
	{
		public const int EMPTY		= 0;
		public const int RED		= 1;
		public const int BLUE		= 2;
		public const int BASE		= 3;
		public const int CLOP     	= 4;
		public const int DEAD		= 5;
		public const int AVAIL    	= 6;
		public int owner;
		public int state;
//		public double dist;	//distance
		public bool flag;	
		public bool avail;
		public double val;
		public double val2; //Additional value (including best path)
		public cell(int x, int y)
		{
			state=EMPTY;
			flag=false;
			owner=EMPTY;
			avail=false;
			val=0;
			val2=0;
			px=x; py=y;
//			dist=-1;
		}
	}
	#endregion

	public class ClopWar
	{	
	    
		#region Class header

		public ClopDraw Drawer;

		// FIELD
		public const int FieldW=33; //33
		public const int FieldH=40; //40
		public const int RedBaseX= 4;
		public const int RedBaseY=FieldH-5;
		public const int BlueBaseX=FieldW-5;
		public const int BlueBaseY= 4;
		public const int StdClopNum = 10;
		int side;		//Own side
		int xside;		//Enemy side
		int bx;			//own base
		int by;
		int ebx;		//enemy base
		int eby;
		
		private cell[,] Field = new cell[FieldW,FieldH];
		public  cell[,] field {get{return Field;}}

		public bool RedFirst;

		public int ClopNum;
		public int clopNum
		{
			get{return ClopNum;}
		}

		private int Turn;
		public int turn{get{return Turn;}}

		private int ClopLeft;
		public int clopLeft{get{return ClopLeft;}}

		private int AvailNum;

		//Game status processing
		public const int GAME_WAIT = 0;
		public const int GAME_RED = 1;
		public const int GAME_BLUE = 2;
		public const int GAME_REDWON = 3;
		public const int GAME_BLUEWON = 4;
		public const int GAME_RECORDING = 5;
		private int GameStatus;
		public int gameStatus{get{return GameStatus;}}

		//Recording & replaying
		private int[,] RecArray = new int[FieldW*FieldH,2];
		public string RecFileExt = null;
		private int RecSize = 0;
		private int CurPlace = 0;
		//Turn history
		private int[,] TurnHist = new int[FieldW*FieldH,2];
		private int TurnNum;
		//Misc
		public bool AutoRedraw = true;

		#endregion

		#region Initializing code
		public ClopWar(ClopDraw drw)
		{
			//Init Drawer
			Drawer=drw;
			Drawer.InitForm(this);
			
			//Init Data
			RedFirst=true;
			ClopNum=StdClopNum;
			AvailNum=0;
			GameStatus=GAME_WAIT;
			RecFileExt=FieldW.ToString()+"x"+FieldH.ToString();

			//Init cells
			for (int i=0; i<FieldW; i++) 
				for (int j=0; j<FieldH; j++) 
				{
					Field[i,j]=new cell(i,j);
				}
			PlaceBase();
			CountFree();
		}

		private void PlaceBase()
		{
			//Reset field
			for (int i=0; i<FieldW; i++) 
				for (int j=0; j<FieldH; j++) 
				{
					Field[i,j].flag=false;
					Field[i,j].avail=false;
					Field[i,j].state=cell.EMPTY;
					Field[i,j].owner=cell.EMPTY;
				}
			//Place bases
			Field[BlueBaseX,BlueBaseY].state=cell.BASE;
			Field[BlueBaseX,BlueBaseY].owner=cell.BLUE;
			Field[RedBaseX,RedBaseY].state=cell.BASE;
			Field[RedBaseX,RedBaseY].owner=cell.RED;
			ClopLeft=ClopNum;
			Turn=cell.BLUE;
			GameStatus=GAME_BLUE;
			if (RedFirst)
			{
				GameStatus=GAME_RED;
				Turn=cell.RED;
			}
			TurnNum=0;
		}

		public void ResetGame()
		{
			GameStatus=GAME_WAIT;
			PlaceBase();
			CountFree();
			Drawer.Refresh();
		}

		#endregion

		#region Turn checking code
		public bool CheckBound(int x, int y)
			//True if (x,y) in field
		{
			if ((x>=0) & (y>=0) & (x<FieldW) & (y<FieldH)) 
				return true;
			else
				return false;
		}

		private void FindAvailCheck(int x, int y, int st)
			//Additional func for FindAvail - perform routine bounds&property checks & mark if neccesary
		{
			if (CheckBound(x,y))
			{
				if ((Field[x,y].owner==cell.EMPTY)|( (Field[x,y].owner!=st)&(Field[x,y].owner!=cell.EMPTY)&(Field[x,y].state==cell.CLOP)&(Field[x,y].state!=cell.BASE) )) 
				{
					Field[x,y].avail=true;
					AvailNum++;
					// Remove comments to watch animated processing
					//Drawer.Refresh();
				}
				else FindAvail(x,y,st,0);
			}
		}

		private void FindAvail(int x,int y, int st, double r)
			//Recursively checks all neighbour cells

		{
			if (!CheckBound(x,y)|(Field[x,y].owner!=st)|(Field[x,y].flag)) return;
			Field[x,y].flag=true;
			//Check neighbour cells
			int[] dx=new int[8] { -1, -1, -1, 1 , 1 , 1 , 0 , 0 };
			int[] dy=new int[8] { -1, 0 , 1 , -1, 0 , 1 , -1, 1 };
			for (int i=0;i<=7;i++)
				FindAvailCheck(x+dx[i],y+dy[i],st);
		}
		
		private void CountFree()
			//Marks & counts all available for turn cells
		{
			side = turn;		//Our side
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

			//Zero flags & avails
			for (int i=0; i<FieldW; i++) 
				for (int j=0; j<FieldH; j++) 
				{
					Field[i,j].flag=false;
					Field[i,j].avail=false;
					//Field[i,j].dist=Distance(i,j,bx,by)*2+0.1; //Empirical distance - to get searching faster
				}
			//Count available
			AvailNum=0;
			//Check
			FindAvail(bx,by,side,0);
		}
		#endregion

		#region Turn handling code
		public bool MakeMove(int i, int j)
		{
			if (CheckBound(i,j))
			if (Field[i,j].avail)
			{
				//Change cell props
				Field[i,j].owner=Turn;
				if (Field[i,j].state==cell.CLOP)
						Field[i,j].state=cell.DEAD;
					else
                        Field[i,j].state=cell.CLOP;

				//Add to history
				TurnHist[TurnNum,0]=i;
				TurnHist[TurnNum,1]=j;
				TurnNum++;

				//If recording
				if (GameStatus==GAME_RECORDING)
                    RecordTurn(i,j);

				//Other
				SwitchTurn();
				CountFree();
				if (AutoRedraw) Drawer.Refresh();

				//Check victory
				if (AvailNum<=0)
				{
					if (Turn==cell.RED)
						GameStatus=GAME_BLUEWON;
					else
						GameStatus=GAME_REDWON;
					Turn=cell.EMPTY;
				}

				return true;
			}
			else
				return false;
			else
				return false;
		}

		public void UndoTurn()
		{
			if ((ClopLeft==ClopNum)|(ClopLeft==0)) return;

			//remove from history
			TurnNum--;

			//Change cell props
			int i=TurnHist[TurnNum,0];
			int j=TurnHist[TurnNum,1];
			if (Field[i,j].state==cell.DEAD)
			{
				Field[i,j].state=cell.CLOP;
				if (Field[i,j].owner==cell.RED)
					Field[i,j].owner=cell.BLUE; 
				else
					Field[i,j].owner=cell.RED;						
			}
			else
			{
				Field[i,j].state=cell.EMPTY;
				Field[i,j].owner=cell.EMPTY;
			}

			//If recording
			if (GameStatus==GAME_RECORDING)
				if (RecSize>0) RecSize--;

			//other
			ClopLeft++;
			CountFree();
			if (AutoRedraw) Drawer.Refresh();
		}

		private void SwitchTurn()
		{
			ClopLeft--;
			if (ClopLeft==0)
			{

				//If recording
				if (GameStatus==GAME_RECORDING)
					StopRecording("def."+this.RecFileExt);

				ClopLeft=ClopNum;
				if (Turn==cell.RED)
				{
					Turn=cell.BLUE;
					GameStatus=GAME_BLUE;
				}
				else
				{
					Turn=cell.RED;
					GameStatus=GAME_RED;
				}
				//Drawer.Refresh();
			}
		}

		#endregion

		#region Additional functionality

		public void StartRecording()
		{
            ResetGame();
			GameStatus=GAME_RECORDING;
			ClopLeft=FieldH*FieldW-4;
			RecSize=0;
			Drawer.Refresh();
		}

		public void StopRecording(string fln)
		{
			ResetGame();
			//Save recorded to the file
			FileStream fs = new FileStream(fln, FileMode.OpenOrCreate);
			// Create the writer for data.
			BinaryWriter w = new BinaryWriter(fs);
			// Write sequence to file
			for (int i=0;i<RecArray.GetLength(0);i++)
			{
				w.Write(RecArray[i,0]);
				w.Write(RecArray[i,1]);
			}
			w.Write(RecSize);
			w.Close();
			fs.Close();
		}

		public void RecordTurn(int x, int y)
		{
			RecSize++;
			RecArray[RecSize-1,0]=x;
			RecArray[RecSize-1,1]=y;
		}

		public int PlayRecord(string Fln, int TurnNum)
		{
			//Load recorded from file & play
			FileStream fs = new FileStream(Fln, FileMode.Open);
			// Create the reader for data.
			BinaryReader r = new BinaryReader(fs);
			// Write sequence to file
			for (int i=0;i<RecArray.GetLength(0);i++)
			{
				RecArray[i,0]=r.ReadInt32();
				RecArray[i,1]=r.ReadInt32();
				//Rotate when needed
				if (Turn!=cell.RED)
				{
					RecArray[i,0]=FieldW-1-RecArray[i,0];
					RecArray[i,1]=FieldH-1-RecArray[i,1];
				}
			}
			RecSize=r.ReadInt32();
			r.Close();
			fs.Close();

			//PLAY
			CurPlace=0;
			int side = Turn;
			int turnnum=0;
			while ((CurPlace<RecSize)&(side==Turn)&(turnnum<TurnNum))
			{
				if (MakeMove(RecArray[CurPlace,0],RecArray[CurPlace,1]))
					turnnum++;
				CurPlace++;				
			}
			return turnnum;
		}

		public double Distance(int x1, int y1, int x2, int y2)
		{
			int dx=x1-x2;
			int dy=y1-y2;
			return Math.Sqrt(dx*dx+dy*dy);
		}

		#endregion

	}
}
