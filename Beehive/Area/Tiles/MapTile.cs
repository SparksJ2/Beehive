using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Beehive
{
	public class MapTile : BaseTile<MainMap, MapTile>
	{
		public bool clear = false;
		public string gly = "#";

		public bool hasNectar = false;
		public Color nectarCol;

		public bool noTunnel = false; // only for maze gen

		public MapTile(Loc p, MainMap f) : base(p, f)
		{
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
	}
}