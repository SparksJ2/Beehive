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
	internal class Flow
	{
		public Map m;
		public Player p;
		public Cubi c;

		public Flow(Map mp, Player pl, Cubi cu)
		{ m = mp; p = pl; c = cu; }

		public void RemakeFlow(Point target)
		{
			var sw = new Stopwatch();
			int headsProcessed = 0;
			sw.Start();

			ClearFlow();

			// add target tile as a starting point
			Tile targetTile = m.TileByLoc(target);
			targetTile.flow = 0;
			List<Tile> heads = new List<Tile> { targetTile };

			// I call them heads because they 'snake' outwards from the initial point
			//    but you get splits into several heads at junctions so it's a bad metaphor...

			// loop till we can't find anything more to do,
			// (or we decide we've messed up)
			bool changes = true;
			int failsafe = 0;
			while (changes == true && failsafe < 256)
			{
				changes = false; failsafe++;

				List<Tile> newHeads = new List<Tile>();
				// for each active head tile...
				foreach (Tile head in heads)
				{
					// ...find the tiles next to it...
					List<Tile> newTiles = new List<Tile>
						{m.OneNorth(head), m.OneEast(head),
						 m.OneSouth(head), m.OneWest(head) };

					// ... and for each one found ...
					foreach (Tile newTile in newTiles)
					{
						// ... if we can improve the flow rating of it ...
						int delta = newTile.flow - head.flow;
						if (newTile.clear && delta > 2)
						{
							// ... do so, and then make it a new head ...
							newTile.flow = head.flow + 1;
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

		public void ClearFlow()
		{
			foreach (Tile t in m.tiles) { t.flow = 9999; }
		}
	}
}