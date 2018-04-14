using System;

namespace Beehive
{
	[Serializable()]
	public class FlowMap : BaseMap<FlowTile>
	{
		private int level;

		public FlowMap(int xIn, int yIn, int levelIn)
		{
			xLen = xIn;
			yLen = yIn;
			level = levelIn;
			tiles = new FlowTile[xLen, yLen];
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y] = new FlowTile(new Loc(x, y), this) { flow = 9999 };
				}
			}
			SetToNines(); // todo didn't we just do this above?
		}

		public static void RemakeAllFlows()
		{
			//var sw = new Stopwatch(); sw.Start();
			foreach (FlowMap f in Refs.m.flows) { f.RemakeFlow(); }
			//Console.WriteLine("Finished all flows in " + sw.ElapsedMilliseconds + "ms.");
		}

		internal double GetHighest()
		{
			double high = 0;
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					if (tiles[x, y].flow > high && tiles[x, y].flow < 2000)
					{
						high = tiles[x, y].flow;
					}
				}
			}
			return high;
		}

		internal double GetLowest()
		{
			double low = 0;
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					if (tiles[x, y].flow < low && tiles[x, y].flow > -2000)
					{
						low = tiles[x, y].flow;
					}
				}
			}
			return low;
		}

		public void SetToNines()
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y].flow = 9999;
				}
			}
		}

		public void MultFactor(double d) // heh
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y].flow *= d;
				}
			}
		}

		public void AdjustFactor(double d) // heh
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					tiles[x, y].flow += d;
				}
			}
		}

		public void Reverse()
		{
			double half = GetHighest() / 2;
			double v;
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					v = tiles[x, y].flow;
					v -= half; v = -v; v += half;
					tiles[x, y].flow = v;
				}
			}
		}

		public void RemakeFlow()
		{
			//var sw = new Stopwatch(); sw.Start();

			// target tiles get a .flow of 0, tiles 1 square from target
			//    get a .flow of 1, tiles 2 out get a .flow of 2, etc...
			// So to navigate to a target tile, just pick a tile with less
			//    flow value than where you currently are.

			// tidy up values from previous runs
			SetToNines();

			// use myAi from here
			if (level > 0) // don't do player flow yet
			{
				// which cubi are we doing this for?
				Cubi c = Harem.GetId(level);

				if (c.doJailBreak)
				{ c.myJbAi(c.teaseDistance, this); }
				else
				{ c.myStdAi(c.teaseDistance, this); }
			}

			// report time
			//Console.WriteLine("Finished this flow in " + sw.ElapsedMilliseconds + "ms.");
		}

		public FlowTileSet AllFlowSquares()
		{
			var flowList = new FlowTileSet();
			foreach (FlowTile fs in tiles) { flowList.Add(fs); }
			return flowList;
		}

		public void RunFlow(Boolean maskWalls)
		{
			FlowTileSet heads = AllFlowSquares();

			if (maskWalls)
			{
				foreach (FlowTile fs in heads)
				{
					// mask out walls etc, we don't flow over those
					MapTile thisTile = Refs.m.TileByLoc(fs.loc);
					if (!thisTile.clear) { fs.mask = true; }
				}
			}

			// I call them heads because they 'snake' outwards from the initial point(s)
			//    but you get splits into several heads at junctions so it's a bad metaphor...

			// loop till we can't find anything more to do,
			// (or we decide we've messed up)
			bool changes = true;
			int failsafe = 0;
			int headsProcessed = 0;
			while (changes == true && failsafe < 256)
			{
				changes = false; failsafe++;

				FlowTileSet newHeads = new FlowTileSet();

				// for each active head tile...
				foreach (FlowTile fs in heads)
				{
					// ...find the tiles next to it...
					FlowTileSet newTiles = new FlowTileSet()
					{
						fs.OneNorth(),
						fs.OneEast(),
						fs.OneSouth(),
						fs.OneWest()
					};

					// ... (ignoring any nulls) ...
					newTiles.RemoveWhere(item => item == null);

					// ... and for each one found ...
					foreach (FlowTile newFlowSq in newTiles)
					{
						// ... if we can improve the flow rating of it ...
						double delta = newFlowSq.flow - fs.flow;

						MapTile targetTile = Refs.m.TileByLoc(fs.loc);
						if (targetTile.clear && delta > 1.0001)
						{
							// ... do so, and then make it a new head ...
							newFlowSq.flow = fs.flow + 1;
							newHeads.Add(newFlowSq);
							changes = true;
						}
					}
					headsProcessed++;
				}
				// ... and next time around, we keep going using those new heads
				heads = newHeads;

				if (failsafe == 255) Console.WriteLine("Hit flow failsafe!");
			}
		}
	}
}