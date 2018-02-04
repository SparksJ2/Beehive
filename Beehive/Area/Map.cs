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
	public partial class Map : IDisposable
	{
		private int xLen;
		private int yLen;
		private MapTile[,] tiles;
		public Flow[] flows;

		public Map(int xIn, int yIn)
		{
			xLen = xIn;
			yLen = yIn;

			tiles = new MapTile[xLen, yLen];

			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y] = new MapTile { loc = new Loc(x, y) };
				}
			}

			// init all flows stuff here
			var flowsCount = Refs.h.roster.Count + 1; // 0 is for master, eventually
			flows = new Flow[flowsCount];
			for (int fLoop = 0; fLoop < flowsCount; fLoop++)
			{
				flows[fLoop] = new Flow(xIn, yIn, fLoop);
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
		public HashSet<MapTile> cachedTileList;

		public HashSet<MapTile> TileList()
		{
			if (cachedTileList == null)
			{
				cachedTileList = new HashSet<MapTile>(new MapTileComp());
				foreach (MapTile t in tiles) { cachedTileList.Add(t); }
			}
			return cachedTileList;
		}

		public MapTile TileByLoc(Loc p)
		{
			return tiles[p.X, p.Y];
		}

		public bool ValidLoc(Loc p)
		{
			return (p.X >= 0 && p.X < xLen && p.Y >= 0 && p.Y < yLen) ? true : false;
		}

		public bool ClearLoc(Loc p)
		{
			return TileByLoc(p).clear == true;
		}

		public bool EdgeLoc(Loc p)
		{
			return (p.X == 0 || p.X == xLen - 1 ||
				p.Y == 0 || p.Y == yLen - 1) ? true : false;
		}

		public bool IsSolid(MapTile t)
		{
			return (t == null || t.clear == false);
		}

		public bool Touching(Mobile m1, Mobile m2)
		{
			double c = Loc.Distance(m1.loc, m2.loc);
			Console.WriteLine(c);
			return (c < 1.01);
		}

		public HashSet<MapTile> GetClearTilesListNormal()
		{
			return TileList().Where(t => t.clear).ToTileHashSet();
		}

		public Cubi CubiAt(Loc l)
		{
			foreach (Cubi c in Refs.h.roster)
			{
				if (c.loc == l) return c;
			}
			throw new Exception("Looking in the wrong place!");
		}

		public bool ContainsCubi(Loc l)
		{
			foreach (Cubi c in Refs.h.roster)
			{
				if (c.loc == l) return true;
			}
			return false;
		}

		public void HealWalls()
		{
			foreach (MapTile t in tiles)
			{
				var n = IsSolid(t.OneNorth());
				var s = IsSolid(t.OneSouth());
				var e = IsSolid(t.OneEast());
				var w = IsSolid(t.OneWest());

				// ┌─┬┐  ╔═╦╗  ╓─╥╖  ╒═╤╕
				// │ ││  ║ ║║  ║ ║║  │ ││
				// ├─┼┤  ╠═╬╣  ╟─╫╢  ╞═╪╡
				// └─┴┘  ╚═╩╝  ╙─╨╜  ╘═╧╛

				if (n && s && e && w) t.gly = "╬";

				if (!n && s && e && w) t.gly = "╦";
				if (n && !s && e && w) t.gly = "╩";
				if (n && s && !e && w) t.gly = "╣";
				if (n && s && e && !w) t.gly = "╠";

				if (n && s && !e && !w) t.gly = "║";
				if (!n && !s && e && w) t.gly = "═";

				if (!n && s && e && !w) t.gly = "╔";
				if (!n && s && !e && w) t.gly = "╗";
				if (n && !s && !e && w) t.gly = "╝";
				if (n && !s && e && !w) t.gly = "╚";

				if (!n && s && !e && !w) t.gly = "║";
				if (n && !s && !e && !w) t.gly = "║";
				if (!n && !s && e && !w) t.gly = "═";
				if (!n && !s && !e && w) t.gly = "═";

				//if (!n && s && !e && !w) t.gly = "╥";
				//if (n && !s && !e && !w) t.gly = "╨";
				//if (!n && !s && e && !w) t.gly = "╞";
				//if (!n && !s && !e && w) t.gly = "╡";

				//if (!n && !s && !e && !w) t.gly = "╳";
				if (!n && !s && !e && !w) t.gly = "X";
			}
		} // end healwalls

		public static void SplurtNectar(MapTile here, Color myColor)
		{
			HashSet<MapTile> splurtArea = here.GetPossibleMoves(Dir.AllAround);
			foreach (MapTile t in splurtArea)
			{
				if (t.clear)
				{
					t.hasNectar = true;
					t.nectarCol = myColor;
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed resources here
				SansSerifBitmapFont.Dispose();
				SymbolaBitmapFont.Dispose();
				SymbolaBitmapFontMiscSyms.Dispose();
			}
			// free native resources
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}