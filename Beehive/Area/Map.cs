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
	public class Map
	{
		private int xLen;
		private int yLen;
		private Tile[,] tiles;
		private Bitmap SansSerifBitmapFont;
		private Bitmap SymbolaBitmapFont;

		public int GetXLen()
		{
			return xLen;
		}

		public int GetYLen()
		{
			return yLen;
		}

		public Map(int xIn, int yIn)
		{
			xLen = xIn;
			yLen = yIn;

			tiles = new Tile[xLen, yLen];

			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					this.tiles[x, y] = new Tile();
					tiles[x, y].loc.X = x;
					tiles[x, y].loc.Y = y;
				}
			}

			SansSerifBitmapFont = new Bitmap(Properties.Resources.MicrosoftSansSerif_11pt_12x15px);
			SymbolaBitmapFont = new Bitmap(Properties.Resources.Symbola_11pt_12x15px);
		}

		public List<Tile> cachedTileList;

		public List<Tile> TileList()
		{
			if (cachedTileList == null)
			{
				cachedTileList = new List<Tile>();
				foreach (Tile t in tiles) { cachedTileList.Add(t); }
			}
			return cachedTileList;
		}

		public List<Tile> GetClosed3Sides(List<Tile> input)
		{
			var r = new List<Tile>();
			foreach (Tile t in input)
			{
				int sum = 0;
				if (OneNorth(t) == null || OneNorth(t).clear == false) sum++;
				if (OneSouth(t) == null || OneSouth(t).clear == false) sum++;
				if (OneEast(t) == null || OneEast(t).clear == false) sum++;
				if (OneWest(t) == null || OneWest(t).clear == false) sum++;
				if (sum >= 3) r.Add(t);
			}
			return r;
		}

		public List<Tile> GetClosed5Sides(List<Tile> input)
		{
			var r = new List<Tile>();
			foreach (Tile t in input)
			{
				int sum = 0;
				if (OneNorth(t) == null || OneNorth(t).clear == false) sum++;
				if (OneSouth(t) == null || OneSouth(t).clear == false) sum++;
				if (OneEast(t) == null || OneEast(t).clear == false) sum++;
				if (OneWest(t) == null || OneWest(t).clear == false) sum++;

				if (OneNorthEast(t) == null || OneNorthEast(t).clear == false) sum++;
				if (OneSouthEast(t) == null || OneSouthEast(t).clear == false) sum++;
				if (OneNorthWest(t) == null || OneNorthWest(t).clear == false) sum++;
				if (OneSouthWest(t) == null || OneSouthWest(t).clear == false) sum++;

				if (sum >= 5) r.Add(t);
			}
			return r;
		}

		public List<Tile> AndAreWalls(List<Tile> ts)
		{
			return ts.Where(t => !t.clear).ToList();
		}

		public Tile OneNorthEast(Tile t)
		{
			var loc = AddPts(t.loc, Dir.NorthEast);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneSouthEast(Tile t)
		{
			var loc = AddPts(t.loc, Dir.SouthEast);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneNorthWest(Tile t)
		{
			var loc = AddPts(t.loc, Dir.NorthWest);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneSouthWest(Tile t)
		{
			var loc = AddPts(t.loc, Dir.SouthWest);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public List<Tile> GetNextTo(Tile t)
		{
			var x = new List<Tile> { OneNorth(t), OneSouth(t), OneEast(t), OneWest(t) };
			x.RemoveAll(i => i == null);
			// leave border intact
			x.RemoveAll(i => i.loc.X == 0);
			x.RemoveAll(i => i.loc.Y == 0);
			x.RemoveAll(i => i.loc.X == xLen - 1);
			x.RemoveAll(i => i.loc.Y == yLen - 1);
			return x;
		}

		public Tile OneNorth(Tile t)
		{
			var loc = AddPts(t.loc, Dir.North);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneSouth(Tile t)
		{
			var loc = AddPts(t.loc, Dir.South);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneEast(Tile t)
		{
			var loc = AddPts(t.loc, Dir.East);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneWest(Tile t)
		{
			var loc = AddPts(t.loc, Dir.West);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
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
					AddCharTile(bmp, x, y, t.gly.ToString(), t.flow);
				}
			}

			AddCharTile(bmp, Refs.p.loc.X, Refs.p.loc.Y, "♂", 0);
			AddCharTile(bmp, Refs.c.loc.X, Refs.c.loc.Y, "☿", 0);

			return bmp;
		}

		public void AddCharTile(Bitmap bmp, int x, int y, string s, int flow)
		{
			// todo this doesn't play well with 16x16 tiles, fix soon
			int multX = 12;
			int multY = 15;
			int edgeX = 10;
			int edgeY = 12;

			int x1 = (x * multX) + edgeX;
			int y1 = (y * multY) + edgeY;
			int x2 = multX;
			int y2 = multY;

			// Create a rectangle for the working area on the map
			RectangleF tileRect = new RectangleF(x1, y1, x2, y2);

			// set flow as background
			Tile t = tiles[x, y];
			int flowInt = t.flow * 12;
			if (flowInt > 96) flowInt = 96;
			Graphics gFlow = Graphics.FromImage(bmp);
			if (t.clear) gFlow.FillRectangle(new SolidBrush(Color.FromArgb(12, flowInt, 12)), tileRect);
			gFlow.Flush();

			if (!t.clear || s == "♂" || s == "☿") // todo find a better way
			{
				// find our symbol in this tileset
				int codePoint = s[0];
				int codeX = codePoint % 64;
				int codeY = codePoint / 64;

				// we'll cut from this rectangle
				Rectangle cloneRect = new Rectangle(codeX * multX, codeY * multY, multX, multY);

				// because symbola gets nicer planet symbols
				Bitmap useBitmapFont = SansSerifBitmapFont;
				if (s == "♂" || s == "☿") { useBitmapFont = SymbolaBitmapFont; }

				// extract this symbols as a tiny bitmap
				System.Drawing.Imaging.PixelFormat format = useBitmapFont.PixelFormat;
				Bitmap singleTileImage = useBitmapFont.Clone(cloneRect, format);

				// paste symbol onto map
				Graphics gChar = Graphics.FromImage(bmp);
				gChar.DrawImage(singleTileImage, x1, y1);

				// clean up
				gChar.Flush();
				singleTileImage.Dispose();
			}
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

		public Tile TileByLoc(Point p)
		{
			return tiles[p.X, p.Y];
		}

		public bool ValidLoc(Point p)
		{
			return (p.X >= 0 && p.X < xLen && p.Y >= 0 && p.Y < yLen) ? true : false;
		}

		public bool IsEdge(Point p)
		{
			return (p.X == 0 || p.X == xLen - 1 ||
				p.Y == 0 || p.Y == yLen - 1) ? true : false;
		}

		public bool IsSolid(Tile t)
		{
			return (t == null || t.clear == false);
		}

		public void HealWalls()
		{
			foreach (Tile t in tiles)
			{
				var n = IsSolid(OneNorth(t));
				var s = IsSolid(OneSouth(t));
				var e = IsSolid(OneEast(t));
				var w = IsSolid(OneWest(t));

				// ┌─┬┐  ╔═╦╗  ╓─╥╖  ╒═╤╕
				// │ ││  ║ ║║  ║ ║║  │ ││
				// ├─┼┤  ╠═╬╣  ╟─╫╢  ╞═╪╡
				// └─┴┘  ╚═╩╝  ╙─╨╜  ╘═╧╛

				if (n && s && e && w) t.gly = '╬';

				if (!n && s && e && w) t.gly = '╦';
				if (n && !s && e && w) t.gly = '╩';
				if (n && s && !e && w) t.gly = '╣';
				if (n && s && e && !w) t.gly = '╠';

				if (n && s && !e && !w) t.gly = '║';
				if (!n && !s && e && w) t.gly = '═';

				if (!n && s && e && !w) t.gly = '╔';
				if (!n && s && !e && w) t.gly = '╗';
				if (n && !s && !e && w) t.gly = '╝';
				if (n && !s && e && !w) t.gly = '╚';

				if (!n && s && !e && !w) t.gly = '║';
				if (n && !s && !e && !w) t.gly = '║';
				if (!n && !s && e && !w) t.gly = '═';
				if (!n && !s && !e && w) t.gly = '═';

				//if (!n && s && !e && !w) t.gly = '╥';
				//if (n && !s && !e && !w) t.gly = '╨';
				//if (!n && !s && e && !w) t.gly = '╞';
				//if (!n && !s && !e && w) t.gly = '╡';

				//if (!n && !s && !e && !w) t.gly = '╳';
				if (!n && !s && !e && !w) t.gly = 'X';
			}
		} // end healwalls

		public bool Touching(Player p, Cubi s)
		{
			double a = Math.Pow(p.loc.X - s.loc.X, 2);
			double b = Math.Pow(p.loc.Y - s.loc.Y, 2);
			double c = Math.Sqrt(a + b);
			Console.WriteLine(c);
			return (c < 1.01);
		}

		private Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public List<Tile> GetClearTilesNormal()
		{
			return TileList().Where(t => t.clear).ToList();
		}

		// cached clear tiles list for maze generator. not for general use.
		private List<Tile> clearCache;

		public void AddToClearTileCache(Tile t)
		{
			clearCache.Add(t);
		}

		public void InitClearTilesCache()
		{
			clearCache = TileList().Where(t => t.clear).ToList();
		}

		public List<Tile> GetClearTilesCache()
		{
			return clearCache;
			//return TileList().Where(t => t.clear).ToList();
		}
	}
}

//public static void AddText(Bitmap bmp, int x, int y, string s)
//{
//	// note: doesn't work yet
//	Graphics g = Graphics.FromImage(bmp);
//	Point p = new Point(x*10, y*10);

//	TextRenderer.DrawText(g, s, new Font("Symbola", 10), p, Color.White, Color.Black);
//}

//public static void AddCharTest(Bitmap bmp, int x, int y, string s, int size)
//{
//	RectangleF rectf = new RectangleF(x, y, 700, 35);
//	Graphics txt = Graphics.FromImage(bmp);
//	Graphics back = Graphics.FromImage(bmp);
//	back.FillRectangle(Brushes.Purple, rectf);
//	back.Flush();

//	txt.SmoothingMode = SmoothingMode.HighQuality;
//	txt.InterpolationMode = InterpolationMode.HighQualityBicubic;
//	txt.PixelOffsetMode = PixelOffsetMode.HighQuality;
//	txt.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
//	StringFormat format = new StringFormat()
//	{
//		Alignment = StringAlignment.Near,
//		LineAlignment = StringAlignment.Near
//	};
//	txt.DrawString(s, new Font("Symbola", size), Brushes.White, rectf, format);
//	txt.Flush();

//}

//int y = 20;

//AddCharTest(bmp, 30, y, "┌─┬┐╔═╦╗╓─╥╖╒═╤╕", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "│X││║X║║║X║║│X││", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "├─┼┤╠═╬╣╟─╫╢╞═╪╡", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "└─┴┘╚═╩╝╙─╨╜╘═╧╛", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "XXXXXXXXXXXXXXXX", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "♈♉♊♋♌♍♎♏♐♑♒♓⛎", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "☿♁♂♃♄♅♆♇", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "⚳⚴⚵⚶⚷", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "⚸♥★⛓⛏", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "♂♀⚲⚢⚣⚤⚥⚦⚧⚨⚩⌑", 24); y = y + 40;