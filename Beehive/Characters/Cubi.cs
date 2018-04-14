using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Beehive
{
	public delegate void CubiStdAi(int distance, FlowMap f);

	public delegate void CubiJailBreak(int distance, FlowMap f);

	[Serializable()]
	public class Cubi : Mobile
	{
		private int spanked = 0; // or other wise incapped, e.g. orgasm throes
		public bool beingCarried = false;
		public int myIdNo;

		public CubiStdAi myStdAi;
		public bool doJailBreak = false;
		public CubiJailBreak myJbAi;

		public double bored = 0.0;
		public int teaseDistance = 11;

		public HorizontalAlignment myAlign = HorizontalAlignment.Right;

		private static Random rng = new Random();

		public Cubi(string name, int id, Color useColor) : base(name, useColor)
		{
			rng = new Random();
			myIdNo = id;
			glyph = "☿";
		}

		public void Spank(int i)
		{
			spanked += i;
			bored = 0;
		}

		public void AiMove()
		{
			MapTile here = Refs.m.TileByLoc(loc);

			// decide if we're going to attempt a jailbreak
			//  note: has no effect until next turn due to the
			//  flows already being made at this point
			bool oldJB = doJailBreak;

			doJailBreak = (AnyCubiCaught() && !OnPent() && !beingCarried && DistToPlayer() > 10);

			//if (!oldJB && doJailBreak)
			//{ Refs.mf.Announce(name + " attempting jailbreak! (debug)", myAlign, myColor); }

			//if (oldJB && !doJailBreak)
			//{ Refs.mf.Announce(name + " cancelling jailbreak! (debug)", myAlign, myColor); }

			// spring a fellow cubi free if they're bound next to us
			foreach (Cubi c in Refs.h.roster)
			{
				if (c != this && c.OnPent() && Loc.Distance(loc, c.loc) < 1.5)
				{
					c.FreeMoveToNearbySafeSquare();
					Refs.mf.Announce(c.name + ", you're free, sweetie! Run!", myAlign, myColor);

					// note they can do this even while being carried... leaving it in for comedy.
				}
			}

			// todo 'bored' just slowly goes up for now
			// todo turned 'bored' off while I work on Ai stuff
			// if (bored < 11.0) { bored += 0.1; }
			// teaseDistance = Convert.ToInt32(11 - bored);

			// being carried resets boredom
			if (beingCarried) { bored = 0; }

			// being close to player makes for horny cubi
			if (DistToPlayer() < 5.0) { horny++; }

			// leave nectar trail, overlay previous trails
			if (horny > 0)
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
				// move with the player
				loc = Refs.p.loc;
				return;
			}
			else if (OnPent())
			{
				// trapped!
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
					MainMap.SplurtNectar(here, myColor);
					spanked += 5;
					horny = 0;
				}
			}
		}

		private bool OnPent()
		{
			foreach (Loc pent in Refs.m.pents) { if (Loc.Same(pent, loc)) { return true; } }
			return false;
		}

		private void AIPathing()
		{
			MapTile myTile = Refs.m.TileByLoc(loc);

			// places one square away that we could go to
			// todo use GetPossibleMoves here
			var maybeTiles = new MapTileSet()
			{
				myTile.OneEast(),
				myTile.OneSouth(),
				myTile.OneNorth(),
				myTile.OneWest()
			};

			// filter tiles containing wall
			maybeTiles = maybeTiles.Where(t => t.clear).ToMapTileSet();

			// don't move directly onto player
			maybeTiles = maybeTiles.Where(t => t.loc != Refs.p.loc).ToMapTileSet();

			// or right next to the player!
			Loc playerLoc = Refs.p.loc;
			MapTile playerTile = Refs.m.TileByLoc(playerLoc);

			var grabRange = playerTile.GetPossibleMoves(Dir.Cardinals);
			foreach (MapTile g in grabRange)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != g.loc).ToMapTileSet();
			}

			// don't move directly onto another cubi
			foreach (Cubi c in Refs.h.roster)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != c.loc).ToMapTileSet();
			}

			// and don't move directly onto a pent!
			foreach (Loc pent in Refs.m.pents)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != pent).ToMapTileSet();
			}

			if (DistToPlayer() < 1.1) // close range tactical maneuvers!
			{
				Console.WriteLine(name + " tactical evading!");
				// last second evasion, our hope is to move tactically to avoid a foolish mistake

				// first, try to move directly away
				// todo fairly ugly but will do for now...
				Loc relative = Loc.SubPts(loc, playerLoc);
				MapTile southTile = myTile.OneSouth();
				MapTile northTile = myTile.OneNorth();
				MapTile eastTile = myTile.OneEast();
				MapTile westTile = myTile.OneWest();

				if (Loc.Same(relative, Dir.North)) { IfClearMoveTo(northTile); return; }
				if (Loc.Same(relative, Dir.South)) { IfClearMoveTo(southTile); return; }
				if (Loc.Same(relative, Dir.East)) { IfClearMoveTo(eastTile); return; }
				if (Loc.Same(relative, Dir.West)) { IfClearMoveTo(westTile); return; }
				Console.WriteLine(name + " away move evasion failed, todo attemptinging sideways move...");

				// todo - secondly, try to move to the square diagonal to the player

				// todo decision making still a problem, we shouldn't move directly away when diagonally would be better...

				// note it's possible to tactially evade onto
				//  a pentagram but I'll leave it because it's hilarious
			}

			// pick a possibility and go there.
			if (maybeTiles.Count > 0)
			{
				FlowMap myFlow = Refs.m.flows[myIdNo];

				// convert maybe tiles to maybe squares
				HashSet<FlowTile> maybeSquares = ConvertTiles.FlowSquaresFromTileSet(maybeTiles, myFlow);

				// is the tile that we're currently on already one of the best tiles?
				double bestFlow = maybeSquares.Min(sq => sq.flow);
				FlowTile hereSquare = myFlow.TileByLoc(myTile.loc);

				// if we're not in an optimal place...
				if (hereSquare.flow > bestFlow)
				{
					// make a list of best flowsquares...
					FlowTileSet bestSquares =
						maybeSquares.Where(t => t.flow == bestFlow).ToFlowTileSet();

					// convert back to tiles..
					MapTileSet bestTiles = ConvertTiles.TileSetFromFlowSquares(bestSquares);

					// choose randomly between best tiles...
					MapTile newplace = MapTile.RandomFromList(bestTiles);

					// finally, perform move to selected tile!
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

		private void IfClearMoveTo(MapTile t)
		{
			// not wall and not player
			if (t.clear == true && t.loc != Refs.p.loc) { loc = t.loc; }
		}

		private void FreeMoveToNearbySafeSquare()
		{
			// todo a fair bit of duplication here could be moved into map utility funcions

			// where are we?
			MapTile myTile = Refs.m.TileByLoc(loc);

			// where can we go to?
			var maybeTiles = myTile.GetPossibleMoves(Dir.AllAround);

			// filter tiles containing wall
			maybeTiles = maybeTiles.Where(t => t.clear).ToMapTileSet();

			// don't move directly onto player
			maybeTiles = maybeTiles.Where(t => t.loc != Refs.p.loc).ToMapTileSet();

			// or right next to the player!
			Loc playerLoc = Refs.p.loc;
			MapTile playerTile = Refs.m.TileByLoc(playerLoc);

			var grabRange = playerTile.GetPossibleMoves(Dir.Cardinals);
			foreach (MapTile g in grabRange)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != g.loc).ToMapTileSet();
			}

			if (maybeTiles.Count > 0)
			{
				// choose randomly between tiles...
				loc = MapTile.RandomFromList(maybeTiles).loc;
			}
		}

		private bool FiftyFifty() => rng.NextDouble() > 0.5;

		private double DistToPlayer() => Loc.Distance(Refs.p.loc, loc);

		private bool AnyCubiCaught()
		{
			foreach (Cubi c in Refs.h.roster)
			{ if (c.OnPent()) return true; }
			return false;
		}
	}
}