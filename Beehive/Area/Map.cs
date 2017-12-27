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
	public partial class Map
	{
		private int xLen;
		private int yLen;
		private Tile[,] tiles;

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

			LoadBitmapFonts();
		}

		public int GetXLen()
		{
			return xLen;
		}

		public int GetYLen()
		{
			return yLen;
		}

		// list of all tiles, never changes.
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

		public Tile TileByLoc(Point p)
		{
			return tiles[p.X, p.Y];
		}

		public bool ValidLoc(Point p)
		{
			return (p.X >= 0 && p.X < xLen && p.Y >= 0 && p.Y < yLen) ? true : false;
		}

		public bool ClearLoc(Point p)
		{
			return TileByLoc(p).clear == true;
		}

		public bool EdgeLoc(Point p)
		{
			return (p.X == 0 || p.X == xLen - 1 ||
				p.Y == 0 || p.Y == yLen - 1) ? true : false;
		}

		public bool IsSolid(Tile t)
		{
			return (t == null || t.clear == false);
		}

		public bool Touching(Player p, Cubi s)
		{
			double a = Math.Pow(p.loc.X - s.loc.X, 2);
			double b = Math.Pow(p.loc.Y - s.loc.Y, 2);
			double c = Math.Sqrt(a + b);
			Console.WriteLine(c);
			return (c < 1.01);
		}

		public List<Tile> GetClearTilesListNormal()
		{
			return TileList().Where(t => t.clear).ToList();
		}

		public void HealWalls()
		{
			foreach (Tile t in tiles)
			{
				var n = IsSolid(t.OneNorth());
				var s = IsSolid(t.OneSouth());
				var e = IsSolid(t.OneEast());
				var w = IsSolid(t.OneWest());

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

		private Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}
	}
}