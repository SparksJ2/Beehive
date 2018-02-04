using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Beehive
{
	public delegate void CubiAiType(int distance, Flow f);

	public class Cubi : Mobile
	{
		private int spanked = 0; // or other wise incapped, e.g. orgasm throes
		public bool beingCarried = false;
		public int myIdNo;
		public CubiAiType myAi;
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

			// todo bored just slowly goes up for now
			// todo turned bored off while I work on Ai stuff
			//if (bored < 11.0) { bored += 0.1; }
			//teaseDistance = Convert.ToInt32(11 - bored);

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
			MapTile myTile = Refs.m.TileByLoc(loc);

			// places one square away that we could go to
			var maybeTiles = new HashSet<MapTile>(new MapTileComp())
			{
				myTile.OneEast(),
				myTile.OneSouth(),
				myTile.OneNorth(),
				myTile.OneWest()
			};

			// filter tiles containing wall
			maybeTiles = maybeTiles.Where(t => t.clear).ToTileHashSet();

			// don't move directly onto player
			maybeTiles = maybeTiles.Where(t => t.loc != Refs.p.loc).ToTileHashSet();

			// or right next to the player!
			Loc playerLoc = Refs.p.loc;
			MapTile playerTile = Refs.m.TileByLoc(playerLoc);

			var grabRange = playerTile.GetPossibleMoves(Dir.Cardinals);
			foreach (MapTile g in grabRange)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != g.loc).ToTileHashSet();
			}

			// don't move directly onto another cubi
			foreach (Cubi c in Refs.h.roster)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != c.loc).ToTileHashSet();
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
			}

			// pick a possibility and go there.
			if (maybeTiles.Count > 0)
			{
				Flow myFlow = Refs.m.flows[myIdNo];

				// convert maybe tiles to maybe squares
				HashSet<FlowTile> maybeSquares = myFlow.FlowSquaresFromTileSet(maybeTiles);

				// is the tile that we're currently on already one of the best tiles?
				double bestFlow = maybeSquares.Min(sq => sq.flow);
				FlowTile hereSquare = myFlow.FlowSquareByLoc(myTile.loc);

				// if we're not in an optimal place...
				if (hereSquare.flow > bestFlow)
				{
					// make a list of best flowsquares...
					HashSet<FlowTile> bestSquares =
						maybeSquares.Where(t => t.flow == bestFlow).ToFlowSquareHashSet();

					// convert back to tiles..
					HashSet<MapTile> bestTiles = myFlow.TileSetFromFlowSquares(bestSquares);

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

		private bool FiftyFifty() => rng.NextDouble() > 0.5;

		private double DistToPlayer() => Loc.Distance(Refs.p.loc, loc);
	}
}