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
		public Player p;

		public Cubi(MainForm mf, Map m, Player master) : base(mf, m)
		{
			rng = new Random();
			p = master;
		}

		public void AiMove()
		{
			var maybe = new List<Tile>();

			Tile here = map.tiles[loc.X, loc.Y];

			if (map.OneEast(here).clear) { maybe.Add(map.OneEast(here)); }
			if (map.OneSouth(here).clear) { maybe.Add(map.OneSouth(here)); }
			if (map.OneNorth(here).clear) { maybe.Add(map.OneNorth(here)); }
			if (map.OneWest(here).clear) { maybe.Add(map.OneWest(here)); }

			// don't move directly onto player
			Tile oops = null;
			foreach (Tile check in maybe) if (check.loc == p.loc) oops = check;
			if (oops != null)
			{
				maybe.Remove(oops);
				Console.WriteLine("oops!");
			}

			// pick a possibility and go there.
			if (maybe.Count > 0)
			{
				Tile newplace = maybe[rng.Next(maybe.Count)];
				loc.X = newplace.loc.X;
				loc.Y = newplace.loc.Y;
			}
			// todo add NorthIfClear, etc
		}
	}
}