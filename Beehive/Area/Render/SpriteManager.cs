using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class SpriteManager
	{
		// todo de-duplicate
		private static string[] nectarChars =
			{" ","⠂", "⡂", "⡡", "⢕", "⢝", "⣝", "⣟", "⣿" };

		public static Size stdSize = new Size(12, 15);
		public static Size tripSize = new Size(12 * 3, 15 * 3);

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
		private static Dictionary<TileDesc, Bitmap> TileBitmapCache;

		public void FlushSpriteCache()
		{
			TileBitmapCache = null;
		}

		public static Bitmap GetSprite(string chr, Size sz, Color col, Color bg)
		{
			if (TileBitmapCache == null)
			{ TileBitmapCache = new Dictionary<TileDesc, Bitmap>(); }

			var key = new TileDesc(chr, sz, col, bg);

			if (!TileBitmapCache.ContainsKey(key))
			{ TileBitmapCache.Add(key, NewSprite(chr, sz, col, bg)); }

			return TileBitmapCache[key];
		}

		private static Bitmap NewSprite(string chr, Size sz, Color col, Color bg)
		{
			Bitmap bmp;
			Rectangle rect;

			// default
			int usePts = 11;
			Font useFont = new Font("Symbola", usePts);

			if (sz == stdSize)
			{
				usePts = 11;

				if ((chr == "♂") || (chr == "☿") || (chr == "⛤") || (nectarChars.Contains(chr)))
				{ useFont = new Font("Symbola", usePts); }
				else
				{ useFont = new Font("Microsoft Sans Serif", usePts); }

				bmp = new Bitmap(stdSize.Width, stdSize.Height);
				rect = new Rectangle(0, 0, sz.Width, sz.Height);
			}
			else if (sz == tripSize)
			{
				bmp = new Bitmap(tripSize.Width, tripSize.Height);
				rect = new Rectangle(0, 0, tripSize.Width, tripSize.Height);
				usePts = 28;
				useFont = new Font("Symbola", usePts);
			}
			else
			{
				Console.WriteLine("NewSprite Unknown sz = " + sz.ToString());
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

			// try to center the character in the rectangle
			StringFormat stringFormat = new StringFormat
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};

			Brush brush = new SolidBrush(col);
			gChar.DrawString(chr, useFont, brush, rect, stringFormat);
			gChar.Flush();

			bmp = SpriteRestoreAlpha(bmp, bg);
			return bmp;
		}

		public static Bitmap SpriteRestoreAlpha(Bitmap source, Color bg)
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
	}
}