using System;
using System.Collections.Generic;
using System.Linq;

namespace Beehive
{
	[Serializable()]
	public partial class MainMap : BaseMap<MapTile>
	{
		/// holds main map data etc

		[NonSerialized()] // don't put flows in savefile
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

			FlowMap.Init(ref flows, xLen, yLen);

			LoadBitmapFonts();
		}

		// list of all tiles, never changes.
		// don't include in save file
		[NonSerialized()]
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

		internal void SpreadNectar()
		{
			// todo : see final nectar spreading goals txt

			foreach (MapTile t in tiles)
			{
				if (t.clear && (t.TotalNectar() > 8 || t.StackedNectar()))
				{
					//Console.WriteLine("Nectar heavy tile (drips>8) detected, spreading...");
					MapTileSet spreadArea = t.GetPossibleMoves(Dir.AllAround);
					spreadArea = spreadArea.Where(x => x.clear).ToMapTileSet();
					spreadArea = spreadArea.Where(x => !x.StackedNectar()).ToMapTileSet();
					spreadArea = spreadArea.Where(x => x.TotalNectar() <= t.TotalNectar()).ToMapTileSet();

					int lightestLevel = -1;
					int lightestAmt = 99;
					for (int nLoop = 0; nLoop < t.nectarLevel.Length; nLoop++)
					{
						if ((t.nectarLevel[nLoop] > 0) && (t.nectarLevel[nLoop] < lightestAmt))
						{
							lightestAmt = t.nectarLevel[nLoop];
							lightestLevel = nLoop;
						}
					}

					// todo : note this will bias spread direction but okay for now
					foreach (MapTile spreadTo in spreadArea)
					{
						if ((spreadTo.nectarLevel[lightestLevel] < t.nectarLevel[lightestLevel]) &&
								(t.nectarLevel[lightestLevel] > 0))
						{
							spreadTo.nectarLevel[lightestLevel]++;
							t.nectarLevel[lightestLevel]--;
						}
					}
				}
			}
		}

		public static void SplurtNectar(MapTile here, int myIndex)
		{
			MapTileSet splurtArea = here.GetPossibleMoves(Dir.AllAround);
			foreach (MapTile t in splurtArea)
			{
				if (t.clear) { t.nectarLevel[myIndex]++; }
			}
		}
	}
}