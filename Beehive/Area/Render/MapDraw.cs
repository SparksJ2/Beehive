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
		///
		// todo de-duplicate

		public Size stdSize = new Size(12, 15);
		public Size tripSize = new Size(12 * 3, 15 * 3);

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
			foreach (MapTile t in tiles) { TileDraw.AddBackgroundStuff(bmp, t); }

			// add walls, nectar
			foreach (MapTile t in tiles) { TileDraw.AddForegroundStuff(bmp, t); }

			// add specials
			TileDraw.AddCharSpecial(bmp, "⛤");

			// add mobiles, player and harem
			TileDraw.AddCharMobile(bmp, Refs.p);
			foreach (Cubi c in Refs.h.roster) { TileDraw.AddCharMobile(bmp, c); }

			return bmp;
		}

		// point update for animations
		public void CubiSingleTileUpdate(Cubi c)
		{
			Image img = Refs.mf.MainBitmap.Image;
			TileDraw.AddCharMobile(img, c);
		}

		// point update for animations
		internal void ResetTile(Loc loc)
		{
			Image img = Refs.mf.MainBitmap.Image;
			SetBlank(img, Refs.m.TileByLoc(loc));
			TileDraw.AddForegroundStuff(img, Refs.m.TileByLoc(loc));
		}

		// point update for animations
		public void SetBlank(Image img, MapTile t)
		{
			int x1 = (t.loc.X * FrameData.multX) + FrameData.edgeX;
			int y1 = (t.loc.Y * FrameData.multY) + FrameData.edgeY;
			Color useCol = Color.DarkSlateBlue;

			using (var gFlow = Graphics.FromImage(img))
			{
				// Create a rectangle for the working area on the map
				RectangleF tileRect = new RectangleF(x1, y1, FrameData.multX, FrameData.multY);
				using (var flowBrush = new SolidBrush(useCol))
				{
					gFlow.FillRectangle(flowBrush, tileRect);
				}
			}
		}
	}
}