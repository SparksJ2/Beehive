using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class MazeGenerator
	{
		private Random rng = new Random();

		public Map Create(int xlen, int ylen)
		{
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
			for (int i = 0; i < 20000; i++)
			{
				// pick random clear tile
				var clears = NewMap.GetClearTilesCache();
				var clear = clears[rng.Next(clears.Count)];

				// get tunnel options
				var nextTo = NewMap.GetNextTo(clear);
				var closed5 = NewMap.GetClosed5Sides(nextTo);
				var andWalls = Tile.FilterOutNotClear(closed5);

				// tunnel in random direction
				if (andWalls.Count > 0)
				{
					var picked = andWalls[rng.Next(andWalls.Count)];
					picked.clear = true;
					NewMap.AddToClearTileCache(picked);
				}
			}
			NewMap.ConsoleDump();
			NewMap.HealWalls();
			NewMap.ConsoleDump();
			return NewMap;
		}
	}
}