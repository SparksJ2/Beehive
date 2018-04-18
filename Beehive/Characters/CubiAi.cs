using System;

namespace Beehive
{
	public static class CubiAi
	{
		public static void FleeToRing(int distance, FlowMap ourFlow)
		{
			FlowTileSet heads = new FlowTileSet();
			heads.UnionWith(SetUpInitialRing(distance, ourFlow));
			heads.UnionWith(PlayerNectarTiles(ourFlow));
			foreach (FlowTile fs in heads) { fs.flow = 0; }
			ourFlow.RunFlow(maskWalls: true);
		}

		public static void JailBreak(int distance, FlowMap ourFlow)
		{
			// note perhaps we could integrate this into the larger flow
			//  system after it's working well?
			// todo change to using FlowTileSet here

			// get list of capture tiles
			FlowTileSet jails = new FlowTileSet();
			foreach (Loc l in Refs.m.pents) { jails.Add(ourFlow.TileByLoc(l)); }

			// get list of cubi locations
			FlowTileSet breaker = new FlowTileSet();
			foreach (Cubi c in Refs.h.roster) { breaker.Add(ourFlow.TileByLoc(c.loc)); }

			// IntersectWith to get occupied jails
			jails.IntersectWith(breaker);

			// set jails as heads
			foreach (FlowTile fs in jails) { fs.flow = 0; }

			// just flow to them
			ourFlow.RunFlow(maskWalls: true);
		}

		public static void FlowOutAndBack(int distance, FlowMap ourFlow)
		{
			// ref http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
			ourFlow.SetToNines();
			ourFlow.TileByLoc(Refs.p.loc).flow = 0;

			ourFlow.RunFlow(maskWalls: true);
			ReportHighAndLow(ourFlow, "after first flow");

			ourFlow.Reverse();
			ReportHighAndLow(ourFlow, "after reverse   ");

			ourFlow.MultFactor(1.5);
			ReportHighAndLow(ourFlow, "after mult      ");

			ourFlow.RunFlow(maskWalls: true);
			ReportHighAndLow(ourFlow, "after 2nd flow  ");

			ourFlow.AdjustFactor(-25);
		}

		// utility stuff follows
		private static void ReportHighAndLow(FlowMap ourFlow, string s)
		{
			double high = ourFlow.GetHighest();
			double low = ourFlow.GetLowest();
			Console.WriteLine(s + ", low =" + low + " , high =" + high);
		}

		private static FlowTileSet SetUpInitialRing(int distance, FlowMap f)
		{
			// we'll try to flow to a set distance from the player by
			//    making a ring of target squares and working from there
			var allTiles = Refs.m.TileList();
			var ring = new MapTileSet();
			foreach (MapTile t in allTiles)
			{
				double c = Loc.Distance(Refs.p.loc, t.loc);
				if (c > distance - 1 && c < distance + 1) { ring.Add(t); }
			}

			return ConvertTiles.FlowSquaresFromTileSet(ring, f);
		}

		private static FlowTileSet PlayerNectarTiles(FlowMap f)
		{
			// tiles containing player nectar are targets too
			var allTiles = Refs.m.TileList();
			var nectarTiles = new MapTileSet();
			foreach (MapTile t in allTiles)
			{
				if (t.nectarLevel[0] > 0) // 0 for player
				{
					nectarTiles.Add(t);
				}
			}
			return ConvertTiles.FlowSquaresFromTileSet(nectarTiles, f);
		}
	}
}