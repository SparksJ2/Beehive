using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace Beehive
{
	public partial class MainMap
	{
		/// turns map data into a bitmap. complicated.

		private string[] nectarChars =
			{" ","⠂", "⡂", "⡡", "⢕", "⢝", "⣝", "⣟", "⣿" };

		private int multX = 12;
		private int multY = 15;
		private int edgeX = 10;
		private int edgeY = 12;

		public bool flipRenderMode = false;

		private Size stdSize = new Size(12, 15);
		private Size tripSize = new Size(12 * 3, 15 * 3);

		public Bitmap AsBitmap()
		{
			Bitmap bmp = new Bitmap((int)(800), (int)(400));
			Graphics gr = Graphics.FromImage(bmp);

			// clear the canvas to dark
			Rectangle pgRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			using (var solidBlack = new SolidBrush(Color.DarkSlateBlue))
			{
				gr.FillRectangle(solidBlack, pgRect);
			}

			// frame the image with a black border
			Rectangle rc = new Rectangle(5, 5, bmp.Width - 10, bmp.Height - 10);
			using (var whitePen = new Pen(Color.White, 4))
			{
				gr.DrawRectangle(whitePen, rc);
			}

			// do all backgrounds here (includes flow, los, glow effects)
			Refs.m.RunLos();
			Refs.m.RunGlows();
			foreach (MapTile t in tiles) { AddBackgroundStuff(bmp, t); }

			// add walls, nectar
			foreach (MapTile t in tiles) { AddForegroundStuff(bmp, t); }

			// add specials and mobiles
			AddCharSpecial(bmp, "⛤");

			AddCharMobile(bmp, Refs.p);
			foreach (Cubi c in Refs.h.roster) { AddCharMobile(bmp, c); }

			return bmp;
		}

		public void AddForegroundStuff(Image img, MapTile t)
		{
			int x1 = (t.loc.X * multX) + edgeX;
			int y1 = (t.loc.Y * multY) + edgeY;

			if (t.clear)  // set flow as background only
			{
				int showFlow = Refs.p.viewFlow;

				// display nectar drops using deepest level
				int deepestLevel = 0;
				int deepestAmt = 0;
				int sumAmt = 0;
				for (int nLoop = 0; nLoop < t.nectarLevel.Length; nLoop++)
				{
					sumAmt += t.nectarLevel[nLoop];
					if (t.nectarLevel[nLoop] > deepestAmt)
					{
						deepestAmt = t.nectarLevel[nLoop];
						deepestLevel = nLoop;
					}
				}
				if (deepestAmt > 0)
				{
					using (var gNectar = Graphics.FromImage(img))
					{
						Color nectarCol;
						if (deepestLevel == 0) { nectarCol = Refs.p.myColor; }
						else { nectarCol = Harem.GetId(deepestLevel).myColor; }

						// Color mixedCol = GetColorMix(t);

						if (sumAmt > 8) { sumAmt = 8; }
						string useNectarChar = nectarChars[sumAmt];
						//if (sumAmt > 1) { useNectarChar = nectarCharLarge; }

						gNectar.DrawImage(
							GetTileBitmap(useNectarChar, stdSize, nectarCol, t.backCol),
							x1, y1);
					}
				}
				// todo bigger blob for more nectar maybe?
			}
			else // it's not marked as clear, so draw the wall
			{
				Bitmap singleTileImage = GetTileBitmap(t.gly, stdSize, Color.White, t.backCol);
				using (var gChar = Graphics.FromImage(img))
				{
					gChar.DrawImage(singleTileImage, x1, y1);
				}
			}
		}

		public void AddBackgroundStuff(Image img, MapTile t)
		{
			int x1 = (t.loc.X * multX) + edgeX;
			int y1 = (t.loc.Y * multY) + edgeY;

			if (t.clear)  // set flow as background only
			{
				int showFlow = Refs.p.viewFlow;

				if (showFlow > 0)
				{
					Color flowCol = Harem.GetId(showFlow).myColor;

					double flowInt = Refs.m.flows[showFlow].TileByLoc(t.loc).flow;

					int r = ByteLimit(Convert.ToInt32(flowCol.R - flowInt * 4));
					int g = ByteLimit(Convert.ToInt32(flowCol.G - flowInt * 4));
					int b = ByteLimit(Convert.ToInt32(flowCol.B - flowInt * 4));

					Color useCol = Color.FromArgb(r, g, b);

					using (var gFlow = Graphics.FromImage(img))
					{
						// Create a rectangle for the working area on the map
						RectangleF tileRect = new RectangleF(x1, y1, multX, multY);
						using (var flowBrush = new SolidBrush(useCol))
						{
							gFlow.FillRectangle(flowBrush, tileRect);
						}
					}
				}
				else // show player los instead
				{
					Color losCol = t.backCol;
					Color hidCol = Color.DarkBlue;

					Color useCol = t.los ? losCol : hidCol;

					using (var gFlow = Graphics.FromImage(img))
					{
						// Create a rectangle for the working area on the map
						RectangleF tileRect = new RectangleF(x1, y1, multX, multY);
						using (var flowBrush = new SolidBrush(useCol))
						{
							gFlow.FillRectangle(flowBrush, tileRect);
						}
					}
				}
			}
		}

		private static Color GetColorMix(MapTile mt)
		{
			// speculative -- not sure if I like nectar mixing
			Int32 mergeR = Refs.p.myColor.R;
			Int32 mergeG = Refs.p.myColor.G;
			Int32 mergeB = Refs.p.myColor.B;
			for (int nLoop = 1; nLoop < mt.nectarLevel.Length - 1; nLoop++) // skip player nectar
			{
				mergeR += Harem.GetId(nLoop).myColor.R;
				mergeG += Harem.GetId(nLoop).myColor.G;
				mergeB += Harem.GetId(nLoop).myColor.B;
			}
			double factor = 1 + Refs.h.roster.Count;
			mergeR = (Int32)(mergeR / factor);
			mergeG = (Int32)(mergeG / factor);
			mergeB = (Int32)(mergeB / factor);
			return Color.FromArgb(0, mergeR, mergeG, mergeB);
		}

		private int ByteLimit(int x)
		{
			x = x < 0 ? 0 : x;
			x = x > 255 ? 255 : x;
			return x;
		}

		public void AddCharSpecial(Image img, string s)
		{
			if (s == "⛤") // set up bed
			{
				using (var gBed = Graphics.FromImage(img))
				{
					Bitmap bedBitmap = GetTileBitmap("⛤", tripSize, Color.Purple, Color.Black);

					foreach (Loc pen in Refs.m.pents)
					{
						int bedx1 = ((pen.X - 1) * multX) + edgeX;
						int bedy1 = ((pen.Y - 1) * multY) + edgeY;
						int bedx2 = multX * 3;
						int bedy2 = multY * 3;
						RectangleF tileBed = new RectangleF(bedx1, bedy1, bedx2, bedy2);
						gBed.DrawImage(bedBitmap, bedx1, bedy1);
					}
				}
			}
		}

		public void AddCharMobile(Image img, Mobile m)
		{
			string s = m.glyph;

			// begin foreground
			if ((s == "♂" || s == "☿") && Refs.m.TileByLoc(m.loc).los)
			{
				Bitmap singleTileImage = GetTileBitmap(s, stdSize, m.myColor, Refs.m.TileByLoc(m.loc).backCol);

				// paste symbol onto map
				using (var gChar = Graphics.FromImage(img))
				{
					int x1 = (m.loc.X * multX) + edgeX;
					int y1 = (m.loc.Y * multY) + edgeY;
					gChar.DrawImage(singleTileImage, x1, y1);
				}
			}
		}

		[Serializable()]
		private struct TileDesc // for TileBitmapCache only
		{
			private string chr;
			private Size sz;
			private Color col;
			private Color bg;

			public TileDesc(string sIn, Size zIn, Color colIn, Color bgIn)
			{
				chr = sIn; sz = zIn; col = colIn; bg = bgIn;
			}
		}

		[NonSerialized()] // don't include dictionary in save file
		private Dictionary<TileDesc, Bitmap> TileBitmapCache;

		public void FlushTileBitmapCache()
		{
			TileBitmapCache = null;
		}

		private Bitmap GetTileBitmap(string chr, Size sz, Color col, Color bg)
		{
			if (TileBitmapCache == null)
			{
				TileBitmapCache = new Dictionary<TileDesc, Bitmap>();
			}

			var key = new TileDesc(chr, sz, col, bg);

			if (!TileBitmapCache.ContainsKey(key))
			{
				// create and cache bitmap
				//if (!flipRenderMode)
				//{
				//	TileBitmapCache.Add(key, CreateTileBitmapFromSpriteSheet(chr, sz, col, bg));
				//}
				//else
				//{
				TileBitmapCache.Add(key, CreateTileBitmapFromNewMethod(chr, sz, col, bg));
				//}
			}

			return TileBitmapCache[key];
		}

		private Bitmap CreateTileBitmapFromNewMethod(string chr, Size sz, Color col, Color bg)
		{
			Bitmap bmp;
			Rectangle rect;
			int pts = 11;

			if (sz == stdSize)
			{
				bmp = new Bitmap(stdSize.Width, stdSize.Height);
				rect = new Rectangle(0, 0, sz.Width, sz.Height);
				pts = 11;
			}
			else if (sz == tripSize)
			{
				bmp = new Bitmap(tripSize.Width, tripSize.Height);
				rect = new Rectangle(0, 0, tripSize.Width, tripSize.Height);
				pts = 28;
			}
			else
			{
				Console.WriteLine("CreateTileBitmapFromNewMethod Unknown sz = " + sz.ToString());
				bmp = new Bitmap(stdSize.Width, stdSize.Height); // default to this
				rect = new Rectangle(0, 0, sz.Width, sz.Height);
			}

			Graphics gChar = Graphics.FromImage(bmp);
			Color Abg = Color.FromArgb(255, bg); // seting bg color affects aliasing later
			gChar.Clear(Abg);
			gChar.Flush();

			gChar = Graphics.FromImage(bmp);
			//gChar.SmoothingMode = SmoothingMode.HighQuality;
			//gChar.InterpolationMode = InterpolationMode.HighQualityBicubic;
			//gChar.PixelOffsetMode = PixelOffsetMode.HighQuality;
			//gChar.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			Font style;
			if ((chr == "♂") || (chr == "☿") || (chr == "⛤") || (nectarChars.Contains(chr)))
			{
				style = new Font("Symbola", pts);
			}
			else
			{
				style = new Font("Microsoft Sans Serif", pts);
			}

			// try to center the character in the rectangle
			StringFormat stringFormat = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};

			Brush brush = new SolidBrush(col);

			gChar.DrawString(chr, style, brush, rect, stringFormat);

			gChar.Flush();

			bmp = TileBgRestoreAlpha(bmp, bg);
			return bmp;
		}

		public Bitmap TileBgRestoreAlpha(Bitmap source, Color bg)
		{
			BitmapData sourceData = source.LockBits(
				new Rectangle(0, 0, source.Width, source.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);

			byte[] buffer = new byte[sourceData.Stride * sourceData.Height];
			Marshal.Copy(sourceData.Scan0, buffer, 0, buffer.Length);
			source.UnlockBits(sourceData);

			byte red = 0; byte green = 0; byte blue = 0;
			for (int k = 0; k + 4 < buffer.Length; k += 4)
			{
				blue = buffer[k + 0];
				green = buffer[k + 1];
				red = buffer[k + 2];

				if ((blue == bg.B) && (red == bg.R) && (green == bg.G))
				{
					buffer[k + 3] = 0; // set Alpha
				}
			}

			Bitmap result = new Bitmap(source.Width, source.Height);

			BitmapData resultData = result.LockBits(
				new Rectangle(0, 0, result.Width, result.Height),
				ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

			Marshal.Copy(buffer, 0, resultData.Scan0, buffer.Length);
			result.UnlockBits(resultData);
			return result;
		}

		//private Bitmap CreateTileBitmapFromSpriteSheet(string chr, Size sz, Color col, Color bg)
		//{
		//	// because symbola gets nicer planet symbols
		//	Bitmap useBitmapFont = SansSerifBitmapFont;
		//	Color useColour = Color.White;

		//	if (chr == "♂")
		//	{
		//		useBitmapFont = SymbolaBitmapFont;
		//		useColour = col;
		//	}
		//	if (nectarChars.Contains(chr))
		//	{
		//		useBitmapFont = SansSerifBitmapFont;
		//		useColour = col;
		//	}

		//	if (chr == "☿" || nectarChars.Contains(chr))
		//	{
		//		useBitmapFont = SymbolaBitmapFont;
		//		useColour = col;
		//	}

		//	int FontCodePointOffset = 0;
		//	if (chr == "⛤")
		//	{
		//		useBitmapFont = SymbolaBitmapFontMiscSyms;
		//		useColour = Color.Purple;
		//		FontCodePointOffset = 0x2600;
		//	}

		//	// find our symbol in this tileset
		//	int codePoint = chr[0] - FontCodePointOffset;
		//	int codeX = codePoint % 64;
		//	int codeY = codePoint / 64;

		//	// we'll cut from this rectangle
		//	Rectangle cloneRect = new Rectangle(
		//		codeX * sz.Width, codeY * sz.Height,
		//		sz.Width - 1, sz.Height - 1);

		//	// extract this symbols as a tiny bitmap, old style
		//	PixelFormat format = useBitmapFont.PixelFormat;
		//	var singleTileImage = useBitmapFont.Clone(cloneRect, format);

		//	//// extract this symbols as a tiny bitmap, new style
		//	//// bit blurry though...
		//	//Bitmap singleTileImage = new Bitmap(z.Width, z.Height);
		//	//using (var g = Graphics.FromImage(singleTileImage))
		//	//{
		//	//	g.InterpolationMode = InterpolationMode.Default;
		//	//	var singleTileRect = new Rectangle(0, 0, z.Width, z.Height);
		//	//	g.DrawImage(useBitmapFont, singleTileRect, cloneRect, GraphicsUnit.Pixel);
		//	//}

		//	// change color
		//	singleTileImage = ColorTint(singleTileImage, useColour);

		//	return singleTileImage;
		//}

		//public Bitmap ColorTint(Bitmap source, Color col)
		//{
		//	double colBlue = col.B / 256.0;
		//	double colGreen = col.G / 256.0;
		//	double colRed = col.R / 256.0;

		//	BitmapData sourceData = source.LockBits(
		//		new Rectangle(0, 0, source.Width, source.Height),
		//		ImageLockMode.ReadOnly,
		//		PixelFormat.Format32bppArgb);

		//	byte[] buffer = new byte[sourceData.Stride * sourceData.Height];
		//	Marshal.Copy(sourceData.Scan0, buffer, 0, buffer.Length);
		//	source.UnlockBits(sourceData);

		//	double red = 0; double green = 0; double blue = 0;
		//	for (int k = 0; k + 4 < buffer.Length; k += 4)
		//	{
		//		blue = buffer[k + 0] * colBlue;
		//		green = buffer[k + 1] * colGreen;
		//		red = buffer[k + 2] * colRed;

		//		if (blue < 0) { blue = 0; }
		//		if (green < 0) { green = 0; }
		//		if (red < 0) { red = 0; }

		//		buffer[k + 0] = (byte)blue;
		//		buffer[k + 1] = (byte)green;
		//		buffer[k + 2] = (byte)red;
		//	}

		//	Bitmap result = new Bitmap(source.Width, source.Height);

		//	BitmapData resultData = result.LockBits(
		//		new Rectangle(0, 0, result.Width, result.Height),
		//		ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

		//	Marshal.Copy(buffer, 0, resultData.Scan0, buffer.Length);
		//	result.UnlockBits(resultData);
		//	return result;
		//}

		public void CubiSingleTileUpdate(Cubi c)
		{
			Image img = Refs.mf.MainBitmap.Image;
			AddCharMobile(img, c);
		}

		internal void ResetTile(Loc loc)
		{
			Image img = Refs.mf.MainBitmap.Image;
			SetBlank(img, Refs.m.TileByLoc(loc));
			AddForegroundStuff(img, Refs.m.TileByLoc(loc));
		}

		public void SetBlank(Image img, MapTile t)
		{
			int x1 = (t.loc.X * multX) + edgeX;
			int y1 = (t.loc.Y * multY) + edgeY;
			Color useCol = Color.DarkSlateBlue;

			using (var gFlow = Graphics.FromImage(img))
			{
				// Create a rectangle for the working area on the map
				RectangleF tileRect = new RectangleF(x1, y1, multX, multY);
				using (var flowBrush = new SolidBrush(useCol))
				{
					gFlow.FillRectangle(flowBrush, tileRect);
				}
			}
		}
	}
}