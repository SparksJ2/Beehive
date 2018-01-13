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
					tiles[x, y] = new Tile { loc = new Loc(x, y) };
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
		public HashSet<Tile> cachedTileList;

		public HashSet<Tile> TileList()
		{
			if (cachedTileList == null)
			{
				cachedTileList = new HashSet<Tile>(new TileComp());
				foreach (Tile t in tiles) { cachedTileList.Add(t); }
			}
			return cachedTileList;
		}

		public Tile TileByLoc(Loc p)
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

		public bool IsSolid(Tile t)
		{
			return (t == null || t.clear == false);
		}

		public bool Touching(Mobile m1, Mobile m2)
		{
			double c = Loc.Distance(m1.loc, m2.loc);
			Console.WriteLine(c);
			return (c < 1.01);
		}

		public HashSet<Tile> GetClearTilesListNormal()
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

		public static void SplurtNectar(Tile here, Color myColor)
		{
			HashSet<Tile> splurtArea = here.GetPossibleMoves(Dir.AllAround);
			foreach (Tile t in splurtArea)
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