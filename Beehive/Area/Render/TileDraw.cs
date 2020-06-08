using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class TileDraw
	{
		// todo de-duplicate
		private static string[] nectarChars =
		{" ","⠂", "⡂", "⡡", "⢕", "⢝", "⣝", "⣟", "⣿" };

		public Size stdSize = new Size(12, 15);
		public Size tripSize = new Size(12 * 3, 15 * 3);

		public static void AddForegroundStuff(Image img, MapTile t)
		{
			int x1 = (t.loc.X * FrameData.multX) + FrameData.edgeX;
			int y1 = (t.loc.Y * FrameData.multY) + FrameData.edgeY;

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
							SpriteManager.GetSprite(useNectarChar, Refs.m.stdSize, nectarCol, t.backCol),
							x1, y1);
					}
				}
				// todo bigger blob for more nectar maybe?
			}
			else // it's not marked as clear, so draw the wall
			{
				Bitmap singleTileImage = SpriteManager.GetSprite(t.gly, Refs.m.stdSize, Color.White, t.backCol);
				using (var gChar = Graphics.FromImage(img))
				{
					gChar.DrawImage(singleTileImage, x1, y1);
				}
			}
		}

		public static void AddBackgroundStuff(Image img, MapTile t)
		{
			int x1 = (t.loc.X * FrameData.multX) + FrameData.edgeX;
			int y1 = (t.loc.Y * FrameData.multY) + FrameData.edgeY;

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
						RectangleF tileRect = new RectangleF(x1, y1, FrameData.multX, FrameData.multY);
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
						RectangleF tileRect = new RectangleF(x1, y1, FrameData.multX, FrameData.multY);
						using (var flowBrush = new SolidBrush(useCol))
						{
							gFlow.FillRectangle(flowBrush, tileRect);
						}
					}
				}
			}
		}

		private static int ByteLimit(int x)
		{
			x = x < 0 ? 0 : x;
			x = x > 255 ? 255 : x;
			return x;
		}

		public static void AddCharSpecial(Image img, string s)
		{
			if (s == "⛤") // set up bed
			{
				using (var gBed = Graphics.FromImage(img))
				{
					Bitmap bedBitmap = SpriteManager.GetSprite("⛤", Refs.m.tripSize, Color.Purple, Color.Black);

					foreach (Loc pen in Refs.m.pents)
					{
						int bedx1 = ((pen.X - 1) * FrameData.multX) + FrameData.edgeX;
						int bedy1 = ((pen.Y - 1) * FrameData.multY) + FrameData.edgeY;
						int bedx2 = FrameData.multX * 3;
						int bedy2 = FrameData.multY * 3;
						RectangleF tileBed = new RectangleF(bedx1, bedy1, bedx2, bedy2);
						gBed.DrawImage(bedBitmap, bedx1, bedy1);
					}
				}
			}
		}

		public static void AddCharMobile(Image img, Mobile m)
		{
			string s = m.glyph;

			// begin foreground
			if ((s == "♂" || s == "☿") && Refs.m.TileByLoc(m.loc).los)
			{
				Bitmap singleTileImage = SpriteManager.GetSprite(s, Refs.m.stdSize, m.myColor, Refs.m.TileByLoc(m.loc).backCol);

				// paste symbol onto map
				using (var gChar = Graphics.FromImage(img))
				{
					int x1 = (m.loc.X * FrameData.multX) + FrameData.edgeX;
					int y1 = (m.loc.Y * FrameData.multY) + FrameData.edgeY;
					gChar.DrawImage(singleTileImage, x1, y1);
				}
			}
		}
	}
}