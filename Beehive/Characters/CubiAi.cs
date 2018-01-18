using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public static class CubiAi
	{
		public static HashSet<Tile> SimpleFlee(int distance)
		{
			HashSet<Tile> heads = new HashSet<Tile>(new TileComp());
			heads.UnionWith(SetUpInitialRing(distance));
			heads.UnionWith(PlayerNectarTiles());

			return heads;
		}

		private static HashSet<Tile> SetUpInitialRing(int distance)
		{
			// we'll try to flow to a set distance from the player by
			//    making a ring of target squares and working from there
			var allTiles = Refs.m.TileList();
			var ring = new HashSet<Tile>(new TileComp());
			foreach (Tile t in allTiles)
			{
				double c = Loc.Distance(Refs.p.loc, t.loc);
				if (c > distance - 1 && c < distance + 1) { ring.Add(t); }
			}
			return ring;
		}

		private static HashSet<Tile> PlayerNectarTiles()
		{
			// tiles containing player nectar are targets too
			var allTiles = Refs.m.TileList();
			var nectarTiles = new HashSet<Tile>(new TileComp());
			foreach (Tile t in allTiles)
			{
				if (t.hasNectar && t.nectarCol == Refs.p.myColor)
				{
					nectarTiles.Add(t);
				}
			}
			return nectarTiles;
		}
	}
}