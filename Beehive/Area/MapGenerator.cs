using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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

			// seed the corners
			NewMap.TileByLoc(new Loc(1, 1)).clear = true;
			NewMap.TileByLoc(new Loc(1, ymax)).clear = true;
			NewMap.TileByLoc(new Loc(xmax, 1)).clear = true;
			NewMap.TileByLoc(new Loc(xmax, ymax)).clear = true;

			// set up central area
			var homeStartClear = new Loc(29, 10);
			var homeEndClear = new Loc(39, 14);
			NewMap.MakeClearArea(homeStartClear, homeEndClear);

			// mark home walls not to be tunneled away
			var homeStartWall = new Loc(homeStartClear.X - 1, homeStartClear.Y - 1);
			var homeEndWall = new Loc(homeEndClear.X + 1, homeEndClear.Y + 1);
			NewMap.MarkNoTunnel(homeStartWall, homeEndWall);

			// todo hardcoded doorway
			var doorStartClear = new Loc(32, 9);
			var doorEndClear = new Loc(36, 11);
			NewMap.MakeClearArea(doorStartClear, doorEndClear);

			// todo hardcoded initial departure area
			var spawnStartClear = new Loc(29, 7);
			var spawnEndClear = new Loc(39, 10);
			NewMap.MakeClearArea(spawnStartClear, spawnEndClear);

			//NewMap.TileByLoc(new Loc(35, homeStartClear.Y)).clear = true;
			//NewMap.TileByLoc(new Loc(34, homeStartClear.Y)).clear = true;
			//NewMap.TileByLoc(new Loc(33, homeStartClear.Y)).clear = true;
			//NewMap.TileByLoc(new Loc(32, homeStartClear.Y)).clear = true;

			// cache startup
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
				var isClosed5sides = NewMap.GetClosed5Sides(nextTo);

				var andWalls = MapTile.FilterOutClear(isClosed5sides);
				var andTunnelable = MapTile.Tunnelable(andWalls);

				var candidates = andTunnelable;

				// if there are digging options...
				if (candidates.Count > 0)
				{
					// ... dig in one of them randomly
					var picked = candidates.ElementAt(rng.Next(candidates.Count));
					picked.clear = true;
					NewMap.AddToClearTileCache(picked);
				}
				else
				{
					// ... if not, this tile will never be mined from,
					//    so remove it from the clear tiles cache
					NewMap.DelFromClearTileCache(clear);
				}

				//Refs.mf.UpdateMap(); // for debugging visualization
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