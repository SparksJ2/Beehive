using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beehive
{
	public partial class Map
	{
		private Bitmap SansSerifBitmapFont;
		private Bitmap SymbolaBitmapFont;

		private void LoadBitmapFonts()
		{
			SansSerifBitmapFont = new Bitmap(Properties.Resources.MicrosoftSansSerif_11pt_12x15px);
			SymbolaBitmapFont = new Bitmap(Properties.Resources.Symbola_11pt_12x15px);
		}

		public Bitmap AsBitmap()
		{
			Bitmap bmp = new Bitmap((int)(800), (int)(400));
			Graphics gr = Graphics.FromImage(bmp);

			// clear the canvas to dark
			Rectangle pgRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			SolidBrush solidBlack = new SolidBrush(Color.DarkSlateBlue);
			gr.FillRectangle(solidBlack, pgRect);

			// frame the image with a black border
			Rectangle rc = new Rectangle(5, 5, bmp.Width - 10, bmp.Height - 10);
			gr.DrawRectangle(new Pen(Color.White, 4), rc);

			// add floor stuff
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					var t = tiles[x, y];
					AddCharTile(bmp, x, y, t.gly.ToString(), t.flow);
				}
			}

			AddCharTile(bmp, Refs.p.loc.X, Refs.p.loc.Y, "♂", 0);
			AddCharTile(bmp, Refs.c.loc.X, Refs.c.loc.Y, "☿", 0);

			return bmp;
		}

		public void AddCharTile(Bitmap bmp, int x, int y, string s, int flow)
		{
			// todo this doesn't play well with 16x16 tiles, fix soon
			int multX = 12;
			int multY = 15;
			int edgeX = 10;
			int edgeY = 12;

			int x1 = (x * multX) + edgeX;
			int y1 = (y * multY) + edgeY;
			int x2 = multX;
			int y2 = multY;

			// Create a rectangle for the working area on the map
			RectangleF tileRect = new RectangleF(x1, y1, x2, y2);

			// set flow as background
			Tile t = tiles[x, y];
			int flowInt = t.flow * 12;
			if (flowInt > 96) flowInt = 96;
			Graphics gFlow = Graphics.FromImage(bmp);
			if (t.clear) gFlow.FillRectangle(new SolidBrush(Color.FromArgb(12, flowInt, 12)), tileRect);
			gFlow.Flush();

			if (!t.clear || s == "♂" || s == "☿") // todo find a better way
			{
				// find our symbol in this tileset
				int codePoint = s[0];
				int codeX = codePoint % 64;
				int codeY = codePoint / 64;

				// we'll cut from this rectangle
				Rectangle cloneRect = new Rectangle(codeX * multX, codeY * multY, multX, multY);

				// because symbola gets nicer planet symbols
				Bitmap useBitmapFont = SansSerifBitmapFont;
				if (s == "♂" || s == "☿") { useBitmapFont = SymbolaBitmapFont; }

				// extract this symbols as a tiny bitmap
				System.Drawing.Imaging.PixelFormat format = useBitmapFont.PixelFormat;
				Bitmap singleTileImage = useBitmapFont.Clone(cloneRect, format);

				// paste symbol onto map
				Graphics gChar = Graphics.FromImage(bmp);
				gChar.DrawImage(singleTileImage, x1, y1);

				// clean up
				gChar.Flush();
				singleTileImage.Dispose();
			}
		}

		public void ConsoleDump()
		{
			Debug.WriteLine("+--------------------+");
			for (int y = 0; y < yLen; y++)
			{
				var rowofchars = "";
				for (int x = 0; x < xLen; x++)
				{
					var t = tiles[x, y];
					if (t.clear)
						rowofchars += " ";
					else
						rowofchars += t.gly;
				}
				Debug.WriteLine("|" + rowofchars + "|");
			}
			Debug.WriteLine("+--------------------+");
		}
	}
}