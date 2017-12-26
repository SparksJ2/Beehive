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
			this.feedbackBox.Enabled = false;
			this.feedbackBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.feedbackBox.Location = new System.Drawing.Point(12, 418);
			this.feedbackBox.Name = "feedbackBox";
			this.feedbackBox.ReadOnly = true;
			this.feedbackBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.feedbackBox.Size = new System.Drawing.Size(593, 95);
			this.feedbackBox.TabIndex = 2;
			this.feedbackBox.Text = "derp feedback box not initialized.";
			// 
			// inventoryLabel
			// 
			this.inventoryLabel.AutoSize = true;
			this.inventoryLabel.Location = new System.Drawing.Point(611, 418);
			this.inventoryLabel.Name = "inventoryLabel";
			this.inventoryLabel.Size = new System.Drawing.Size(91, 13);
			this.inventoryLabel.TabIndex = 4;
			this.inventoryLabel.Text = "Currently carrying:";
			// 
			// miniInventory
			// 
			this.miniInventory.Enabled = false;
			this.miniInventory.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.miniInventory.Location = new System.Drawing.Point(614, 435);
			this.miniInventory.Name = "miniInventory";
			this.miniInventory.ReadOnly = true;
			this.miniInventory.Size = new System.Drawing.Size(198, 69);
			this.miniInventory.TabIndex = 5;
			this.miniInventory.Text = "derp mini inventory not set";
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(828, 540);
			this.Controls.Add(this.miniInventory);
			this.Controls.Add(this.inventoryLabel);
			this.Controls.Add(this.feedbackBox);
			this.Controls.Add(this.MainBitmap);
			this.KeyPreview = true;
			this.Name = "MainForm";
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

