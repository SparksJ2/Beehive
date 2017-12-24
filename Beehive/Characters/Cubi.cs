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
		public Random rng;
		public int spanked = 0;

		public Cubi() : base()
		{
			rng = new Random();
		}

		public void AiMove()
		{
			if (spanked > 0) { spanked--; return; }

			var maybe = new List<Tile>();

			Tile here = Refs.m.tiles[loc.X, loc.Y];

			maybe.Add(here.OneEast());
			maybe.Add(here.OneSouth());
			maybe.Add(here.OneNorth());
			maybe.Add(here.OneWest());
			maybe.Add(here); // not moving is now an option

			// filter not clear maybes
			maybe = maybe.Where(t => t.clear).ToList();

			// don't move directly onto player
			maybe = maybe.Where(t => t.loc != Refs.p.loc).ToList();

			// pick a possibility and go there.
			if (maybe.Count > 0)
			{
				maybe = maybe.OrderBy(i => i.flow).ToList(); // linq ftw

				// make a list of best tiles
				int bestflow = maybe[0].flow;
				List<Tile> bests = maybe.Where(t => t.flow == bestflow).ToList();

				// choose randomly between best tiles
				Tile newplace = bests[rng.Next(bests.Count)];
				loc = newplace.loc;
			}
		}
	}
}