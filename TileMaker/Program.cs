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
			int limit = 0x3200; // 0x10ffff
			int size = 16;
			int charsPerLine = 64;

			string fontString = "Symbola";
			int em = 11;
			MakeTileFile(limit, size, charsPerLine, fontString, em);

			fontString = "Microsoft Sans Serif";
			em = 11;
			MakeTileFile(limit, size, charsPerLine, fontString, em);
		}

		private static void MakeTileFile(int limit, int size, int charsPerLine, string fontString, int em)
		{
			Font useFont = new Font(fontString, em);
			if (useFont.Name != fontString)
			{
				Console.WriteLine("tried " + fontString + ", got " + useFont.Name);
				Console.ReadLine();
				return;
			}

			string spacelessName = useFont.Name.Replace(" ", "");
			string filename = spacelessName + "-" + em + "pt-" + size + "px";
			Bitmap bm = CreateFontMap(limit, size, charsPerLine, useFont);
			bm.Save("..\\..\\..\\Beehive\\Resources\\" + filename + ".png"); // type based off extension
		}

		private static Bitmap CreateFontMap(int limit, int size, int charsPerLine, Font style)
		{
			Bitmap bm = new Bitmap(size * charsPerLine, limit * size / charsPerLine);

			for (int i = 0; i < limit; i++)
			{
				int xLoc = i * size;
				int yLoc = 0;

				// overflow onto new lines
				while (xLoc >= size * charsPerLine) { yLoc += size; xLoc -= size * charsPerLine; }

				Rectangle rect = new Rectangle(xLoc, yLoc, size, size);

				// attempt a decent look
				Graphics gChar = Graphics.FromImage(bm);
				gChar.SmoothingMode = SmoothingMode.HighQuality;
				gChar.InterpolationMode = InterpolationMode.HighQualityBicubic;
				gChar.PixelOffsetMode = PixelOffsetMode.HighQuality;
				gChar.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

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