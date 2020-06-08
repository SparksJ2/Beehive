using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileExperiments
{
	public partial class GraphicsTest : Form
	{
		public GraphicsTest()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Bitmap bmp = new Bitmap(600, 250);

			string s = "♂ ☿  ├─┼┤  ╠═╬╣  ╟─╫╢  ╞═╪╡";
			Font useFont = new Font("Symbola", 20);
			//Font useFont = new Font("Microsoft Sans Serif",20);

			Rectangle rect = new Rectangle(0, 0, 575, 60);
			Brush br = Brushes.White;
			Brush bg = Brushes.Crimson;
			PrintLine(bmp, s, useFont, rect, br, bg);

			rect = new Rectangle(0, 60, 575, 60);
			br = Brushes.LawnGreen;
			bg = Brushes.BlueViolet;
			PrintLine(bmp, s, useFont, rect, br, bg);

			rect = new Rectangle(0, 120, 575, 60);
			br = Brushes.Black;
			bg = Brushes.DarkOrange;
			PrintLine(bmp, s, useFont, rect, br, bg);

			// now try transparent
			// todo generate transparent bitmap

			// todo paste into locations

			PictureBoxExperimental.Image = bmp;
		}

		private void PrintLine(Bitmap bmp, string s, Font useFont, Rectangle rect, Brush br, Brush bg)
		{
			Graphics gChar = Graphics.FromImage(bmp);

			gChar.SmoothingMode = SmoothingMode.HighQuality;
			gChar.InterpolationMode = InterpolationMode.HighQualityBicubic;
			gChar.PixelOffsetMode = PixelOffsetMode.HighQuality;
			gChar.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			StringFormat stringFormat = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};
			gChar.FillRectangle(bg, rect);
			gChar.DrawString(s, useFont, br, rect, stringFormat);
			gChar.Flush();
		}

		private void CloseButton_Click(object sender, EventArgs e)
		{
			this.Hide();
			this.Close();
		}
	}
}