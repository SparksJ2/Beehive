namespace TileExperiments
{
	partial class GraphicsTest
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.PictureBoxExperimental = new System.Windows.Forms.PictureBox();
			this.CloseButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxExperimental)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Hello World!";
			// 
			// PictureBoxExperimental
			// 
			this.PictureBoxExperimental.Location = new System.Drawing.Point(12, 25);
			this.PictureBoxExperimental.Name = "PictureBoxExperimental";
			this.PictureBoxExperimental.Size = new System.Drawing.Size(600, 250);
			this.PictureBoxExperimental.TabIndex = 2;
			this.PictureBoxExperimental.TabStop = false;
			// 
			// CloseButton
			// 
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Location = new System.Drawing.Point(12, 281);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 29);
			this.CloseButton.TabIndex = 3;
			this.CloseButton.Text = "Close";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// GraphicsTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(627, 319);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.PictureBoxExperimental);
			this.Controls.Add(this.label1);
			this.Name = "GraphicsTest";
			this.Text = "Graphics Test";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.PictureBoxExperimental)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox PictureBoxExperimental;
		private System.Windows.Forms.Button CloseButton;
	}
}

