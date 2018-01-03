using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
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
			var sw = new Stopwatch();
			sw.Start();

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

			Console.WriteLine("Finished map drawing in " + sw.ElapsedMilliseconds + "ms");
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
				//singleTileImage.Dispose(); // no! we'll re-use this bitmap from the cache
			}
		}

		private Dictionary<string, Bitmap> TileBitmapCache;

		private Bitmap GetTileBitmap(string s)
		{
			if (TileBitmapCache == null)
			{
				TileBitmapCache = new Dictionary<string, Bitmap>(); // todo add comparer
			}

			if (TileBitmapCache.ContainsKey(s))
			{
				return TileBitmapCache[s];
			}
			else
			{
				// find our symbol in this tileset
				int codePoint = s[0];
				int codeX = codePoint % 64;
				int codeY = codePoint / 64;

				// because symbola gets nicer planet symbols
				Bitmap useBitmapFont = SansSerifBitmapFont;
				Color useColour = Color.White;

				if (s == "♂")
				{
					useBitmapFont = SymbolaBitmapFont;
					useColour = Refs.p.myColor;
				}

				if (s == "☿" || s == nectarChar)
				{
					useBitmapFont = SymbolaBitmapFont;
					useColour = Refs.c.myColor;
				}

				// we'll cut from this rectangle
				Rectangle cloneRect = new Rectangle(codeX * multX, codeY * multY, multX, multY);

				// extract this symbols as a tiny bitmap
				System.Drawing.Imaging.PixelFormat format = useBitmapFont.PixelFormat;
				Bitmap singleTileImage = useBitmapFont.Clone(cloneRect, format);

				// change color
				singleTileImage = ColorTint(singleTileImage, useColour);

				// we cache these bitmaps
				TileBitmapCache.Add(s, singleTileImage);
				return singleTileImage;
			}
		}

		public Bitmap ColorTint(Bitmap source, Color col)
		{
			double colBlue = col.B / 256.0;
			double colGreen = col.G / 256.0;
			double colRed = col.R / 256.0;

			BitmapData sourceData = source.LockBits(
				new Rectangle(0, 0, source.Width, source.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

			byte[] buffer = new byte[sourceData.Stride * sourceData.Height];
			Marshal.Copy(sourceData.Scan0, buffer, 0, buffer.Length);
			source.UnlockBits(sourceData);

			double red = 0; double green = 0; double blue = 0;
			for (int k = 0; k + 4 < buffer.Length; k += 4)
			{
				blue = buffer[k + 0] * colBlue;
				green = buffer[k + 1] * colGreen;
				red = buffer[k + 2] * colRed;

				if (blue < 0) { blue = 0; }
				if (green < 0) { green = 0; }
				if (red < 0) { red = 0; }

				buffer[k + 0] = (byte)blue;
				buffer[k + 1] = (byte)green;
				buffer[k + 2] = (byte)red;
			}

			Bitmap result = new Bitmap(source.Width, source.Height);

			BitmapData resultData = result.LockBits(
				new Rectangle(0, 0, result.Width, result.Height),
				ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

			Marshal.Copy(buffer, 0, resultData.Scan0, buffer.Length);
			result.UnlockBits(resultData);
			return result;
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