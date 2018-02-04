using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public static class CubiAi
	{
		public static void FleeToRing(int distance, Flow ourFlow)
		{
			HashSet<FlowTile> heads = new HashSet<FlowTile>(new FlowTileComp());
			heads.UnionWith(SetUpInitialRing(distance, ourFlow));
			heads.UnionWith(PlayerNectarTiles(ourFlow));
			foreach (FlowTile fs in heads) { fs.flow = 0; }
			ourFlow.RunFlow(maskWalls: true);
		}

		public static void FlowOutAndBack(int distance, Flow ourFlow)
		{
			// ref http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
			ourFlow.SetToNines();
			ourFlow.FlowSquareByLoc(Refs.p.loc).flow = 0;

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

		private static void ReportHighAndLow(Flow ourFlow, string s)
		{
			double high = ourFlow.GetHighest();
			double low = ourFlow.GetLowest();
			Console.WriteLine(s + ", low =" + low + " , high =" + high);
		}

		// utility stuff follows
		private static HashSet<FlowTile> SetUpInitialRing(int distance, Flow f)
		{
			// we'll try to flow to a set distance from the player by
			//    making a ring of target squares and working from there
			var allTiles = Refs.m.TileList();
			var ring = new HashSet<MapTile>(new MapTileComp());
			foreach (MapTile t in allTiles)
			{
				double c = Loc.Distance(Refs.p.loc, t.loc);
				if (c > distance - 1 && c < distance + 1) { ring.Add(t); }
			}

			return f.FlowSquaresFromTileSet(ring);
		}

		private static HashSet<FlowTile> PlayerNectarTiles(Flow f)
		{
			// tiles containing player nectar are targets too
			var allTiles = Refs.m.TileList();
			var nectarTiles = new HashSet<MapTile>(new MapTileComp());
			foreach (MapTile t in allTiles)
			{
				if (t.hasNectar && t.nectarCol == Refs.p.myColor)
				{
					nectarTiles.Add(t);
				}
			}
			return f.FlowSquaresFromTileSet(nectarTiles);
		}
	}
}