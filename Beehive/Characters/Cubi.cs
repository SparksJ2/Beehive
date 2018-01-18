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
	public delegate HashSet<Tile> CubiAiType();

	public class Cubi : Mobile
	{
		private int spanked = 0; // or other wise incapped, e.g. orgasm throes
		public bool beingCarried = false;
		public int IdNo;
		public CubiAiType myAi;

		public HorizontalAlignment myAlign = HorizontalAlignment.Right;

		private static Random rng = new Random();

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
			if (horny > 0 && here.hasNectar == false)
			{
				here.hasNectar = true;
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

				// consume player nectar
				if (here.hasNectar && here.nectarCol == Refs.p.myColor)
				{
					Refs.mf.Announce("Yes, masters nectar! *lap lap*", myAlign, myColor);
					here.hasNectar = false;
					horny += 5;
				}

				if (horny > 15) // having fun
				{
					Refs.mf.Announce("Aieee I'm cumming! *splurt*", myAlign, myColor);
					Map.SplurtNectar(here, myColor);
					spanked += 5;
					horny = 0;
				}
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
				int bestflow = maybe.Min(t => t.flow[IdNo]); // linq ftw

				// is the tile that we're currently on already one of the best tiles?
				if (here.flow[IdNo] != bestflow)
				{
					// make a list of best tiles
					HashSet<Tile> bests = maybe.Where(t => t.flow[IdNo] == bestflow).ToTileHashSet();

					// choose randomly between best tiles
					Tile newplace = Tile.RandomFromList(bests);

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
			return Loc.Distance(Refs.p.loc, loc);
		}
	}
}