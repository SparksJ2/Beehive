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
			Bitmap bmp = new Bitmap(350, 250);

			string s = "♂ ☿  ├─┼┤  ╠═╬╣  ╟─╫╢  ╞═╪╡";
			Font useFont = new Font("Symbola", 11);
			Rectangle rect = new Rectangle(0, 0, 325, 30);
			Brush br = Brushes.White;
			Brush bg = Brushes.Crimson;
			PrintLine(bmp, s, useFont, rect, br, bg);

			rect = new Rectangle(0, 30, 325, 30);
			br = Brushes.DarkOliveGreen;
			bg = Brushes.BlueViolet;
			PrintLine(bmp, s, useFont, rect, br, bg);

			rect = new Rectangle(0, 60, 325, 30);
			br = Brushes.Black;
			bg = Brushes.DarkOrange;
			PrintLine(bmp, s, useFont, rect, br, bg);

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