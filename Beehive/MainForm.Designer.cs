using System.Drawing;

namespace Beehive
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		public System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		public void InitializeComponent()
		{
			this.MainBitmap = new System.Windows.Forms.PictureBox();
			this.feedbackBox = new System.Windows.Forms.RichTextBox();
			this.inventoryLabel = new System.Windows.Forms.Label();
			this.miniInventory = new System.Windows.Forms.RichTextBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.quickloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.controlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cheatsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearNectarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.topOffEnergynotUsedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aIVisualizerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.playerViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.s1NavigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.s2NavigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.s3NavigationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.s4NagicatioonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.MainBitmap)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainBitmap
			// 
			this.MainBitmap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MainBitmap.Location = new System.Drawing.Point(12, 42);
			this.MainBitmap.Name = "MainBitmap";
			this.MainBitmap.Size = new System.Drawing.Size(800, 400);
			this.MainBitmap.TabIndex = 1;
			this.MainBitmap.TabStop = false;
			// 
			// feedbackBox
			// 
			this.feedbackBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.feedbackBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.feedbackBox.ForeColor = System.Drawing.Color.White;
			this.feedbackBox.Location = new System.Drawing.Point(12, 448);
			this.feedbackBox.Name = "feedbackBox";
			this.feedbackBox.ReadOnly = true;
			this.feedbackBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.feedbackBox.Size = new System.Drawing.Size(693, 151);
			this.feedbackBox.TabIndex = 2;
			this.feedbackBox.TabStop = false;
			this.feedbackBox.Text = "derp feedback box not initialized.";
			// 
			// inventoryLabel
			// 
			this.inventoryLabel.AutoSize = true;
			this.inventoryLabel.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.inventoryLabel.ForeColor = System.Drawing.Color.DarkOrchid;
			this.inventoryLabel.Location = new System.Drawing.Point(711, 448);
			this.inventoryLabel.Name = "inventoryLabel";
			this.inventoryLabel.Size = new System.Drawing.Size(71, 16);
			this.inventoryLabel.TabIndex = 4;
			this.inventoryLabel.Text = "Carrying:";
			this.inventoryLabel.Click += new System.EventHandler(this.inventoryLabel_Click);
			// 
			// miniInventory
			// 
			this.miniInventory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.miniInventory.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.miniInventory.ForeColor = System.Drawing.Color.White;
			this.miniInventory.Location = new System.Drawing.Point(714, 467);
			this.miniInventory.Name = "miniInventory";
			this.miniInventory.ReadOnly = true;
			this.miniInventory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.miniInventory.Size = new System.Drawing.Size(98, 132);
			this.miniInventory.TabIndex = 5;
			this.miniInventory.TabStop = false;
			this.miniInventory.Text = "derp mini inventory not set";
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackgroundImage = global::Beehive.Properties.Resources._032_000_032BG;
			this.menuStrip1.ForeColor = System.Drawing.Color.MediumOrchid;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.cheatsToolStripMenuItem,
            this.aIVisualizerToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(828, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.quickloadToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.saveToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.saveToolStripMenuItem.Text = "Quicksave";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.MenuSaveEvent);
			// 
			// quickloadToolStripMenuItem
			// 
			this.quickloadToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.quickloadToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.quickloadToolStripMenuItem.Name = "quickloadToolStripMenuItem";
			this.quickloadToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.quickloadToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
			this.quickloadToolStripMenuItem.Text = "Quickload";
			this.quickloadToolStripMenuItem.Click += new System.EventHandler(this.MenuLoadEvent);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlsToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// controlsToolStripMenuItem
			// 
			this.controlsToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.controlsToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
			this.controlsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.controlsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.controlsToolStripMenuItem.Text = "Controls";
			this.controlsToolStripMenuItem.Click += new System.EventHandler(this.HelpPopupEvent);
			// 
			// cheatsToolStripMenuItem
			// 
			this.cheatsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearNectarToolStripMenuItem,
            this.topOffEnergynotUsedToolStripMenuItem});
			this.cheatsToolStripMenuItem.Name = "cheatsToolStripMenuItem";
			this.cheatsToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
			this.cheatsToolStripMenuItem.Text = "Cheats";
			// 
			// clearNectarToolStripMenuItem
			// 
			this.clearNectarToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.clearNectarToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.clearNectarToolStripMenuItem.Name = "clearNectarToolStripMenuItem";
			this.clearNectarToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.clearNectarToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
			this.clearNectarToolStripMenuItem.Text = "Clear Nectar";
			this.clearNectarToolStripMenuItem.Click += new System.EventHandler(this.MenuClearNectarEvent);
			// 
			// topOffEnergynotUsedToolStripMenuItem
			// 
			this.topOffEnergynotUsedToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.topOffEnergynotUsedToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.topOffEnergynotUsedToolStripMenuItem.Name = "topOffEnergynotUsedToolStripMenuItem";
			this.topOffEnergynotUsedToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F3;
			this.topOffEnergynotUsedToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
			this.topOffEnergynotUsedToolStripMenuItem.Text = "Top off Energy (not used)";
			this.topOffEnergynotUsedToolStripMenuItem.Click += new System.EventHandler(this.MenuTopOffEnergyEvent);
			// 
			// aIVisualizerToolStripMenuItem
			// 
			this.aIVisualizerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playerViewToolStripMenuItem,
            this.s1NavigationToolStripMenuItem,
            this.s2NavigationToolStripMenuItem,
            this.s3NavigationToolStripMenuItem,
            this.s4NagicatioonToolStripMenuItem});
			this.aIVisualizerToolStripMenuItem.Name = "aIVisualizerToolStripMenuItem";
			this.aIVisualizerToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
			this.aIVisualizerToolStripMenuItem.Text = "AI Visualizer";
			// 
			// playerViewToolStripMenuItem
			// 
			this.playerViewToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.playerViewToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.playerViewToolStripMenuItem.Name = "playerViewToolStripMenuItem";
			this.playerViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.playerViewToolStripMenuItem.Text = "Player View        0";
			this.playerViewToolStripMenuItem.Click += new System.EventHandler(this.MenuVisAIPlayer);
			// 
			// s1NavigationToolStripMenuItem
			// 
			this.s1NavigationToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.s1NavigationToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.s1NavigationToolStripMenuItem.Name = "s1NavigationToolStripMenuItem";
			this.s1NavigationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.s1NavigationToolStripMenuItem.Text = "S1 Navigation   1";
			this.s1NavigationToolStripMenuItem.Click += new System.EventHandler(this.MenuVisAIOne);
			// 
			// s2NavigationToolStripMenuItem
			// 
			this.s2NavigationToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.s2NavigationToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.s2NavigationToolStripMenuItem.Name = "s2NavigationToolStripMenuItem";
			this.s2NavigationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.s2NavigationToolStripMenuItem.Text = "S2 Navigation   2";
			this.s2NavigationToolStripMenuItem.Click += new System.EventHandler(this.MenuVisAITwo);
			// 
			// s3NavigationToolStripMenuItem
			// 
			this.s3NavigationToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.s3NavigationToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.s3NavigationToolStripMenuItem.Name = "s3NavigationToolStripMenuItem";
			this.s3NavigationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.s3NavigationToolStripMenuItem.Text = "S3 Navigation   3";
			this.s3NavigationToolStripMenuItem.Click += new System.EventHandler(this.MenuVisAIThree);
			// 
			// s4NagicatioonToolStripMenuItem
			// 
			this.s4NagicatioonToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.s4NagicatioonToolStripMenuItem.ForeColor = System.Drawing.Color.MediumOrchid;
			this.s4NagicatioonToolStripMenuItem.Name = "s4NagicatioonToolStripMenuItem";
			this.s4NagicatioonToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.s4NagicatioonToolStripMenuItem.Text = "S4 Navigation   4";
			this.s4NagicatioonToolStripMenuItem.Click += new System.EventHandler(this.MenuVisAIFour);
			// 
			// MainForm
			// 
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(0)))), ((int)(((byte)(32)))));
			this.ClientSize = new System.Drawing.Size(828, 613);
			this.Controls.Add(this.miniInventory);
			this.Controls.Add(this.inventoryLabel);
			this.Controls.Add(this.feedbackBox);
			this.Controls.Add(this.MainBitmap);
			this.Controls.Add(this.menuStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Beehive The Game";
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.MainBitmap)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.PictureBox MainBitmap;
		public System.Windows.Forms.RichTextBox feedbackBox;
		public System.Windows.Forms.Label inventoryLabel;
		public System.Windows.Forms.RichTextBox miniInventory;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem quickloadToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem controlsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cheatsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearNectarToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem topOffEnergynotUsedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aIVisualizerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem playerViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem s1NavigationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem s2NavigationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem s3NavigationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem s4NagicatioonToolStripMenuItem;
	}
}

