using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using System.IO;

using Clops.Ai;
using clops.drawing.win32;

namespace Clops_
{
	/// <summary>
	/// Summary description for frmTool.
	/// </summary>
	public class frmTool : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmTool()
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
				if(components != null)
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
            this.lblStrat1 = new System.Windows.Forms.Label();
            this.lblStrat2 = new System.Windows.Forms.Label();
            this.lblVersus = new System.Windows.Forms.Label();
            this.btnStrat1 = new System.Windows.Forms.Button();
            this.btnStrat2 = new System.Windows.Forms.Button();
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.btnFight = new System.Windows.Forms.Button();
            this.chkbFast = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblStrat1
            // 
            this.lblStrat1.BackColor = System.Drawing.Color.Transparent;
            this.lblStrat1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblStrat1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblStrat1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStrat1.Location = new System.Drawing.Point(168, 8);
            this.lblStrat1.Name = "lblStrat1";
            this.lblStrat1.Size = new System.Drawing.Size(200, 24);
            this.lblStrat1.TabIndex = 0;
            this.lblStrat1.Text = "Strategy1";
            this.lblStrat1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStrat1.Click += new System.EventHandler(this.lblStrat1_Click);
            // 
            // lblStrat2
            // 
            this.lblStrat2.BackColor = System.Drawing.Color.Transparent;
            this.lblStrat2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblStrat2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblStrat2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStrat2.Location = new System.Drawing.Point(168, 64);
            this.lblStrat2.Name = "lblStrat2";
            this.lblStrat2.Size = new System.Drawing.Size(200, 24);
            this.lblStrat2.TabIndex = 1;
            this.lblStrat2.Text = "Strategy2";
            this.lblStrat2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblStrat2.Click += new System.EventHandler(this.lblStrat2_Click);
            // 
            // lblVersus
            // 
            this.lblVersus.BackColor = System.Drawing.Color.Transparent;
            this.lblVersus.Location = new System.Drawing.Point(168, 40);
            this.lblVersus.Name = "lblVersus";
            this.lblVersus.Size = new System.Drawing.Size(200, 16);
            this.lblVersus.TabIndex = 2;
            this.lblVersus.Text = "VS.";
            this.lblVersus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnStrat1
            // 
            this.btnStrat1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrat1.Location = new System.Drawing.Point(368, 8);
            this.btnStrat1.Name = "btnStrat1";
            this.btnStrat1.Size = new System.Drawing.Size(24, 24);
            this.btnStrat1.TabIndex = 3;
            this.btnStrat1.Text = "...";
            this.btnStrat1.Click += new System.EventHandler(this.btnStrat1_Click);
            // 
            // btnStrat2
            // 
            this.btnStrat2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStrat2.Location = new System.Drawing.Point(368, 64);
            this.btnStrat2.Name = "btnStrat2";
            this.btnStrat2.Size = new System.Drawing.Size(24, 24);
            this.btnStrat2.TabIndex = 4;
            this.btnStrat2.Text = "...";
            this.btnStrat2.Click += new System.EventHandler(this.btnStrat2_Click);
            // 
            // dlgOpen
            // 
            this.dlgOpen.InitialDirectory = "strateg";
            // 
            // btnFight
            // 
            this.btnFight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFight.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFight.Location = new System.Drawing.Point(160, 96);
            this.btnFight.Name = "btnFight";
            this.btnFight.Size = new System.Drawing.Size(232, 24);
            this.btnFight.TabIndex = 5;
            this.btnFight.Text = "Fight!";
            this.btnFight.Click += new System.EventHandler(this.btnFight_Click);
            // 
            // chkbFast
            // 
            this.chkbFast.BackColor = System.Drawing.Color.Transparent;
            this.chkbFast.Location = new System.Drawing.Point(168, 136);
            this.chkbFast.Name = "chkbFast";
            this.chkbFast.Size = new System.Drawing.Size(224, 24);
            this.chkbFast.TabIndex = 6;
            this.chkbFast.Text = "Fast Chellenge (Stupid)";
            this.chkbFast.UseVisualStyleBackColor = false;
            // 
            // frmTool
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 516);
            this.Controls.Add(this.chkbFast);
            this.Controls.Add(this.btnFight);
            this.Controls.Add(this.btnStrat2);
            this.Controls.Add(this.btnStrat1);
            this.Controls.Add(this.lblVersus);
            this.Controls.Add(this.lblStrat2);
            this.Controls.Add(this.lblStrat1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "frmTool";
            this.Text = "Strategy Tool";
            this.Load += new System.EventHandler(this.frmTool_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTool_FormClosed);
            this.Resize += new System.EventHandler(this.OnResize);
            this.ResumeLayout(false);

		}
		#endregion

		public ClopWar Clp;
		public WinClopDraw ClpDraw;
		public ClopCPU ClpCPU;
		public Array StratFiles;
		public Random RndGen;
		public string Strat1;
		private Label lblStrat1;
		private Label lblStrat2;
		private Label lblVersus;
		private Button btnStrat1;
		private Button btnStrat2;
		private OpenFileDialog dlgOpen;
		private Button btnFight;
		private CheckBox chkbFast;
		public string Strat2;
	    private Thread fightThread;

		private void frmTool_Load(object sender, EventArgs e)
		{
			//Icon
			//Icon= new Icon(GetType(),"clop.ico");

			//Show
			Show();

			//Enable double buffering to avoid flickering
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
			UpdateStyles();

			ClpDraw=new WinClopDraw(this);
			Clp = new ClopWar(ClpDraw);
			ClpCPU = new ClopCPU(Clp);

			ClpDraw.showtext   = false;
			ClpDraw.showavail  = false;
			ClpDraw.showcursor = false;
			ClpDraw.dispclopleft=false;
			Clp.AutoRedraw	   = false;

			//LoadStrat();

            fightThread = new Thread(DoFight);

			//Start!
			Clp.RedFirst=true;
		}

		private void LoadStrat()
		{
			//Get strategies filelist
			DirectoryInfo dir = new DirectoryInfo("strateg");
			StratFiles = (Array)dir.GetFiles("*."+Clp.RecFileExt).Clone();
			RndGen=new Random(DateTime.Now.Second);
			if (StratFiles.GetLength(0)>0)
			{
				Strat1=((FileInfo)(StratFiles.GetValue(RndGen.Next(StratFiles.GetUpperBound(0)+1)))).FullName;
				Strat2=((FileInfo)(StratFiles.GetValue(RndGen.Next(StratFiles.GetUpperBound(0)+1)))).FullName;
				ClpCPU.StrategyFile=Strat1;
				lblStrat1.Text=Strat1.Remove(0,Strat1.LastIndexOf("\\")+1);
				lblStrat2.Text=Strat2.Remove(0,Strat2.LastIndexOf("\\")+1);
			}
		}

		private void ResizeField()
		{
			//Controls
			btnStrat1.Left=this.Width-btnStrat1.Width-25;
			lblStrat1.Left=btnStrat1.Left-lblStrat1.Width-5;
			btnStrat2.Left=this.Width-btnStrat2.Width-25;
			lblStrat2.Left=btnStrat2.Left-lblStrat2.Width-5;
			lblVersus.Left=lblStrat1.Left;
			btnFight.Left=lblStrat1.Left;
			chkbFast.Left=lblStrat1.Left;

			//Field
			ClpDraw.CellH = (Height-ClpDraw.OffY-ClpDraw.OffY2)/ClopWar.FieldH;
			ClpDraw.CellW = (Width-ClpDraw.OffX-ClpDraw.OffX2-40)/ClopWar.FieldW;
			ClpDraw.Refresh();
		}

		private void OnPaint(object sender, PaintEventArgs e)
		{
			ClpDraw.DrawField(e.Graphics);
		}

		private void OnResize(object sender, EventArgs e)
		{
			ResizeField();
		}

        private void btnStrat1_Click(object sender, EventArgs e)
        {
            dlgOpen.Filter = "Strategy Files|*." + Clp.RecFileExt;
            if (dlgOpen.ShowDialog(this) == DialogResult.OK)
            {
                Strat1 = dlgOpen.FileName;
                lblStrat1.Text = Strat1.Remove(0, Strat1.LastIndexOf("\\") + 1);
            }
        }

        private void btnStrat2_Click(object sender, EventArgs e)
        {
            dlgOpen.Filter = "Strategy Files|*." + Clp.RecFileExt;
            if (dlgOpen.ShowDialog(this) == DialogResult.OK)
            {
                Strat2 = dlgOpen.FileName;
                lblStrat2.Text = Strat2.Remove(0, Strat2.LastIndexOf("\\") + 1);
            }
        }

        private void btnFight_Click(object sender, EventArgs e)
        {
            if (fightThread.IsAlive)
                fightThread.Abort();

            fightThread = new Thread(DoFight);

            fightThread.Start();
        }

        private void DoFight()
        {
            Clp.ClopNum = ClopWar.StdClopNum;
            Clp.ResetGame();
            Clp.ClopNum = ClopWar.StdClopNum;

            if (chkbFast.Checked)
                ClpCPU.AdvancedDefence = false;
            else
                ClpCPU.AdvancedDefence = true;

            ClpCPU.StrategyFile = Strat2;
            while ((Clp.gameStatus == ClopWar.GAME_BLUE) | (Clp.gameStatus == ClopWar.GAME_RED))
            {
                if (ClpCPU.StrategyFile == Strat1)
                    ClpCPU.StrategyFile = Strat2;
                else
                    ClpCPU.StrategyFile = Strat1;
                ClpCPU.Turn();
                ClpDraw.Refresh();
            }
        }

        private void lblStrat1_Click(object sender, EventArgs e)
        {
            if (Strat1 == null) return;
            Clp.ClopNum = 100;
            Clp.ResetGame();
            Clp.PlayRecord(Strat1, 100);
            ClpDraw.Refresh();
        }

        private void lblStrat2_Click(object sender, EventArgs e)
        {
            if (Strat2 == null) return;
            Clp.ClopNum = 100;
            Clp.ResetGame();
            Clp.PlayRecord(Strat2, 100);
            ClpDraw.Refresh();
        }

        private void frmTool_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (fightThread.IsAlive)
                fightThread.Abort();
        }

	}
}
