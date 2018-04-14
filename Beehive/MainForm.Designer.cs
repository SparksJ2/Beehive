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
			((System.ComponentModel.ISupportInitialize)(this.MainBitmap)).BeginInit();
			this.SuspendLayout();
			// 
			// MainBitmap
			// 
			this.MainBitmap.Location = new System.Drawing.Point(12, 12);
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
			this.feedbackBox.Location = new System.Drawing.Point(12, 418);
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
			this.inventoryLabel.Location = new System.Drawing.Point(711, 420);
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
			this.miniInventory.Location = new System.Drawing.Point(714, 437);
			this.miniInventory.Name = "miniInventory";
			this.miniInventory.ReadOnly = true;
			this.miniInventory.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.miniInventory.Size = new System.Drawing.Size(98, 132);
			this.miniInventory.TabIndex = 5;
			this.miniInventory.TabStop = false;
			this.miniInventory.Text = "derp mini inventory not set";
			// 
			// MainForm
			// 
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(828, 581);
			this.Controls.Add(this.miniInventory);
			this.Controls.Add(this.inventoryLabel);
			this.Controls.Add(this.feedbackBox);
			this.Controls.Add(this.MainBitmap);
			this.KeyPreview = true;
			this.Name = "MainForm";
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.MainBitmap)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		public System.Windows.Forms.PictureBox MainBitmap;
		public System.Windows.Forms.RichTextBox feedbackBox;
		public System.Windows.Forms.Label inventoryLabel;
		public System.Windows.Forms.RichTextBox miniInventory;
	}
}

