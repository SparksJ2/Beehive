using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Beehive
{
	public class Cubi : Mobile
	{
		// todo needs a general rng singleton or something
		public Random rng;

		private int spanked = 0;
		private int horny = 0;
		public bool beingCarried = false;
		public int IdNo;

		public HorizontalAlignment myAlign = HorizontalAlignment.Right;

		public Cubi(string name, int id, Color useColor) : base(name, useColor)
		{
			rng = new Random();
			IdNo = id;
			glyph = "☿";
		}

		public void Spank(int i)
		{
			spanked += i;
		}

		public void AiMove()
		{
			Tile here = Refs.m.TileByLoc(loc);

			// being close to player makes for horny cubi
			if (DistToPlayer() < 5.0)
			{
				horny++;
			}

			// leave nectar trail
			if (horny > 0 && here.Cnectar == false)
			{
				here.Cnectar = true;
				here.nectarCol = myColor;
				horny--;
			}

			var noMove = false;
			if (spanked > 0)
			{
				spanked--;
				// pain ==> pleasure
				horny++;
				noMove = true; // too oww to move.
			}

			if (beingCarried)
			{
				loc = Refs.p.loc;
				return;
			}
			else if (OnBed())
			{
				return;
			}
			else if (!noMove)
			{
				AIPathing();
			}
		}

		private bool OnBed()
		{
			// todo fix hardcoded location
			return ((loc.X == 31 && loc.Y == 12) ||
				(loc.X == 31 + 3 && loc.Y == 12) ||
				(loc.X == 31 + 6 && loc.Y == 12));
		}

		private void AIPathing()
		{
			Tile here = Refs.m.TileByLoc(loc);

			var maybe = new HashSet<Tile>(new TileComp())
			{
				here.OneEast(),
				here.OneSouth(),
				here.OneNorth(),
				here.OneWest()
			};

			// filter not clear maybes
			maybe = maybe.Where(t => t.clear).ToTileHashSet();

			// don't move directly onto player
			maybe = maybe.Where(t => t.loc != Refs.p.loc).ToTileHashSet();

			// don't move directly onto another cubi
			foreach (Cubi c in Refs.h.roster)
			{
				maybe = maybe.Where(t => t.loc != c.loc).ToTileHashSet();
			}

			// pick a possibility and go there.
			if (maybe.Count > 0)
			{
				int bestflow = maybe.Min(t => t.flow); // linq ftw

				// is the tile that we're currently on already one of the best tiles?
				if (here.flow != bestflow)
				{
					// make a list of best tiles
					HashSet<Tile> bests = maybe.Where(t => t.flow == bestflow).ToTileHashSet();

					// choose randomly between best tiles
					// todo there is a method for rng tiles now
					Tile newplace = bests.ElementAt(rng.Next(bests.Count));
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

		private double DistToPlayer()
		{
			double a = Math.Pow(Refs.p.loc.X - loc.X, 2);
			double b = Math.Pow(Refs.p.loc.Y - loc.Y, 2);
			return Math.Sqrt(a + b);
		}
	}
}