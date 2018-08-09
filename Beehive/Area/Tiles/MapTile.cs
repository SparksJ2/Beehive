using System;
using System.Collections.Generic;
using System.Linq;

namespace Beehive
{
	[Serializable()]
	public class MapTile : BaseTile<MainMap, MapTile>
	{
		public bool clear = false;
		public string gly = "#";
		public bool los = false;

		public int[] nectarLevel; // 0 is for master, cubis start as 1 (same as flows)

		public bool noTunnel = false; // only for maze gen

		public MapTile(Loc p, MainMap f) : base(p, f)
		{
			nectarLevel = new int[1 + Harem.MaxId()];
		}

		// for use with KnightMoves(), DodgeMoves(), LeapMoves()
		public MapTileSet GetPossibleMoves(HashSet<Loc> options)
		{
			var result = new MapTileSet();

			foreach (Loc p in options)
			{
				Loc newloc = Loc.AddPts(loc, p);
				if (Refs.m.ValidLoc(newloc) && Refs.m.ClearLoc(newloc))
				{
					result.Add(Refs.m.TileByLoc(newloc));
				}
			}
			return result;
		}

		public static MapTileSet FilterOutClear(MapTileSet ts)
		{
			return ts.Where(t => !t.clear).ToMapTileSet();
		}

		public static MapTileSet Tunnelable(MapTileSet ts)
		{
			return ts.Where(t => t.noTunnel == false).ToMapTileSet();
		}

		internal int TotalNectar()
		{
			int sumAmt = 0;
			for (int nLoop = 0; nLoop < nectarLevel.Length; nLoop++)
			{
				sumAmt += nectarLevel[nLoop];
			}
			return sumAmt;
		}

		internal bool StackedNectar()
		{
			int clash = 0;
			for (int nLoop = 0; nLoop < nectarLevel.Length; nLoop++)
			{ if (nectarLevel[nLoop] > 0) { clash++; } }

			return (clash >= 2);
		}
	}
}