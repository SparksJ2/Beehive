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

		public bool IsEdge(Point p)
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

		// for use with KnightMoves(), DodgeMoves(), LeapMoves()
		public List<Tile> GetPossibleMoves(Tile t, List<Point> options)
		{
			var result = new List<Tile>();

			foreach (Point p in options)
			{
				Point newloc = AddPts(t.loc, p);
				if (ValidLoc(newloc) && ClearLoc(newloc))
				{
					result.Add(TileByLoc(newloc));
				}
			}
			return result;
		}

		private Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public List<Tile> GetClearTilesNormal()
		{
			return TileList().Where(t => t.clear).ToList();
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
	}
}