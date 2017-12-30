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

		private string nectarChar = "・"; // katakana middle dot
										 //private string nectarChar = "•"; // list bullet point

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
					// note we're disposing of 3252 Drawing.Graphics objects per turn,
					//    might want to cache some if it becomes slow.
					AddCharTile(bmp, x, y, t.gly.ToString(), t.flow);
				}
			}

			AddCharTile(bmp, Refs.p.loc.X, Refs.p.loc.Y, "♂", 0);
			AddCharTile(bmp, Refs.c.loc.X, Refs.c.loc.Y, "☿", 0);

			return bmp;
		}

		private int multX = 12;
		private int multY = 15;
		private int edgeX = 10;
		private int edgeY = 12;

		public void AddCharTile(Bitmap bmp, int x, int y, string s, int flow)
		{
			int x1 = (x * multX) + edgeX;
			int y1 = (y * multY) + edgeY;
			int x2 = multX;
			int y2 = multY;
			Tile t = tiles[x, y];

			// Create a rectangle for the working area on the map
			RectangleF tileRect = new RectangleF(x1, y1, x2, y2);

			// set flow as background
			if (t.clear)
			{
				int flowInt = t.flow * 12;
				if (flowInt > 96) flowInt = 96;
				Graphics gFlow = Graphics.FromImage(bmp);
				gFlow.FillRectangle(new SolidBrush(Color.FromArgb(12, flowInt, 12)), tileRect);
				gFlow.Flush();

				// add   nectar drops
				if (t.Cnectar)
				{
					Graphics gNectar = Graphics.FromImage(bmp);
					gNectar.DrawImage(GetTileBitmap(nectarChar), x1, y1);
					gNectar.Flush();
				}
			}
			// end background

			// begin foreground
			if (!t.clear || s == "♂" || s == "☿") // todo find a better way
			{
				Bitmap singleTileImage = GetTileBitmap(s);

				// paste symbol onto map
				Graphics gChar = Graphics.FromImage(bmp);
				gChar.DrawImage(singleTileImage, x1, y1);

				// clean up
				gChar.Flush();
				singleTileImage.Dispose();
			}
		}

		private Bitmap GetTileBitmap(string s)
		{
			// find our symbol in this tileset
			int codePoint = s[0];
			int codeX = codePoint % 64;
			int codeY = codePoint / 64;

			// because symbola gets nicer planet symbols
			Bitmap useBitmapFont = SansSerifBitmapFont;
			if (s == "♂" || s == "☿") { useBitmapFont = SymbolaBitmapFont; }
			if (s == nectarChar) { useBitmapFont = SymbolaBitmapFont; }

			// we'll cut from this rectangle
			Rectangle cloneRect = new Rectangle(codeX * multX, codeY * multY, multX, multY);

			// extract this symbols as a tiny bitmap
			System.Drawing.Imaging.PixelFormat format = useBitmapFont.PixelFormat;
			Bitmap singleTileImage = useBitmapFont.Clone(cloneRect, format);

			// todo we should probably cache these bitmaps
			return singleTileImage;
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