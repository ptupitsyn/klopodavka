/////////////////////////////
/// code by TPS
/// tps0@hotmail.com
/////////////////////////////

using System;
using System.IO;
using Clops.Ai.Algo;
using Clops.Ifaces;


namespace Clops.Ai
{
	/// <summary>
	/// Class ClopWar
	/// Handles all processes on the Field of CLOP WARS.
	/// Does turns from any source (AI, Human)
	/// </summary>
	
	#region Struct cell

    public class Cell : Node, IClopCell
    {
		public const int EMPTY		= 0;
		public const int RED		= 1;
		public const int BLUE		= 2;
		public const int BASE		= 3;
		public const int CLOP     	= 4;
		public const int DEAD		= 5;
		public const int AVAIL    	= 6;

        public int Owner { get; set; }

	    public int State { get; set; }

	    public bool Flag { get; set; }	
		public bool Avail { get; set; }
        public double Value { get; set; }

		public Cell(int x, int y)
		{
			State=EMPTY;
			Flag=false;
			Owner=EMPTY;
			Avail=false;
			Value=0;
			px=x; py=y;
		}
	}
	#endregion

	public class ClopWar : IClopWar
	{	
	    
		#region Class header

		public IClopDrawer Drawer;

		// FIELD
		public const int FieldW=33; //33
		public const int FieldH=40; //40
		public const int RedBaseX= 4;
		public const int RedBaseY=FieldH-5;
		public const int BlueBaseX=FieldW-5;
		public const int BlueBaseY= 4;
		public const int StdClopNum = 10;
		int side;		//Own side
	    int bx;			//own base
		int by;

	    private Cell[,] _field = new Cell[FieldW,FieldH];
		
        public  IClopCell[,] Field 
		{
			get{return _field;}
            set { _field = value as Cell[,]; }
		}

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
		public ClopWar(IClopDrawer drw)
		{
			//Init Drawer
			Drawer=drw;
			Drawer.InitForm(this);
			
			//Init Data
			RedFirst=true;
			ClopNum=StdClopNum;
			AvailNum=0;
			GameStatus=GAME_WAIT;
            RecFileExt = FieldW + "x" + FieldH;

			//Init cells
			for (int i=0; i<FieldW; i++) 
				for (int j=0; j<FieldH; j++) 
				{
					_field[i,j]=new Cell(i,j);
				}
			PlaceBase();
			CountFree();
		}

		private void PlaceBase()
		{
			//Reset Field
			for (int i=0; i<FieldW; i++) 
				for (int j=0; j<FieldH; j++) 
				{
					_field[i,j].Flag=false;
					_field[i,j].Avail=false;
					_field[i,j].State=Cell.EMPTY;
					_field[i,j].Owner=Cell.EMPTY;
				}
			//Place bases
			_field[BlueBaseX,BlueBaseY].State=Cell.BASE;
			_field[BlueBaseX,BlueBaseY].Owner=Cell.BLUE;
			_field[RedBaseX,RedBaseY].State=Cell.BASE;
			_field[RedBaseX,RedBaseY].Owner=Cell.RED;
			ClopLeft=ClopNum;
			Turn=Cell.BLUE;
			GameStatus=GAME_BLUE;
			if (RedFirst)
			{
				GameStatus=GAME_RED;
				Turn=Cell.RED;
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
        public bool CheckBound(int x, int y) //True if (x,y) in Field
		{
            return (x >= 0) && (y >= 0) && (x < FieldW) && (y < FieldH);
		}

        //Additional func for FindAvail - perform routine bounds&property checks & mark if neccesary
        private void FindAvailCheck(int x, int y, int st)
        {
            if (CheckBound(x, y))
            {
                if ((_field[x, y].Owner == Cell.EMPTY) | ((_field[x, y].Owner != st) & (_field[x, y].Owner != Cell.EMPTY) & (_field[x, y].State == Cell.CLOP) & (_field[x, y].State != Cell.BASE)))
                {
                    _field[x, y].Avail = true;
                    AvailNum++;
                    // Remove comments to watch animated processing
                    //Drawer.Refresh();
                }
                else FindAvail(x, y, st);
            }
        }

        //Recursively checks all neighbour cells
        private void FindAvail(int x, int y, int st)
		{
            if (!CheckBound(x, y) || (_field[x, y].Owner != st) || (_field[x, y].Flag)) return;
            _field[x, y].Flag = true;
            //Check neighbour cells
            int[] dx = new int[8] { -1, -1, -1, 1, 1, 1, 0, 0 };
            int[] dy = new int[8] { -1, 0, 1, -1, 0, 1, -1, 1 };
            for (int i = 0; i <= 7; i++)
                FindAvailCheck(x + dx[i], y + dy[i], st);
        }
		
		private void CountFree()
			//Marks & counts all available for turn cells
		{
			side = turn;		//Our side
			
			//Find bases coords
			if (side==Cell.RED)
			{
				bx  = RedBaseX;
				by  = RedBaseY;
			}
			else
			{
				bx  = BlueBaseX;
				by  = BlueBaseY;
			}

			//Zero flags & avails
			for (int i=0; i<FieldW; i++) 
				for (int j=0; j<FieldH; j++) 
				{
					_field[i,j].Flag=false;
					_field[i,j].Avail=false;
					//_field[i,j].dist=Distance(i,j,bx,by)*2+0.1; //Empirical distance - to get searching faster
				}
			//Count available
			AvailNum=0;
			//Check
			FindAvail(bx,by,side);
		}
		#endregion

		#region Turn handling code
		public bool MakeMove(int i, int j)
		{
			if (CheckBound(i,j))
			if (_field[i,j].Avail)
			{
				//Change cell props
				_field[i,j].Owner=Turn;
				if (_field[i,j].State==Cell.CLOP)
						_field[i,j].State=Cell.DEAD;
					else
                        _field[i,j].State=Cell.CLOP;

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
					if (Turn==Cell.RED)
						GameStatus=GAME_BLUEWON;
					else
						GameStatus=GAME_REDWON;
					Turn=Cell.EMPTY;
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
            if ((ClopLeft == ClopNum) || (ClopLeft == 0)) return;

			//remove from history
			TurnNum--;

			//Change cell props
			int i=TurnHist[TurnNum,0];
			int j=TurnHist[TurnNum,1];
			if (_field[i,j].State==Cell.DEAD)
			{
				_field[i,j].State=Cell.CLOP;
				if (_field[i,j].Owner==Cell.RED)
					_field[i,j].Owner=Cell.BLUE; 
				else
					_field[i,j].Owner=Cell.RED;						
			}
			else
			{
				_field[i,j].State=Cell.EMPTY;
				_field[i,j].Owner=Cell.EMPTY;
			}

			//If recording
			if (GameStatus==GAME_RECORDING)
				if (RecSize>0) RecSize--;

			//other
			ClopLeft++;
			CountFree();
			if (AutoRedraw) Drawer.Refresh();
		}

		public void SwitchTurn()
		{
			SwitchTurn(false, false, false);
		}
		
		public void SwitchTurn(bool force, bool setRed, bool setBlue)
		{
			if (force || ((--ClopLeft) == 0))
			{

				//If recording
				if (GameStatus==GAME_RECORDING)
                    StopRecording("def." + RecFileExt);

				ClopLeft = ClopNum;
				if (setBlue || (Turn == Cell.RED) && !setRed)
				{
					Turn = Cell.BLUE;
					GameStatus = GAME_BLUE;
				}
				else
				{
					Turn = Cell.RED;
					GameStatus = GAME_RED;
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
				if (Turn!=Cell.RED)
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
