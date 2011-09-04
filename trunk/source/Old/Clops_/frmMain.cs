/////////////////////////////
/// code by TPS
/// tps0@hotmail.com
/////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using System.IO;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;

using System.Runtime.InteropServices;

using Clops.Ai;
using clops.drawing.win32;

namespace Clops_
{
	/// <summary>
	/// frmMain - form for displaying & handling Clop Field
	/// passed to ClopWar as a container
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		#region Class Header


		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmMain));
			this.mnuMain = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuMakeAutoTurn = new System.Windows.Forms.MenuItem();
			this.menuLoadStrategy = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.btnRedFirst = new System.Windows.Forms.MenuItem();
			this.btnBlueFirst = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.btnOppComp = new System.Windows.Forms.MenuItem();
			this.btnOppHuman = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuDifficultyEasy = new System.Windows.Forms.MenuItem();
			this.menuDifficultyHard = new System.Windows.Forms.MenuItem();
			this.menuDifficultyImpossible = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.btnStrategy = new System.Windows.Forms.MenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.StatusTimer = new System.Windows.Forms.Timer(this.components);
			this.TurnProgress = new System.Windows.Forms.ProgressBar();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// mnuMain
			// 
			this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.menuItem1,
																					this.menuItem2,
																					this.menuItem3,
																					this.menuItem4,
																					this.menuItem5,
																					this.menuItem7,
																					this.menuItem9});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "New Game";
			this.menuItem1.Click += new System.EventHandler(this.btnNewGame_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "Record";
			this.menuItem2.Click += new System.EventHandler(this.btnRecord_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 2;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuMakeAutoTurn,
																					  this.menuLoadStrategy});
			this.menuItem3.Text = "Auto turn";
			// 
			// menuMakeAutoTurn
			// 
			this.menuMakeAutoTurn.Index = 0;
			this.menuMakeAutoTurn.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.menuMakeAutoTurn.Text = "Make auto turn";
			this.menuMakeAutoTurn.Click += new System.EventHandler(this.AutoTurn_OnClick);
			// 
			// menuLoadStrategy
			// 
			this.menuLoadStrategy.Index = 1;
			this.menuLoadStrategy.Shortcut = System.Windows.Forms.Shortcut.F3;
			this.menuLoadStrategy.Text = "Load strategy...";
			this.menuLoadStrategy.Click += new System.EventHandler(this.menuLoadStrategy_OnClick);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.menuItem4.Text = "Undo turn";
			this.menuItem4.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 4;
			this.menuItem5.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.btnRedFirst,
																					  this.btnBlueFirst,
																					  this.menuItem6,
																					  this.menuItem8});
			this.menuItem5.Text = "Options";
			// 
			// btnRedFirst
			// 
			this.btnRedFirst.Checked = true;
			this.btnRedFirst.Index = 0;
			this.btnRedFirst.RadioCheck = true;
			this.btnRedFirst.Text = "Red First";
			this.btnRedFirst.Click += new System.EventHandler(this.btnRedFirst_OnClick);
			// 
			// btnBlueFirst
			// 
			this.btnBlueFirst.Index = 1;
			this.btnBlueFirst.RadioCheck = true;
			this.btnBlueFirst.Shortcut = System.Windows.Forms.Shortcut.Ins;
			this.btnBlueFirst.ShowShortcut = false;
			this.btnBlueFirst.Text = "Blue First";
			this.btnBlueFirst.Click += new System.EventHandler(this.btnBlueFirst_OnClick);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.btnOppComp,
																					  this.btnOppHuman});
			this.menuItem6.Text = "Opponent";
			// 
			// btnOppComp
			// 
			this.btnOppComp.Checked = true;
			this.btnOppComp.Index = 0;
			this.btnOppComp.RadioCheck = true;
			this.btnOppComp.Text = "Computer";
			this.btnOppComp.Click += new System.EventHandler(this.btnOppComp_OnCLick);
			// 
			// btnOppHuman
			// 
			this.btnOppHuman.Index = 1;
			this.btnOppHuman.RadioCheck = true;
			this.btnOppHuman.Text = "Human";
			this.btnOppHuman.Click += new System.EventHandler(this.btnOppHuman_OnClick);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 3;
			this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuDifficultyEasy,
																					  this.menuDifficultyHard,
																					  this.menuDifficultyImpossible});
			this.menuItem8.Text = "Difficulty";
			// 
			// menuDifficultyEasy
			// 
			this.menuDifficultyEasy.Index = 0;
			this.menuDifficultyEasy.RadioCheck = true;
			this.menuDifficultyEasy.Text = "Easy (For beginners, stupid turns)";
			this.menuDifficultyEasy.Click += new System.EventHandler(this.menuDifficultyEasy_Click);
			// 
			// menuDifficultyHard
			// 
			this.menuDifficultyHard.Checked = true;
			this.menuDifficultyHard.Index = 1;
			this.menuDifficultyHard.RadioCheck = true;
			this.menuDifficultyHard.Text = "Hard (Advanced path finding)";
			this.menuDifficultyHard.Click += new System.EventHandler(this.menuDifficultyHard_Click);
			// 
			// menuDifficultyImpossible
			// 
			this.menuDifficultyImpossible.Index = 2;
			this.menuDifficultyImpossible.RadioCheck = true;
			this.menuDifficultyImpossible.Text = "Impossible (Advanced defence tactics, slow)";
			this.menuDifficultyImpossible.Click += new System.EventHandler(this.menuDifficultyImpossible_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 5;
			this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.btnStrategy});
			this.menuItem7.Text = "Tools";
			// 
			// btnStrategy
			// 
			this.btnStrategy.Index = 0;
			this.btnStrategy.Text = "Strategy Tool";
			this.btnStrategy.Click += new System.EventHandler(this.btnStrategy_OnClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.InitialDirectory = "strateg";
			this.openFileDialog.Title = "Choose file to use as your strategy:";
			// 
			// StatusTimer
			// 
			this.StatusTimer.Enabled = true;
			this.StatusTimer.Tick += new System.EventHandler(this.StatusTimer_Tick);
			// 
			// TurnProgress
			// 
			this.TurnProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.TurnProgress.Location = new System.Drawing.Point(0, 258);
			this.TurnProgress.Name = "TurnProgress";
			this.TurnProgress.Size = new System.Drawing.Size(492, 8);
			this.TurnProgress.TabIndex = 0;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Title = "Save recorded strategy as...";
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 6;
			this.menuItem9.Text = "About";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(492, 266);
			this.Controls.Add(this.TurnProgress);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mnuMain;
			this.MinimumSize = new System.Drawing.Size(500, 300);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Битвы Клопов";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDown);
			this.Resize += new System.EventHandler(this.frmMain_OnResize);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnKeyUp);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}
		#endregion

		//======================================
		//== VARS
		#region Vars
		public ClopWar Clp;
		public WinClopDraw ClpDraw;
		public ClopCPU ClpCPU;
		public System.Array StratFiles;
		public Random RndGen;
		public string OwnStrat;
		public string CPUStrat;
		public bool OpponentHuman;

		frmTool formTool = null;
		bool formToolLoaded = false;

		private System.Windows.Forms.MainMenu mnuMain;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem btnRedFirst;
		private System.Windows.Forms.MenuItem btnBlueFirst;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem btnOppComp;
		private System.Windows.Forms.MenuItem btnOppHuman;
		public bool Debug_quit = false;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem btnStrategy;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuDifficultyHard;
		private System.Windows.Forms.MenuItem menuDifficultyImpossible;
		private System.Windows.Forms.MenuItem menuDifficultyEasy;
		private System.Windows.Forms.MenuItem menuMakeAutoTurn;
		private System.Windows.Forms.MenuItem menuLoadStrategy;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Timer StatusTimer;
		private System.Windows.Forms.ProgressBar TurnProgress;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem menuItem9;
		public bool loaded = false;
		#endregion

		private void frmMain_Load(object sender, System.EventArgs e)
		{

			//Show
			this.Show();

			//Enable double buffering to avoid flickering
			this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
			this.UpdateStyles();

			ClpDraw=new WinClopDraw(this);
			Clp = new ClopWar(ClpDraw);
			ClpCPU = new ClopCPU(Clp);

            LoadStrat();

			loaded=true;

			//Max difficulty
			menuDifficultyImpossible_Click(null,null);
			//Start!
			Clp.RedFirst=true;
			OpponentHuman=false;
			Clp.ResetGame();
			CheckGame();
		}

		private void LoadStrat()
		{
			//Get strategies filelist
			DirectoryInfo dir = new DirectoryInfo(Application.StartupPath+"\\strateg");
			StratFiles = (Array)dir.GetFiles("*."+Clp.RecFileExt).Clone();
			RndGen=new Random(DateTime.Now.Second);
			if (StratFiles.GetLength(0)>0)
			{
				OwnStrat=null;//((FileInfo)(StratFiles.GetValue(RndGen.Next(StratFiles.GetUpperBound(0)+1)))).FullName;
				CPUStrat=((FileInfo)(StratFiles.GetValue(RndGen.Next(StratFiles.GetUpperBound(0)+1)))).FullName;
				ClpCPU.StrategyFile=CPUStrat;
			}
		}

		private void CheckGameOver()
		{
			switch (Clp.gameStatus)
			{
				case ClopWar.GAME_BLUEWON:
					MessageBox.Show("BLUE won!");
					if (StratFiles.GetLength(0)>0)
					{
						LoadStrat();
					}
					Debug_quit=true;
					Clp.ResetGame();
					break;
				case ClopWar.GAME_REDWON:
					MessageBox.Show("RED won!");
					if (StratFiles.GetLength(0)>0)
					{
						LoadStrat();
					}
					Clp.ResetGame();
					Debug_quit=true;
					break;
			}
		}

		private void CheckGame()
		{
			CheckGameOver();
			if ( (Clp.turn==Cell.BLUE)&(OpponentHuman==false) ) 
			{
				//Make CPU Turn
				
				//Disable input
				Enabled=false;
				
                //Think
                DoThreadTurn();
				
                //Enable controls
				Enabled=true;
				//MessageBox.Show(this,"Your turn!","Info",MessageBoxButtons.OK);
			}
			CheckGameOver();
		}


		#region Events
		
		protected override void OnPaint(PaintEventArgs e)
		{
			ClpDraw.DrawField(e.Graphics);
		}

		private void Mouse_Up(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		private void OnKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//MessageBox.Show(e.KeyCode.ToString());		
		}

		private void OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			ClpDraw.ParseKey(e);		
			CheckGame();
			if (e.Shift) 
				Clp.MakeMove(ClpDraw.Cx,ClpDraw.Cy);
			CheckGame();
		}

		private void Mouse_Down(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button==MouseButtons.Left) ClpDraw.ParseClick(e.X,e.Y);
			if (e.Button==MouseButtons.Right) Clp.UndoTurn();
			CheckGame();		
		}

		private void btnNewGame_Click(object sender, System.EventArgs e)
		{
			if (StratFiles.GetLength(0)>0)
			{
				LoadStrat();
			}
			Clp.ResetGame();
			CheckGame();
		}

		private void btnRecord_Click(object sender, System.EventArgs e)
		{
			if (Clp.gameStatus!=ClopWar.GAME_RECORDING)
			{
				menuItem2.Text="Stop Recording";
				Clp.StartRecording();
			}
			else
			{
				menuItem2.Text="Record";
				saveFileDialog.Filter="Strategy files|*."+Clp.RecFileExt;
				if (saveFileDialog.ShowDialog(this)==DialogResult.OK)
				{
					Clp.StopRecording(saveFileDialog.FileName);
				}				
			}
		}

		private void AutoTurn_OnClick(object sender, System.EventArgs e)
		{
			if ((Clp.turn==Cell.RED)&(OpponentHuman==false))
			{
				ClpCPU.StrategyFile=OwnStrat;			
				bool ap=ClpCPU.AdvancedPath;
				bool ad=ClpCPU.AdvancedDefence;
				ClpCPU.AdvancedDefence=false;
				ClpCPU.AdvancedPath=false;

			    ClpCPU.Turn();

				ClpCPU.AdvancedDefence=ad;
				ClpCPU.AdvancedPath=ap;
				ClpCPU.StrategyFile=CPUStrat;
				CheckGame();		
			}
		}

        private void DoThreadTurn()
        {
            Thread t = new Thread(ClpCPU.Turn);
            t.Start();
        }

		private void btnUndo_Click(object sender, System.EventArgs e)
		{
			Clp.UndoTurn();
		}

		private void btnRedFirst_OnClick(object sender, System.EventArgs e)
		{
			btnRedFirst.Checked=true;
			btnBlueFirst.Checked=false;
			Clp.RedFirst=true;
		}

		private void btnBlueFirst_OnClick(object sender, System.EventArgs e)
		{
			btnRedFirst.Checked=false;
			btnBlueFirst.Checked=true;
			Clp.RedFirst=false;
		}

		private void btnOppHuman_OnClick(object sender, System.EventArgs e)
		{
			OpponentHuman=true;
			btnOppHuman.Checked=true;
			btnOppComp.Checked=false;
		}

		private void btnOppComp_OnCLick(object sender, System.EventArgs e)
		{
			OpponentHuman=false;		
			btnOppHuman.Checked=false;
			btnOppComp.Checked=true;
		}

		private void frmMain_OnResize(object sender, System.EventArgs e)
		{
			if (loaded)
			{
				//Resize Field
				ClpDraw.CellH=(int)(frmMain.ActiveForm.Height-ClpDraw.OffY-ClpDraw.OffY2)/ClopWar.FieldH;
				ClpDraw.CellW=(int)(frmMain.ActiveForm.Width-ClpDraw.OffX-ClpDraw.OffX2)/ClopWar.FieldW;
				if (ClpDraw.CellH<ClpDraw.CellW)
					ClpDraw.CellW=ClpDraw.CellH;
				else
					ClpDraw.CellH=ClpDraw.CellW;

				//ClpDraw.Refresh();
			}
		}

		private void btnStrategy_OnClick(object sender, System.EventArgs e)
		{
			if (!formToolLoaded) formTool=new frmTool();			
			if (formTool.IsDisposed) formTool=new frmTool();			
			formTool.Show();
			formToolLoaded=true;
		}

		private void menuDifficultyEasy_Click(object sender, System.EventArgs e)
		{
			menuDifficultyImpossible.Checked=false;
			menuDifficultyHard.Checked=false;
			menuDifficultyEasy.Checked=true;		
			ClpCPU.AdvancedDefence=false;
			ClpCPU.AdvancedPath=false;
		}

		private void menuDifficultyHard_Click(object sender, System.EventArgs e)
		{
			menuDifficultyImpossible.Checked=false;
			menuDifficultyHard.Checked=true;
			menuDifficultyEasy.Checked=false;				
			ClpCPU.AdvancedDefence=false;
			ClpCPU.AdvancedPath=true;
		}

		private void menuDifficultyImpossible_Click(object sender, System.EventArgs e)
		{
			menuDifficultyImpossible.Checked=true;
			menuDifficultyHard.Checked=false;
			menuDifficultyEasy.Checked=false;
			ClpCPU.AdvancedDefence=true;
			ClpCPU.AdvancedPath=true;
		}

		private void menuLoadStrategy_OnClick(object sender, System.EventArgs e)
		{
			openFileDialog.Filter="Strategy files|*."+Clp.RecFileExt;
			if (openFileDialog.ShowDialog(this)==DialogResult.OK)
			{
				OwnStrat=openFileDialog.FileName;
			}
		}

		private void StatusTimer_Tick(object sender, System.EventArgs e)
		{
            TurnProgress.Maximum=Clp.ClopNum;
			TurnProgress.Minimum=0;
			TurnProgress.Value=Math.Min(Math.Abs(Clp.ClopNum-Clp.clopLeft),Clp.ClopNum);		
		}

		#endregion

		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this,"nAXAHsoft Klopodavka"+(char)13+"http://www.klopodavka.nm.ru"+(char)13+"nAXAH@nm.ru","About",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}

		//[DllImport("coredll.dll")]public static extern bool sndPlaySound(String lpszSoundname, uint fuSound);
		//[DllImport("coredll.dll")]public static extern bool PlaySound(string pszSound, uint hmod, uint fdwSound);
	}
}
