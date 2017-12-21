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
			// target tiles get a .flow of 0, tiles 1 square from target
			//    get a .flow of 1, tiles 2 out get a .flow of 2, etc...
			// So to navigate to a target tile, just pick a tile with less
			//    flow value than where you currently are.

			var sw = new Stopwatch();
			int headsProcessed = 0;
			sw.Start();

			// tidy up values from previous runs
			ClearFlow();

			// get a list of starting tiles(s) as starting point(s)
			List<Tile> heads = SetUpInitialRing();

			foreach (Tile t in heads) { if (t.clear) { t.flow = 0; } }

			// I call them heads because they 'snake' outwards from the initial point(s)
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

					// ... (ignoring any nulls) ...
					newTiles.RemoveAll(item => item == null);

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

		private List<Tile> SetUpInitialRing()
		{
			// we'll try to flow to a set distance from the player by
			//    making a ring of target squares and working from there
			var allTiles = m.TileList();
			var ring = new List<Tile>();
			foreach (Tile t in allTiles)
			{
				// todo de-duplicate with other pythagorus
				double a = Math.Pow(p.loc.X - t.loc.X, 2);
				double b = Math.Pow(p.loc.Y - t.loc.Y, 2);
				double c = Math.Sqrt(a + b);

				if (c > 10 && c < 12) { ring.Add(t); }
			}
			return ring;
		}

		public void ClearFlow()
		{
			foreach (Tile t in m.tiles) { t.flow = 9999; }
		}
	}
}