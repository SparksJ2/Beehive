using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourcesTest
{
	internal static class Program
	{
		/// The main entry point for the application.
		private static void Main()
		{
			int maxCodePoint = 0x3200; // 0x10ffff
			int minCodePoint = 0;
			Size size = new Size(12, 15); // as (width, height)
			int charsPerLine = 64;
			string fontString = "Symbola";
			int em = 11;
			string description = "";
			MakeTileFile(minCodePoint, maxCodePoint, size, charsPerLine, fontString, em, description);

			fontString = "Microsoft Sans Serif";
			em = 11;
			MakeTileFile(minCodePoint, maxCodePoint, size, charsPerLine, fontString, em, description);

			minCodePoint = 0x2600;
			maxCodePoint = 0x26ff; // 0x10ffff
			size = new Size(12*3, 15*3); // as (width, height)
			charsPerLine = 16;
			fontString = "Symbola";
			description = "MiscSyms";
			em = 20;
			MakeTileFile(minCodePoint, maxCodePoint, size, charsPerLine, fontString, em, description);

			//minCodePoint = 0x2500;
			//maxCodePoint = 0x257f;
			//description = "BoxDraw";
			//MakeTileFile(minCodePoint, maxCodePoint, size, charsPerLine, fontString, em, description);
		}

		private static void MakeTileFile(int minCodePoint, int maxCodePoint, Size size, int charsPerLine, string fontString, int em, string description)
		{
			Font useFont = new Font(fontString, em);
			if (useFont.Name != fontString)
			{
				Console.WriteLine("tried " + fontString + ", got " + useFont.Name);
				Console.ReadLine();
				return;
			}

			string spacelessName = useFont.Name.Replace(" ", "");
			string filename = spacelessName + "-" + em + "pt-" + size.Width + "x" + size.Height + "px";
			if (description != "") { filename += "-" + description; }

			Bitmap bm = CreateFontMap(minCodePoint, maxCodePoint, size, charsPerLine, useFont);
			bm.Save("..\\..\\..\\Beehive\\Resources\\" + filename + ".png"); // type based off extension
		}

		private static Bitmap CreateFontMap(int minCodePoint, int maxCodePoint, Size size, int charsPerRow, Font style)
		{
			int bitmapWidth = size.Width * charsPerRow;
			int bitmapHeight = (maxCodePoint - minCodePoint) * size.Height / charsPerRow;
			int lostRows = minCodePoint / charsPerRow;

			Bitmap bm = new Bitmap(bitmapWidth, bitmapHeight);

			for (int i = minCodePoint; i < maxCodePoint + 1; i++)
			{
				int xLoc = (i % charsPerRow) * size.Width;
				int yLoc = ((i / charsPerRow) - lostRows) * size.Height;

				Rectangle rect = new Rectangle(xLoc, yLoc, size.Width, size.Height);

				// attempt a decent look
				Graphics gChar = Graphics.FromImage(bm);
				gChar.SmoothingMode = SmoothingMode.HighQuality;
				gChar.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gChar.PixelOffsetMode = PixelOffsetMode.HighQuality;
				gChar.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

				// try to center the character in the rectangle
				StringFormat stringFormat = new StringFormat
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				};

				string s = " ";
				// forbidden range causes exceptions;
				if (i < 0x00d800 || i > 0x00dfff) { s = char.ConvertFromUtf32(i).ToString(); }

				gChar.DrawString(s, style, Brushes.White, rect, stringFormat);
				gChar.Flush();
			}
			return bm;
		}
	}
}