/////////////////////////////
/// code by TPS
/// tps0@hotmail.com
/////////////////////////////


using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Clops.Ai;
using Clops.Ifaces;


namespace clops.drawing.win32
{
    /// <summary>
    /// ClopDraw class draws ClopWar Field.
    /// </summary>

    public class WinClopDraw : IClopDrawer
    {

        #region Class header

        private readonly Form _ownerForm;

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

        private IClopWar _clopWar;

        public WinClopDraw(Form Frm)
        {
            //Init class members
            _ownerForm=Frm;	
        }
        #endregion

        public void InitForm(IClopWar clpw)
        {
            //Get ClopWar class intance
            _clopWar=clpw; // as ClopWar;
            FieldW=ClopWar.FieldW;
            FieldH=ClopWar.FieldH;
            //Init form
            _ownerForm.Width=FieldW*CellW+OffX2;
            _ownerForm.Height=FieldH*CellH+OffY2;
            _ownerForm.Top=10;
            _ownerForm.Left=10;
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
            LinearGradientBrush BackBrush = new LinearGradientBrush(new Rectangle(0,0,_ownerForm.Width,_ownerForm.Height),_ownerForm.BackColor,Color.Black,LinearGradientMode.Vertical);

            //BG bitmap
//			Bitmap BGpic = new Bitmap("art\\bg.bmp");
            Bitmap BGpic = new Bitmap(typeof(WinClopDraw), "Resources.bg.bmp");

            //Clops bitmaps
            Bitmap RClop = new Bitmap(typeof(WinClopDraw), "Resources.rclop.bmp");
            Bitmap BClop = new Bitmap(typeof(WinClopDraw), "Resources.bclop.bmp");
            Bitmap RClopD = new Bitmap(typeof(WinClopDraw), "Resources.rclop_dead.bmp");
            Bitmap BClopD = new Bitmap(typeof(WinClopDraw), "Resources.bclop_dead.bmp");
            Bitmap RClopG = new Bitmap(typeof(WinClopDraw), "Resources.rclop_gray.bmp");
            Bitmap BClopG = new Bitmap(typeof(WinClopDraw), "Resources.bclop_gray.bmp");
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
            //Grph.FillRectangle(BackBrush,new Rectangle(0,0,_ownerForm.Width,_ownerForm.Height));
            Grph.DrawImage(BGpic,OffX,OffY,CellW*FieldW,CellH*FieldH);			

            Rectangle rect = new Rectangle(0,0,CellW,CellH);
            Rectangle rct = new Rectangle(0,0,CellW-2,CellH-2);					
            Point pnt = new Point(0,0);
                    

            //Draw cells & clops
            for (int i=0;i<FieldH;i++)
                for (int j=0;j<FieldW;j++) 
                    //if (OldField[j,i]!=ClopWar.Field[j,i].state)					
                {
                    rect.X=OffX+j*CellW;
                    rect.Y=OffY+i*CellH;
                    rct.X=OffX+j*CellW+1;
                    rct.Y=OffY+i*CellH+1;

                    switch(_clopWar.Field[j,i].Owner)
                    {
                        case Cell.RED:
                            pen = RedPen;
                            brush = gRedBrush;
                            if (_clopWar.Field[j,i].State==Cell.DEAD)
                                bmp=RedClopD;
                            else
                                bmp=RedClop;
                            break;
                        case Cell.BLUE:
                            pen = BluePen;
                            brush = gBlueBrush;
                            if (_clopWar.Field[j,i].State==Cell.DEAD)
                                bmp=BlueClopD;
                            else
                                bmp=BlueClop;
                            break;
                        case Cell.EMPTY:
                            pen = GridPen;
                            brush = BGBrush;
                            if (_clopWar.turn==Cell.RED) 
                                bmp=RedClopG;
                            else
                                bmp=BlueClopG;
                            break;
                    }

                    //Grph.FillRectangle(BGBrush,rect);
                    pnt.X=OffX+j*CellW-(bmp.Width-CellW)/2;
                    pnt.Y=OffY+i*CellH-(bmp.Height-CellH)/2;
					

                    switch(_clopWar.Field[j,i].State)
                    {
                        case Cell.CLOP:
                            //Grph.DrawEllipse(pen,rct);
                            Grph.DrawImageUnscaled(bmp,pnt);
                            break;
                        case Cell.DEAD:
                            Grph.DrawImageUnscaled(bmp,pnt);
                            break;
                        case Cell.BASE:
                            Grph.FillRectangle(brush,rect);
                            if (_clopWar.turn==_clopWar.Field[j,i].Owner) 
                            {
                                Grph.DrawEllipse(GridPen,rct);
                            };
                            break;
                    }

                    if (showavail)
                        if (_clopWar.Field[j,i].Avail)
                            //Grph.DrawImageUnscaled(bmp,pnt);
                            Grph.DrawLine(GridPen,OffX+j*CellW+CellW/2,OffY+i*CellH+CellH/2,OffX+j*CellW+CellW/2+1,OffY+i*CellH+CellH/2+1);

                    OldField[j,i]=_clopWar.Field[j,i].State;
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
                if (_clopWar.gameStatus==ClopWar.GAME_RED)
                    s+="RED";
                if (_clopWar.gameStatus==ClopWar.GAME_BLUE)
                    s+="BLUE";
                s+=" "+(char)10+(char)13;
                //s+="Coords(x,y,val): "+cx.ToString()+", "+cy.ToString()+", ";
                //s+=ClopWar.Field[cx,cy].cost.ToString()+(char)10+(char)13;
                s+="Clops left: "+_clopWar.clopLeft.ToString()+" of "+_clopWar.clopNum.ToString();
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
                for (int i=-_clopWar.clopNum;i<0;i++)
                {
                    Grph.DrawImage(RClopG,OffX+FieldW*CellW+10,OffY+FieldH*CellH+(i-1)*(_ownerForm.Height/_clopWar.clopNum/2),50,50);
                    Grph.DrawImage(BClopG,OffX+FieldW*CellW+70,OffY+FieldH*CellH+(i-1)*(_ownerForm.Height/_clopWar.clopNum/2),50,50);
                }
                //Color
                bmp=RClop;
                int oo=0;
                if (_clopWar.turn==Cell.BLUE)
                {
                    bmp=BClop;
                    oo=60;
                }
			
                for (int i=-_clopWar.clopLeft;i<0;i++)
                {
                    Grph.DrawImage(bmp,OffX+FieldW*CellW+10+oo,OffY+FieldH*CellH+(i-1)*(_ownerForm.Height/_clopWar.clopNum/2),50,50);
                    Grph.DrawImage(bmp,OffX+FieldW*CellW+10+oo,OffY+FieldH*CellH+(i-1)*(_ownerForm.Height/_clopWar.clopNum/2),50,50);
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
                _clopWar.MakeMove(X,Y);
                cx=X; cy=Y;
				
                _ownerForm.Invalidate();
            }
        }

        public void ParseKey(System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case (Keys.Enter):
                    _clopWar.MakeMove(cx,cy);
                    _ownerForm.Invalidate();
                    break;
                case (Keys.Space):
                    _clopWar.MakeMove(cx,cy);
                    _ownerForm.Invalidate();
                    break;
                case (Keys.Clear):
                    _clopWar.MakeMove(cx,cy);
                    _ownerForm.Invalidate();
                    break;
                case Keys.Up:
                    if (cy>0) cy--;
                    _ownerForm.Invalidate();
                    break;
                case Keys.Down:
                    if (cy+1<FieldH) cy++;
                    _ownerForm.Invalidate();
                    break;
                case Keys.Left:
                    if (cx>0) cx--;
                    _ownerForm.Invalidate();
                    break;
                case Keys.Right:
                    if (cx+1<FieldW) cx++;
                    _ownerForm.Invalidate();
                    break;
                case Keys.Prior:
                    if ((cx+1<FieldW) & (cy>0)) {cx++; cy--;}
                    _ownerForm.Invalidate();
                    break;
                case Keys.Next:
                    if ((cx+1<FieldW) & (cy+1<FieldH)) {cx++; cy++;}
                    _ownerForm.Invalidate();
                    break;
                case Keys.Home:
                    if ((cx>0) & (cy>0)) {cx--; cy--;}
                    _ownerForm.Invalidate();
                    break;
                case Keys.End:
                    if ((cx>0) & (cy+1<FieldH)) {cx--; cy++;}
                    _ownerForm.Invalidate();
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
                _ownerForm.Invalidate();
                System.Windows.Forms.Application.DoEvents();
            }
        }
    }
}