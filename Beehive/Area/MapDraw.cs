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
		private Bitmap SymbolaBitmapFontMiscSyms;

		private string nectarChar = "・"; // katakana middle dot
										 //private string nectarChar = "•"; // list bullet point

		private int multX = 12;
		private int multY = 15;
		private int edgeX = 10;
		private int edgeY = 12;

		private Size stdSize = new Size(12, 15);
		private Size tripSize = new Size(12 * 3, 15 * 3);

		private void LoadBitmapFonts()
		{
			SansSerifBitmapFont = new Bitmap(Properties.Resources.MicrosoftSansSerif_11pt_12x15px);
			SymbolaBitmapFont = new Bitmap(Properties.Resources.Symbola_11pt_12x15px);
			SymbolaBitmapFontMiscSyms = new Bitmap(Properties.Resources.Symbola_28pt_36x45px_MiscSyms);
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

			// add flow and walls stuff
			foreach (Tile t in tiles) { AddCharFlow(bmp, t); }

			// specials and mobiles
			AddCharSpecial(bmp, "⛤");
			AddCharMobile(bmp, Refs.p, "♂");
			AddCharMobile(bmp, Refs.c, "☿");

			Console.WriteLine("Finished map drawing in " + sw.ElapsedMilliseconds + "ms");
			return bmp;
		}

		// todo could use some de-duplication work here
		public void AddCharFlow(Bitmap bmp, Tile t)
		{
			int x1 = (t.loc.X * multX) + edgeX;
			int y1 = (t.loc.Y * multY) + edgeY;

			if (t.clear) // set flow as background
			{
				int flowInt = t.flow * 12;
				if (flowInt > 96) flowInt = 96;
				using (var gFlow = Graphics.FromImage(bmp))
				{
					// Create a rectangle for the working area on the map
					RectangleF tileRect = new RectangleF(x1, y1, multX, multY);
					gFlow.FillRectangle(new SolidBrush(Color.FromArgb(12, flowInt, 12)), tileRect);
				}

				// add nectar drops
				if (t.Cnectar)
				{
					using (var gNectar = Graphics.FromImage(bmp))
					{
						gNectar.DrawImage(GetTileBitmap(nectarChar, stdSize), x1, y1);
					}
				}
			}
			else // or add walls
			{
				Bitmap singleTileImage = GetTileBitmap(t.gly, stdSize);
				using (var gChar = Graphics.FromImage(bmp))
				{
					gChar.DrawImage(singleTileImage, x1, y1);
				}
			}
		}

		public void AddCharSpecial(Bitmap bmp, string s)
		{
			if (s == "⛤") // set up bed
			{
				using (var gBed = Graphics.FromImage(bmp))
				{
					int bedx1 = (30 * multX) + edgeX;
					int bedy1 = (11 * multY) + edgeY;
					int bedx2 = multX * 3;
					int bedy2 = multY * 3;
					RectangleF tileBed = new RectangleF(bedx1, bedy1, bedx2, bedy2);
					Bitmap bedBitmap = GetTileBitmap("⛤", tripSize);
					gBed.DrawImage(bedBitmap, bedx1, bedy1);
				}
			}
		}

		public void AddCharMobile(Bitmap bmp, Mobile m, string s)
		{
			int x1 = (m.loc.X * multX) + edgeX;
			int y1 = (m.loc.Y * multY) + edgeY;

			// begin foreground
			if (s == "♂" || s == "☿")
			{
				Bitmap singleTileImage = GetTileBitmap(s, stdSize);

				// paste symbol onto map
				using (var gChar = Graphics.FromImage(bmp))
				{
					gChar.DrawImage(singleTileImage, x1, y1);
				}
			}
		}

		public struct TileDesc // for TileBitmapCache only
		{
			private string s; private Size z;

			public TileDesc(string sIn, Size zIn)
			{
				s = sIn; z = zIn;
			}
		}

		private Dictionary<TileDesc, Bitmap> TileBitmapCache;

		private Bitmap GetTileBitmap(string s, Size z)
		{
			if (TileBitmapCache == null)
			{
				TileBitmapCache = new Dictionary<TileDesc, Bitmap>(); // todo add comparer
			}

			var key = new TileDesc(s, z);

			if (TileBitmapCache.ContainsKey(key))
			{
				return TileBitmapCache[key];
			}
			else
			{
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

				int FontCodePointOffset = 0;
				if (s == "⛤")
				{
					useBitmapFont = SymbolaBitmapFontMiscSyms;
					useColour = Color.Purple;
					FontCodePointOffset = 0x2600;
				}

				// find our symbol in this tileset
				int codePoint = s[0] - FontCodePointOffset;
				int codeX = codePoint % 64;
				int codeY = codePoint / 64;

				// we'll cut from this rectangle
				Rectangle cloneRect = new Rectangle(
					codeX * z.Width, codeY * z.Height,
					z.Width - 1, z.Height - 1);

				// extract this symbols as a tiny bitmap, old style
				PixelFormat format = useBitmapFont.PixelFormat;
				var singleTileImage = useBitmapFont.Clone(cloneRect, format);

				//// extract this symbols as a tiny bitmap, new style
				//// bit blurry though...
				//Bitmap singleTileImage = new Bitmap(z.Width, z.Height);
				//using (var g = Graphics.FromImage(singleTileImage))
				//{
				//	g.InterpolationMode = InterpolationMode.Default;
				//	var singleTileRect = new Rectangle(0, 0, z.Width, z.Height);
				//	g.DrawImage(useBitmapFont, singleTileRect, cloneRect, GraphicsUnit.Pixel);
				//}

				// change color
				singleTileImage = ColorTint(singleTileImage, useColour);

				// we cache these bitmaps
				TileBitmapCache.Add(key, singleTileImage);
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