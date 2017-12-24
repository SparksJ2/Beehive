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

			Tile initial = NewMap.TileByLoc(new Point(1, 1));
			initial.clear = true;

			Tile final = NewMap.TileByLoc(
				new Point(NewMap.GetXLen() - 2, NewMap.GetYLen() - 2));

			// todo maze generation
			var options = NewMap.GetNextTo(initial);

			for (int i = 0; i < 100000; i++)
			{
				// pick random clear tile
				var clears = NewMap.ClearTiles();
				var clear = clears[rng.Next(clears.Count)];

				// get tunnel options
				var opts1 = NewMap.GetNextTo(clear);
				var opts2 = NewMap.GetClosed5Sides(opts1);

				// tunnel in random direction
				//NewMap.ConsoleDump();
				if (opts2.Count > 0)
				{
					var opt = opts2[rng.Next(opts2.Count)];
					opt.clear = true;
				}
			}
			NewMap.ConsoleDump();
			NewMap.HealWalls();
			NewMap.ConsoleDump();
			return NewMap;
		}
	}
}