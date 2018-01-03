using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Beehive
{
	public class MazeGenerator
	{
		private Random rng = new Random();

		public Map Create(int xlen, int ylen)
		{
			var sw = new Stopwatch();
			sw.Start();

			Map NewMap = new Map(xlen, ylen);
			Refs.m = NewMap; // needed for utils

			int xmax = NewMap.GetXLen() - 2;
			int ymax = NewMap.GetYLen() - 2;

			// seed the corners and middle
			NewMap.TileByLoc(new Point(1, 1)).clear = true;
			NewMap.TileByLoc(new Point(1, ymax)).clear = true;
			NewMap.TileByLoc(new Point(xmax, 1)).clear = true;
			NewMap.TileByLoc(new Point(xmax, ymax)).clear = true;
			NewMap.TileByLoc(new Point(xmax / 2, ymax / 2)).clear = true;
			NewMap.InitClearTilesCache();

			// maze generation
			int rounds = 0;
			var clears = NewMap.GetClearTilesCache();
			while (clears.Count > 0)
			{
				// pick a random clear tile
				clears = NewMap.GetClearTilesCache();
				var clear = clears.ElementAt(rng.Next(clears.Count));

				// which ways can we dig from it?
				var nextTo = NewMap.GetNextTo(clear);
				var closed5 = NewMap.GetClosed5Sides(nextTo);
				var andWalls = Tile.FilterOutNotClear(closed5);

				// if there are digging options...
				if (andWalls.Count > 0)
				{
					// ... dig in one of them randomly
					var picked = andWalls.ElementAt(rng.Next(andWalls.Count));
					picked.clear = true;
					NewMap.AddToClearTileCache(picked);
				}
				else
				{
					// ... if not, this tile will never be mined from,
					//    so remove it from the clear tiles cache
					NewMap.DelFromClearTileCache(clear);
				}

				rounds++;
			}
			NewMap.ConsoleDump();
			NewMap.HealWalls();
			NewMap.ConsoleDump();

			// typ old time 230ms, new time 110ms
			Console.WriteLine("Finished mapgen in " + sw.ElapsedMilliseconds + "ms, at "
				+ rounds + " rounds");

			return NewMap;
		}
	}
}