using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Beehive
{
	public partial class MainMap : BaseMap<MapTile>
	{
		public FlowMap[] flows;
		public List<Loc> pents;

		public MainMap(int xIn, int yIn)
		{
			xLen = xIn;
			yLen = yIn;

			tiles = new MapTile[xLen, yLen];

			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y] = new MapTile(new Loc(x, y), this);
				}
			}

			// holding pens
			pents = new List<Loc>();

			// init all flows stuff here
			var flowsCount = Refs.h.roster.Count + 1; // 0 is for master, eventually
			flows = new FlowMap[flowsCount];
			for (int fLoop = 0; fLoop < flowsCount; fLoop++)
			{
				flows[fLoop] = new FlowMap(xIn, yIn, fLoop);
			}

			LoadBitmapFonts();
		}

		// list of all tiles, never changes.
		public MapTileSet cachedTileList;

		public MapTileSet TileList()
		{
			if (cachedTileList == null)
			{
				cachedTileList = new MapTileSet();
				foreach (MapTile t in tiles) { cachedTileList.Add(t); }
			}
			return cachedTileList;
		}

		public bool ClearLoc(Loc p) => TileByLoc(p).clear == true;

		public bool EdgeLoc(Loc p)
		{
			return (p.X == 0 || p.X == xLen - 1 ||
				p.Y == 0 || p.Y == yLen - 1) ? true : false;
		}

		public bool IsSolid(MapTile t) => (t == null || t.clear == false);

		public bool Touching(Mobile m1, Mobile m2)
		{
			double c = Loc.Distance(m1.loc, m2.loc);
			Console.WriteLine(c);
			return (c < 1.01);
		}

		public MapTileSet GetClearTilesListNormal() => TileList().Where(t => t.clear).ToMapTileSet();

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
			MapTileSet splurtArea = here.GetPossibleMoves(Dir.AllAround);
			foreach (MapTile t in splurtArea)
			{
				if (t.clear)
				{
					t.hasNectar = true;
					t.nectarCol = myColor;
				}
			}
		}
	}
}