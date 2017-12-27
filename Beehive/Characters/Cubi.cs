using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Beehive
{
	public class Cubi : Mobile
	{
		private Random rng;
		private int spanked = 0;

		public Cubi(string name) : base(name)
		{
			rng = new Random();
		}

		public void Spank(int i)
		{
			spanked += i;
		}

		public void AiMove()
		{
			if (spanked > 0) { spanked--; return; }

			var maybe = new List<Tile>();

			Tile here = Refs.m.TileByLoc(loc);

			maybe.Add(here.OneEast());
			maybe.Add(here.OneSouth());
			maybe.Add(here.OneNorth());
			maybe.Add(here.OneWest());

			// filter not clear maybes
			maybe = maybe.Where(t => t.clear).ToList();

			// don't move directly onto player
			maybe = maybe.Where(t => t.loc != Refs.p.loc).ToList();

			// pick a possibility and go there.
			if (maybe.Count > 0)
			{
				int bestflow = maybe.Min(t => t.flow); // linq ftw

				// is the tile that we're currently on already one of the best tiles?
				if (here.flow != bestflow)
				{
					// make a list of best tiles
					List<Tile> bests = maybe.Where(t => t.flow == bestflow).ToList();

					// choose randomly between best tiles
					Tile newplace = bests[rng.Next(bests.Count)];
					loc = newplace.loc;
				}
				else
				{
					// not moving is a viable option
					// don't vibrate between good tiles
					//    (at least not in this way)
				}
			}
		}
	}
}