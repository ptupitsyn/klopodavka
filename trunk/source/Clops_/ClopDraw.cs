/////////////////////////////
/// code by TPS
/// tps0@hotmail.com
/////////////////////////////


using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;


namespace Clops_
{
	/// <summary>
	/// ClopDraw class draws ClopWar field.
	/// </summary>

	public class ClopDraw
	{

		#region Class header

		private System.Windows.Forms.Form OwnerForm;

		public int CellW=13;
		public int CellH=13;
		public int OffX=10;
		public int OffY=2;
		public int OffX2=230;
		public int OffY2=60;

		//Cursor pos
		private int cx;
		private int cy;
		public int Cx {get {return cx;}}
		public int Cy {get {return cy;}}

		public int FieldW, FieldH;

		public int[,] OldField;

		//Options
		public bool enablerefresh;
		public bool showtext = true;
		public bool showcursor = true;
		public bool showavail = true;
		public bool dispclopleft = true;

		public string InfoString;
		//end

		private ClopWar ClopWar;

		public ClopDraw(System.Windows.Forms.Form Frm)
		{
			//Init class members
			OwnerForm=Frm;	
		}
		#endregion

		public void InitForm(ClopWar clpw)
		{
			//Get ClopWar class intance
			ClopWar=clpw;
			FieldW=ClopWar.FieldW;
			FieldH=ClopWar.FieldH;
			//Init form
			OwnerForm.Width=FieldW*CellW+OffX2;
			OwnerForm.Height=FieldH*CellH+OffY2;
			OwnerForm.Top=10;
			OwnerForm.Left=10;
			//Cursor
			cx=ClopWar.RedBaseX;
			cy=ClopWar.RedBaseY;
			//refresh
			enablerefresh=true;
			//Info
			InfoString="Info";
			//OldField
            OldField=new int[FieldW,FieldH];
			for (int i=0;i<FieldW;i++)
				for (int j=0;j<FieldH;j++)
					OldField[i,j]=-1;
		}

		public void DrawField(System.Drawing.Graphics Grph)
		{
			/*
			if  (!needdraw) 
				return;
			else
				needdraw=false;
			*/

			Pen GridPen = new Pen(Color.FromArgb(100,Color.Black),1);

			//Red
			Brush RedBrush = new SolidBrush(Color.Red);
			LinearGradientBrush gRedBrush = new LinearGradientBrush(new Rectangle(0,0,CellW,CellH),Color.Red,Color.DarkRed,LinearGradientMode.Vertical);
			
			//Blue
			Brush BlueBrush = new SolidBrush(Color.Blue);
			LinearGradientBrush gBlueBrush = new LinearGradientBrush(new Rectangle(0,0,CellW,CellH),Color.DarkBlue,Color.Blue,LinearGradientMode.Vertical);

			//Other
			Brush BGBrush = new SolidBrush(Color.White);
			Brush DeadBrush = new SolidBrush(Color.Gray);
			LinearGradientBrush BackBrush = new LinearGradientBrush(new Rectangle(0,0,OwnerForm.Width,OwnerForm.Height),OwnerForm.BackColor,Color.Black,LinearGradientMode.Vertical);

			//BG bitmap
//			Bitmap BGpic = new Bitmap("art\\bg.bmp");
			Bitmap BGpic = new Bitmap(GetType(),"bg.bmp");

			//Clops bitmaps
			Bitmap RClop = new Bitmap(GetType(),"rclop.bmp");			
			Bitmap BClop = new Bitmap(GetType(),"bclop.bmp");
			Bitmap RClopD = new Bitmap(GetType(),"rclop_dead.bmp");			
			Bitmap BClopD = new Bitmap(GetType(),"bclop_dead.bmp");
			Bitmap RClopG = new Bitmap(GetType(),"rclop_gray.bmp");			
			Bitmap BClopG = new Bitmap(GetType(),"bclop_gray.bmp");
			RClop.MakeTransparent(Color.White);
			BClop.MakeTransparent(Color.White);
			RClopD.MakeTransparent(Color.White);
			BClopD.MakeTransparent(Color.White);
			RClopG.MakeTransparent(Color.White);
			BClopG.MakeTransparent(Color.White);
			Bitmap RedClop = new Bitmap(RClop,CellW+(int)(0.6*CellW),CellH+(int)(0.6*CellH));
			Bitmap BlueClop = new Bitmap(BClop,CellW+(int)(0.6*CellW),CellH+(int)(0.6*CellH));
			Bitmap RedClopG = new Bitmap(RClopG,CellW+(int)(0.6*CellW),CellH+(int)(0.6*CellH));
			Bitmap BlueClopG = new Bitmap(BClopG,CellW+(int)(0.6*CellW),CellH+(int)(0.6*CellH));
			Bitmap RedClopD = new Bitmap(RClopD,CellW+(int)(0.2*CellW),CellH+(int)(0.2*CellH));
			Bitmap BlueClopD = new Bitmap(BClopD,CellW+(int)(0.2*CellW),CellH+(int)(0.2*CellH));

			Pen RedPen = new Pen(RedBrush,1);
			Pen BluePen = new Pen(BlueBrush,1);
			Pen pen=RedPen;
			Brush brush=RedBrush;
			Bitmap bmp=RedClop;

			//Clear
			//Graph.FillRectangle(new SolidBrush(Color.White),OffX,OffY,FieldW*CellW,FieldH*CellH);
			//Grph.FillRectangle(BackBrush,new Rectangle(0,0,OwnerForm.Width,OwnerForm.Height));
			Grph.DrawImage(BGpic,OffX,OffY,CellW*FieldW,CellH*FieldH);			

			Rectangle rect = new Rectangle(0,0,CellW,CellH);
			Rectangle rct = new Rectangle(0,0,CellW-2,CellH-2);					
			Point pnt = new Point(0,0);
                    

			//Draw cells & clops
			for (int i=0;i<FieldH;i++)
				for (int j=0;j<FieldW;j++) 
					//if (OldField[j,i]!=ClopWar.field[j,i].state)					
				{
					rect.X=OffX+j*CellW;
					rect.Y=OffY+i*CellH;
					rct.X=OffX+j*CellW+1;
					rct.Y=OffY+i*CellH+1;

					switch(ClopWar.field[j,i].owner)
					{
						case cell.RED:
							pen = RedPen;
							brush = gRedBrush;
							if (ClopWar.field[j,i].state==cell.DEAD)
								bmp=RedClopD;
							else
								bmp=RedClop;
							break;
						case cell.BLUE:
							pen = BluePen;
							brush = gBlueBrush;
							if (ClopWar.field[j,i].state==cell.DEAD)
								bmp=BlueClopD;
							else
								bmp=BlueClop;
							break;
						case cell.EMPTY:
							pen = GridPen;
							brush = BGBrush;
							if (ClopWar.turn==cell.RED) 
								bmp=RedClopG;
							else
								bmp=BlueClopG;
							break;
					}

					//Grph.FillRectangle(BGBrush,rect);
					pnt.X=OffX+j*CellW-(int)(bmp.Width-CellW)/2;
					pnt.Y=OffY+i*CellH-(int)(bmp.Height-CellH)/2;
					

					switch(ClopWar.field[j,i].state)
					{
						case cell.CLOP:
							//Grph.DrawEllipse(pen,rct);
							Grph.DrawImageUnscaled(bmp,pnt);
							break;
						case cell.DEAD:
							Grph.DrawImageUnscaled(bmp,pnt);
							break;
						case cell.BASE:
							Grph.FillRectangle(brush,rect);
							if (ClopWar.turn==ClopWar.field[j,i].owner) 
							{
								Grph.DrawEllipse(GridPen,rct);
							};
							break;
					}

					if (showavail)
						if (ClopWar.field[j,i].avail)
							//Grph.DrawImageUnscaled(bmp,pnt);
							Grph.DrawLine(GridPen,OffX+j*CellW+CellW/2,OffY+i*CellH+CellH/2,OffX+j*CellW+CellW/2+1,OffY+i*CellH+CellH/2+1);

					OldField[j,i]=ClopWar.field[j,i].state;
				}

			//Draw grid
			for (int i=0;i<=FieldW;i++)
				Grph.DrawLine(GridPen,OffX+i*CellW,OffY,OffX+i*CellW,OffY+FieldH*CellH);
			for (int i=0;i<=FieldH;i++)
				Grph.DrawLine(GridPen,OffX,OffY+i*CellH,OffX+FieldW*CellW,OffY+i*CellH);
			
			//Draw cursor
			if (showcursor)
                Grph.FillRectangle(new SolidBrush(Color.FromArgb(130,Color.Gray)),OffX+cx*CellW,OffY+cy*CellH,CellW,CellH);			

			//Draw status
			if (showtext)
			{
				string s;
				s="Turn: ";
				if (ClopWar.gameStatus==ClopWar.GAME_RED)
					s+="RED";
				if (ClopWar.gameStatus==ClopWar.GAME_BLUE)
					s+="BLUE";
				s+=" "+(char)10+(char)13;
				//s+="Coords(x,y,val): "+cx.ToString()+", "+cy.ToString()+", ";
				//s+=ClopWar.field[cx,cy].cost.ToString()+(char)10+(char)13;
				s+="Clops left: "+ClopWar.clopLeft.ToString()+" of "+ClopWar.clopNum.ToString();
				//s+=" "+(char)10+(char)13+" "+(char)10+(char)13+InfoString;
			
				RectangleF trect = new RectangleF(OffX+FieldW*CellW+10,OffY,200,200);
				Grph.DrawString("Game Status",new Font("Courier",18,FontStyle.Bold),new LinearGradientBrush(new Point(0,0),new Point(800,800),Color.Red,Color.Blue),trect);
				trect.Y+=30;
				Grph.DrawString(s,new Font("Arial",10),new SolidBrush(Color.White),trect);
				trect.X++; trect.Y++;
				Grph.DrawString(s,new Font("Arial",10),new SolidBrush(Color.Black),trect);
			}

			if (dispclopleft)
			{

				//Clops left
				//Gray
				//bmp=RClopG;
				//if (ClopWar.turn==cell.BLUE) bmp=BClopG;
				for (int i=-ClopWar.clopNum;i<0;i++)
				{
					Grph.DrawImage(RClopG,OffX+FieldW*CellW+10,OffY+FieldH*CellH+(i-1)*(OwnerForm.Height/ClopWar.clopNum/2),50,50);
					Grph.DrawImage(BClopG,OffX+FieldW*CellW+70,OffY+FieldH*CellH+(i-1)*(OwnerForm.Height/ClopWar.clopNum/2),50,50);
				}
				//Color
				bmp=RClop;
				int oo=0;
				if (ClopWar.turn==cell.BLUE)
				{
					bmp=BClop;
					oo=60;
				}
			
				for (int i=-ClopWar.clopLeft;i<0;i++)
				{
					Grph.DrawImage(bmp,OffX+FieldW*CellW+10+oo,OffY+FieldH*CellH+(i-1)*(OwnerForm.Height/ClopWar.clopNum/2),50,50);
					Grph.DrawImage(bmp,OffX+FieldW*CellW+10+oo,OffY+FieldH*CellH+(i-1)*(OwnerForm.Height/ClopWar.clopNum/2),50,50);
				}

			}
			

			//DISPOSE!!!
			GridPen.Dispose();
			RedBrush.Dispose();
			gRedBrush.Dispose();
			BlueBrush.Dispose();
			gBlueBrush.Dispose();
			BGBrush.Dispose();
			DeadBrush.Dispose();
			BackBrush.Dispose();
			RClop.Dispose();
			BClop.Dispose();
			RClopD.Dispose();
			BClopD.Dispose();
			RedClop.Dispose();
			BlueClop.Dispose();
			RedClopD.Dispose();
			BlueClopD.Dispose();
			RedPen.Dispose();
			BluePen.Dispose();
		}

		public bool XYinGrid(int x, int y)
		{
			return ((x>OffX)&(y>OffY)&(x<OffX+FieldW*CellW)&(y<OffY+FieldH*CellH));
		}

		public void ParseClick(int x, int y)
		{
			//Find cell where mouse clicked and change it's state
			if (XYinGrid(x,y)) 
			{
				int X=(x-OffX)/CellW;
				int Y=(y-OffY)/CellH;
				ClopWar.MakeMove(X,Y);
				cx=X; cy=Y;
				
				OwnerForm.Invalidate();
			}
		}

		public void ParseKey(System.Windows.Forms.KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case (Keys.Enter):
					ClopWar.MakeMove(cx,cy);
					OwnerForm.Invalidate();
					break;
				case (Keys.Space):
					ClopWar.MakeMove(cx,cy);
					OwnerForm.Invalidate();
					break;
				case (Keys.Clear):
					ClopWar.MakeMove(cx,cy);
					OwnerForm.Invalidate();
					break;
				case Keys.Up:
					if (cy>0) cy--;
					OwnerForm.Invalidate();
					break;
                case Keys.Down:
					if (cy+1<FieldH) cy++;
					OwnerForm.Invalidate();
					break;
				case Keys.Left:
					if (cx>0) cx--;
					OwnerForm.Invalidate();
					break;
				case Keys.Right:
					if (cx+1<FieldW) cx++;
					OwnerForm.Invalidate();
					break;
				case Keys.Prior:
					if ((cx+1<FieldW) & (cy>0)) {cx++; cy--;}
					OwnerForm.Invalidate();
					break;
				case Keys.Next:
					if ((cx+1<FieldW) & (cy+1<FieldH)) {cx++; cy++;}
					OwnerForm.Invalidate();
					break;
				case Keys.Home:
					if ((cx>0) & (cy>0)) {cx--; cy--;}
					OwnerForm.Invalidate();
					break;
				case Keys.End:
					if ((cx>0) & (cy+1<FieldH)) {cx--; cy++;}
					OwnerForm.Invalidate();
					break;
					/*default:
					MessageBox.Show(key.ToString());
					break;*/
			}
		}


		public void Refresh()
		{
			if (enablerefresh)
			{
				OwnerForm.Invalidate();
				System.Windows.Forms.Application.DoEvents();
			}
		}

	}
}
