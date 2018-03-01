using System;
using System.Collections.Generic;

namespace Beehive
{
	public static class CubiAi
	{
		public static void FleeToRing(int distance, FlowMap ourFlow)
		{
			HashSet<FlowTile> heads = new HashSet<FlowTile>(new FlowTileComp());
			heads.UnionWith(SetUpInitialRing(distance, ourFlow));
			heads.UnionWith(PlayerNectarTiles(ourFlow));
			foreach (FlowTile fs in heads) { fs.flow = 0; }
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

		private static void ReportHighAndLow(FlowMap ourFlow, string s)
		{
			double high = ourFlow.GetHighest();
			double low = ourFlow.GetLowest();
			Console.WriteLine(s + ", low =" + low + " , high =" + high);
		}

		// utility stuff follows
		private static HashSet<FlowTile> SetUpInitialRing(int distance, FlowMap f)
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

		private static HashSet<FlowTile> PlayerNectarTiles(FlowMap f)
		{
			// tiles containing player nectar are targets too
			var allTiles = Refs.m.TileList();
			var nectarTiles = new MapTileSet();
			foreach (MapTile t in allTiles)
			{
				if (t.hasNectar && t.nectarCol == Refs.p.myColor)
				{
					nectarTiles.Add(t);
				}
			}
			return ConvertTiles.FlowSquaresFromTileSet(nectarTiles, f);
		}
	}
}