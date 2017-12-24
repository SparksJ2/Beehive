﻿using System;
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
			Size size = new Size(12, 15); // as (width, height)
			int charsPerLine = 64;

			string fontString = "Symbola";
			int em = 11;
			MakeTileFile(limit, size, charsPerLine, fontString, em);

			fontString = "Microsoft Sans Serif";
			em = 11;
			MakeTileFile(limit, size, charsPerLine, fontString, em);
		}

		private static void MakeTileFile(int limit, Size size, int charsPerLine, string fontString, int em)
		{
			Font useFont = new Font(fontString, em);
			if (useFont.Name != fontString)
			{
				Console.WriteLine("tried " + fontString + ", got " + useFont.Name);
				Console.ReadLine();
				return;
			}

			string spacelessName = useFont.Name.Replace(" ", "");
			string filename = spacelessName + "-" + em +
				"pt-" + size.Width + "x" + size.Height + "px";

			Bitmap bm = CreateFontMap(limit, size, charsPerLine, useFont);
			bm.Save("..\\..\\..\\Beehive\\Resources\\" + filename + ".png"); // type based off extension
		}

		private static Bitmap CreateFontMap(int limit, Size size, int charsPerRow, Font style)
		{
			Bitmap bm = new Bitmap(size.Width * charsPerRow, limit * size.Height / charsPerRow);

			for (int i = 0; i < limit; i++)
			{
				int xLoc = i * size.Width;
				int yLoc = 0;

				// overflow onto new lines
				while (xLoc >= size.Width * charsPerRow)
				{
					yLoc += size.Height;
					xLoc -= size.Width * charsPerRow;
				}

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