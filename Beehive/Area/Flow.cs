using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Diagnostics;

namespace Beehive
{
	public class Flow
	{
		public void RemakeAllFlows()
		{
			foreach (Cubi c in Refs.h.roster)
			{
				RemakeFlow(c.IdNo);
			}
		}

		public void RemakeFlow(int level)
		{
			// target tiles get a .flow of 0, tiles 1 square from target
			//    get a .flow of 1, tiles 2 out get a .flow of 2, etc...
			// So to navigate to a target tile, just pick a tile with less
			//    flow value than where you currently are.

			var sw = new Stopwatch();
			int headsProcessed = 0;
			sw.Start();

			// tidy up values from previous runs
			ClearFlow(level);

			// get a list of starting tiles(s) as starting loc(s)
			HashSet<Tile> heads = Refs.h.roster[level - 1].myAi();

			foreach (Tile t in heads) { if (t.clear) { t.flow[level] = 0; } }

			// I call them heads because they 'snake' outwards from the initial point(s)
			//    but you get splits into several heads at junctions so it's a bad metaphor...

			// loop till we can't find anything more to do,
			// (or we decide we've messed up)
			bool changes = true;
			int failsafe = 0;
			while (changes == true && failsafe < 256)
			{
				changes = false; failsafe++;

				HashSet<Tile> newHeads = new HashSet<Tile>(new TileComp());
				// for each active head tile...
				foreach (Tile head in heads)
				{
					// ...find the tiles next to it...
					HashSet<Tile> newTiles = new HashSet<Tile>(new TileComp())
					{
						head.OneNorth(), head.OneEast(),
						head.OneSouth(), head.OneWest()
					};

					// ... (ignoring any nulls) ...

					newTiles.RemoveWhere(item => item == null);

					// ... and for each one found ...
					foreach (Tile newTile in newTiles)
					{
						// ... if we can improve the flow rating of it ...
						int delta = newTile.flow[level] - head.flow[level];
						if (newTile.clear && delta > 2)
						{
							// ... do so, and then make it a new head ...
							newTile.flow[level] = head.flow[level] + 1;
							newHeads.Add(newTile);
							changes = true;
						}
					}
					headsProcessed++;
				}
				// ... and next time around, we keep going using those new heads
				heads = newHeads;

				if (failsafe == 255) Console.WriteLine("Hit flow failsafe!");
			}
			Console.WriteLine(
				"Finished flow in " + sw.ElapsedMilliseconds + "ms, " +
				"heads = " + headsProcessed + ", " +
				"failsafe reached = " + failsafe + ".");
		}

		private void ClearFlow(int level)
		{
			foreach (Tile t in Refs.m.TileList()) { t.flow[level] = 9999; }
		}
	}
}