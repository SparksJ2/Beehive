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
			this.BeginsLabel = new System.Windows.Forms.Label();
			this.MainBitmap = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.MainBitmap)).BeginInit();
			this.SuspendLayout();
			// 
			// BeginsLabel
			// 
			this.BeginsLabel.AutoSize = true;
			this.BeginsLabel.Location = new System.Drawing.Point(12, 9);
			this.BeginsLabel.Name = "BeginsLabel";
			this.BeginsLabel.Size = new System.Drawing.Size(130, 13);
			this.BeginsLabel.TabIndex = 0;
			this.BeginsLabel.Text = "Here is where it all begins.";
			// 
			// MainBitmap
			// 
			this.MainBitmap.Location = new System.Drawing.Point(15, 35);
			this.MainBitmap.Name = "MainBitmap";
			this.MainBitmap.Size = new System.Drawing.Size(800, 400);
			this.MainBitmap.TabIndex = 1;
			this.MainBitmap.TabStop = false;
			// 
			// MainForm
			// 
			this.ClientSize = new System.Drawing.Size(828, 452);
			this.Controls.Add(this.MainBitmap);
			this.Controls.Add(this.BeginsLabel);
			this.KeyPreview = true;
			this.Name = "MainForm";
			((System.ComponentModel.ISupportInitialize)(this.MainBitmap)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		
		public System.Windows.Forms.Label BeginsLabel;
		public System.Windows.Forms.PictureBox MainBitmap;
	}
}

