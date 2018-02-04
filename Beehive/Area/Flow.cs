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
		private FlowTile[,] flowMap;
		private int xLen, yLen;
		private int level;

		public static void RemakeAllFlows()
		{
			//var sw = new Stopwatch(); sw.Start();
			foreach (Flow f in Refs.m.flows) { f.RemakeFlow(); }
			//Console.WriteLine("Finished all flows in " + sw.ElapsedMilliseconds + "ms.");
		}

		public Flow(int xIn, int yIn, int levelIn)
		{
			xLen = xIn;
			yLen = yIn;
			level = levelIn;
			flowMap = new FlowTile[xLen, yLen];
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					flowMap[x, y] = new FlowTile(new Loc(x, y), this) { flow = 9999 };
				}
			}
			SetToNines();
		}

		internal double GetHighest()
		{
			double high = 0;
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					if (flowMap[x, y].flow > high && flowMap[x, y].flow < 2000)
					{
						high = flowMap[x, y].flow;
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
					if (flowMap[x, y].flow < low && flowMap[x, y].flow > -2000)
					{
						low = flowMap[x, y].flow;
					}
				}
			}
			return low;
		}

		public FlowTile FlowSquareByLoc(Loc p)
		{
			return flowMap[p.X, p.Y];
		}

		public void SetToNines()
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					flowMap[x, y].flow = 9999;
				}
			}
		}

		public void MultFactor(double d) // heh
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					flowMap[x, y].flow *= d;
				}
			}
		}

		public void AdjustFactor(double d) // heh
		{
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					flowMap[x, y].flow += d;
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
					v = flowMap[x, y].flow;
					v -= half; v = -v; v += half;
					flowMap[x, y].flow = v;
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

				c.myAi(c.teaseDistance, this);
			}

			// report time
			//Console.WriteLine("Finished this flow in " + sw.ElapsedMilliseconds + "ms.");
		}

		public HashSet<FlowTile> AllFlowSquares()
		{
			var flowList = new HashSet<FlowTile>(new FlowTileComp());
			foreach (FlowTile fs in flowMap) { flowList.Add(fs); }
			return flowList;
		}

		public void RunFlow(Boolean maskWalls)
		{
			HashSet<FlowTile> heads = AllFlowSquares();

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

				HashSet<FlowTile> newHeads = new HashSet<FlowTile>(new FlowTileComp());

				// for each active head tile...
				foreach (FlowTile fs in heads)
				{
					// ...find the tiles next to it...
					HashSet<FlowTile> newTiles = new HashSet<FlowTile>(new FlowTileComp())
					{
						fs.OneNorth(), fs.OneEast(),
						fs.OneSouth(), fs.OneWest()
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

		internal HashSet<FlowTile> FlowSquaresFromTileSet(HashSet<MapTile> tiles)
		{
			var result = new HashSet<FlowTile>(new FlowTileComp());

			foreach (MapTile t in tiles)
			{
				result.Add(FlowSquareByLoc(t.loc));
			}

			return result;
		}

		internal HashSet<MapTile> TileSetFromFlowSquares(HashSet<FlowTile> squares)
		{
			var result = new HashSet<MapTile>(new MapTileComp());

			foreach (FlowTile fs in squares)
			{
				result.Add(Refs.m.TileByLoc(fs.loc));
			}

			return result;
		}
	}
}