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

			if (Refs.m.OneEast(here).clear) { maybe.Add(Refs.m.OneEast(here)); }
			if (Refs.m.OneSouth(here).clear) { maybe.Add(Refs.m.OneSouth(here)); }
			if (Refs.m.OneNorth(here).clear) { maybe.Add(Refs.m.OneNorth(here)); }
			if (Refs.m.OneWest(here).clear) { maybe.Add(Refs.m.OneWest(here)); }

			// don't move directly onto player
			Tile oops = null;
			foreach (Tile check in maybe) if (check.loc == Refs.p.loc) oops = check;
			if (oops != null)
			{
				maybe.Remove(oops);
				Console.WriteLine("oops!");
			}

			// pick a possibility and go there.
			if (maybe.Count > 0)
			{
				maybe = maybe.OrderBy(i => -i.flow).ToList(); // linq ftw

				// just follow the flow
				Tile newplace = maybe[maybe.Count - 1];
				loc.X = newplace.loc.X;
				loc.Y = newplace.loc.Y;
			}
			// todo add NorthIfClear, etc
		}
	}
}