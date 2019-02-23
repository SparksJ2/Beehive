using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Beehive
{
	public delegate void CubiStdAi(int distance, FlowMap f);

	public delegate void CubiJbAi(int distance, FlowMap f);

	[Serializable()]
	public class Cubi : Mobile
	{
		public int myIdNo;

		public bool beingCarried = false;

		private int spanked; // or other wise incapped, e.g. orgasm throes
		private int jumpEnergy;

		public int Spanked
		{
			get => spanked;
			set
			{
				// all spanking should be input here
				spanked = value;
				bored = 0;
			}
		}

		private double bored;
		public double Bored { get => bored; set => bored = value; }

		private int teaseDistance;
		public int TeaseDistance { get => teaseDistance; set => teaseDistance = value; }

		public CubiStdAi myStdAi;
		public CubiJbAi myJbAi;
		public bool doJailBreak = false;

		private static Random rng = new Random();

		public Cubi(int id) : base()
		{
			/// freshly hatched!
			myIdNo = id;
			rng = new Random();
			Bored = 0;
			Spanked = 0;
			TeaseDistance = 11;
			jumpEnergy = 6;
			myAlign = HorizontalAlignment.Right;
		}

		public void AiMove()
		{
			/// our turn! what do we do?

			MapTile here = Refs.m.TileByLoc(loc);

			// decide if we're going to attempt a jailbreak
			//  note: has no effect until next turn due to the
			//  flows already being made at this point

			// jailbreak status reporting disabled
			//bool oldJB = doJailBreak;

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
			//if (bored < 11.0) { bored += 0.1; }
			//teaseDistance = Convert.ToInt32(11 - bored);

			// being carried resets boredom
			if (beingCarried) { Bored = 0; }

			// being close to player makes for horny cubi
			if (DistToPlayer() < 5.0) { horny++; }

			// leave nectar trail, color overlays previous trails
			if (horny > 0)
			{
				here.nectarLevel[myIdNo]++;
				horny--;
			}

			// charge the jump drives
			if (jumpEnergy < 10) { jumpEnergy++; }

			var spankNoMove = false;
			if (Spanked > 0) // todo integrate this with the if/else chain below
			{
				Spanked--;
				// pain ==> pleasure
				horny++;
				spankNoMove = true; // too oww to move.
				if (Spanked == 0)
				{
					Refs.mf.Announce("That was intense... but time to get going again!", myAlign, myColor);
				}
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
			else if (!spankNoMove)
			{
				// make a move!
				AIPathing();

				// consume player nectar
				if (here.nectarLevel[0] > 0) // level [0] for player nectar
				{
					Refs.mf.Announce("Yes, masters nectar! *lap lap*", myAlign, myColor);
					horny += here.nectarLevel[0] * 5;
					here.nectarLevel[0] = 0;
				}

				if (horny > 15) // having fun
				{
					Refs.mf.Announce("Aieee I'm cumming! *splurt*", myAlign, myColor);
					MainMap.SplurtNectar(here, myIdNo);
					Spanked += 5;
					horny = 0;
				}
			}
		}

		private bool OnPent()
		{
			/// are we sitting on a pent?
			foreach (Loc pent in Refs.m.pents) { if (Loc.Same(pent, loc)) { return true; } }
			return false;
		}

		private void AIPathing()
		{
			/// we get to move! yay! but where to?

			MapTile myTile = Refs.m.TileByLoc(loc);

			// places one square away that we could go to
			var maybeTiles = myTile.GetPossibleMoves(Dir.Cardinals);

			// filter player grab range, walls, other cubi, pents, etc...
			maybeTiles.FilterNavHazards(maybeTiles);

			// pick a possibility and go there.
			if (maybeTiles.Count > 0)
			{
				if (DistToPlayer() < 1.5) // close range tactical maneuvers!
				{
					// last second evasion, our hope is to move tactically to avoid a foolish mistake
					//  e.g. getting stuck in a little dead end corner typically
					Console.WriteLine("--" + name + " tactical evading!");

					if (jumpEnergy > 20)
					{
						var knightTiles = myTile.GetPossibleMoves(Dir.KnightMoves);
						maybeTiles.UnionWith(knightTiles);
						Console.WriteLine("--" + name + " can jump!");
					}

					maybeTiles.FilterNavHazards(maybeTiles);

					if (maybeTiles.Count > 0)
					{
						Console.WriteLine("--" + name + " choosing between " + maybeTiles.Count + " tiles");

						// find tile furthest from player
						MapTile furthest;
						double distance = 0;
						furthest = null;
						foreach (MapTile t in maybeTiles)
						{
							double tmpDist = Loc.Distance(t.loc, Refs.p.loc);
							if (tmpDist > distance)
							{
								distance = tmpDist;
								furthest = t;
							}
						}

						if (furthest != null)
						{
							// finally, perform move to selected tile!
							loc = furthest.loc;
							Console.WriteLine("--" + name + " evaded ok?");
						}
						else
						{
							Console.WriteLine("--" + name + " ended up holding a null? :(");
						}
					}
					else
					{
						Console.WriteLine("--" + name + " couldn't find square to evade to!");
					}

					// final note: it's possible to tactially evade onto
					//  a pentagram but I'll leave it because it's hilarious
				}
				else // follow the flow
				{
					// convert maybe tiles to maybe squares
					FlowMap myFlow = Refs.m.flows[myIdNo];
					FlowTileSet maybeSquares = ConvertTiles.FlowSquaresFromTileSet(maybeTiles, myFlow);

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
						MapTile newplace = MainMap.RandomFromList(bestTiles);

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
		}

		private void IfClearMoveTo(MapTile t)
		{
			/// used for last second tactical evasion

			// if (not wall and not player), move there
			if (t.clear == true && t.loc != Refs.p.loc) { loc = t.loc; }
		}

		private void FreeMoveToNearbySafeSquare()
		{
			/// used when jailbreaking a cubi, to move it out of the pent

			// todo a fair bit of duplication here could be moved into map utility funcions

			// where are we?
			MapTile myTile = Refs.m.TileByLoc(loc);

			// where can we go to?
			var maybeTiles = myTile.GetPossibleMoves(Dir.AllAround);

			// filter tiles containing wall
			maybeTiles = maybeTiles.Where(t => t.clear).ToMapTileSet();

			// don't move directly onto player
			maybeTiles = maybeTiles.Where(t => t.loc != Refs.p.loc).ToMapTileSet();

			// or into players grab range!
			Loc playerLoc = Refs.p.loc;
			MapTile playerTile = Refs.m.TileByLoc(playerLoc);

			var grabRange = playerTile.GetPossibleMoves(Dir.Cardinals);
			foreach (MapTile g in grabRange)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != g.loc).ToMapTileSet();
			}

			// move if we can
			if (maybeTiles.Count > 0)
			{
				// choose randomly between tiles...
				loc = MainMap.RandomFromList(maybeTiles).loc;
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

		// not currently used, stats filled instead using Grimoire.
		//public Cubi(string name, int id, Color useColor) : base(name, useColor)
		//{
		//	rng = new Random();
		//	myIdNo = id;
		//	glyph = "☿";
		//}
	}
}