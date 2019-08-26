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
			((System.ComponentModel.ISupportInitialize)(this.MainBitmap)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// MainBitmap
			// 
			this.MainBitmap.Location = new System.Drawing.Point(12, 42);
			this.MainBitmap.Name = "MainBitmap";
			this.MainBitmap.Size = new System.Drawing.Size(800, 400);
			this.MainBitmap.TabIndex = 1;
			this.MainBitmap.TabStop = false;
			// 
			// feedbackBox
			// 
			this.feedbackBox.BackColor = System.Drawing.Color.Black;
			this.feedbackBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this.inventoryLabel.ForeColor = System.Drawing.Color.White;
			this.inventoryLabel.Location = new System.Drawing.Point(711, 450);
			this.inventoryLabel.Name = "inventoryLabel";
			this.inventoryLabel.Size = new System.Drawing.Size(91, 13);
			this.inventoryLabel.TabIndex = 4;
			this.inventoryLabel.Text = "Currently carrying:";
			// 
			// miniInventory
			// 
			this.miniInventory.BackColor = System.Drawing.Color.Black;
			this.miniInventory.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
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
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.saveToolStripMenuItem.Text = "Quicksave";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.MenuSaveEvent);
			// 
			// quickloadToolStripMenuItem
			// 
			this.quickloadToolStripMenuItem.Name = "quickloadToolStripMenuItem";
			this.quickloadToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
			this.quickloadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
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
			this.controlsToolStripMenuItem.Name = "controlsToolStripMenuItem";
			this.controlsToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
			this.controlsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
			this.controlsToolStripMenuItem.Text = "Controls";
			this.controlsToolStripMenuItem.Click += new System.EventHandler(this.HelpPopupEvent);
			// 
			// MainForm
			// 
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(828, 613);
			this.Controls.Add(this.miniInventory);
			this.Controls.Add(this.inventoryLabel);
			this.Controls.Add(this.feedbackBox);
			this.Controls.Add(this.MainBitmap);
			this.Controls.Add(this.menuStrip1);
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
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
	}
}

