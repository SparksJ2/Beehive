﻿using System.Threading;

namespace Beehive
{
	partial class Player
	{
		/// player-throwing-a-cubi trajectory logic

		private void ThrowCubiMain(Loc vector)
		{
			// todo lots of duplication here
			Player p = Refs.p;
			Cubi cubiThrown = Harem.GetId(p.heldCubiId);

			Refs.mf.Announce("You throw " + cubiThrown.name + " through the air!", myAlign, myColor);
			Refs.mf.Announce("*flap* *flap* *flap*", cubiThrown.myAlign, cubiThrown.myColor);
			cubiThrown.beingCarried = false;
			p.heldCubiId = 0;

			// determine release point of throw
			// todo check for hit on very first tile
			Loc startloc = Loc.AddPts(loc, vector);
			MapTile activeTile = Refs.m.TileByLoc(startloc);

			// if very first tile is a holding pent, they can fly right over
			Loc zero = new Loc(0, 0);
			if (CheckClearForThrown(zero, activeTile) == "pent")
			{
				// oops you threw from too close
				Refs.mf.Announce("*desperate flapping* That was close, just made it over!", cubiThrown.myAlign, cubiThrown.myColor);
			}

			// if the next tile is another cubi, throw short
			// todo consolidate long and short throws
			// todo need to prevent throwing a cubi while ones already directly next to you
			string moveClear = CheckClearForThrown(vector, activeTile);
			if (moveClear == "cubi")
			{
				MapTile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
				Cubi victim = Refs.m.CubiAt(victimTile.loc);

				Refs.mf.Announce("Owf!", cubiThrown.myAlign, cubiThrown.myColor);
				victim.Spanked += 5;

				Refs.mf.Announce("Oof too!", cubiThrown.myAlign, cubiThrown.myColor);
				cubiThrown.Spanked += 5; // and a good time was had by both
			}
			else if (moveClear == "pent")
			{
				// we just overflew a pent so consider her flight path clear for now
				moveClear = "clear";
			}

			// todo refresh player square so she's not superimposed on player square

			while (moveClear == "clear")
			{
				// blip activeTile with cubi symbol
				cubiThrown.loc = activeTile.loc;
				AnimateMobile(activeTile, cubiThrown);

				// is the next tile clear?
				moveClear = CheckClearForThrown(vector, activeTile);

				// nope, it has a cubi in.
				if (moveClear == "cubi")
				{
					MapTile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
					Cubi victim = Refs.m.CubiAt(victimTile.loc);

					MapTileSet escapes = new MapTileSet();

					if (IsVertical(vector)) { escapes = victimTile.GetPossibleMoves(Dir.DodgeVertical); }
					else { escapes = victimTile.GetPossibleMoves(Dir.DodgeHorizontal); }

					if (escapes.Count > 0)
					{
						victim.loc = MainMap.RandomFromList(escapes).loc;
						Refs.mf.Announce("Nyahhh missed me!", victim.myAlign, victim.myColor);
						moveClear = "clear";
					}
					else
					{
						victim.Spanked += 5;
						Refs.mf.Announce("Owwwww!", victim.myAlign, victim.myColor);
					}

					// if it had a cubi, it could have moved revealing a holding pent
					// so let's scan again
					moveClear = CheckClearForThrown(vector, activeTile);
				}

				if (moveClear == "pent")
				{
					// move one more tile
					activeTile = Refs.m.TileByLoc(Loc.AddPts(vector, activeTile.loc));

					Refs.mf.Announce("Eep! I'm caught!", cubiThrown.myAlign, cubiThrown.myColor);
				}

				// just a wall. stop here.
				if (moveClear == "wall")
				{ Refs.mf.Announce("You didn't hit anything interesting.", myAlign, myColor); }

				// it's clear, so move activeTile up and iterate
				if (moveClear == "clear")
				{ activeTile = Refs.m.TileByLoc(Loc.AddPts(vector, activeTile.loc)); }
			}

			// deposit cubi here
			cubiThrown.loc = activeTile.loc;

			Refs.mf.UpdateMap();
		}

		private void AnimateMobile(MapTile activeTile, Cubi c)
		{
			// todo animation code is a bit makeshift and needs to be cleaned up andmoved to Map.cs
			// todo not going to be the right color for anything but pillows...

			Refs.m.CubiSingleTileUpdate(c);
			Refs.mf.Refresh();

			Thread.Sleep(75);

			Refs.m.ResetTile(c.loc);
			Refs.mf.Refresh();
			// todo also fix nectar drops.
		}
	}
}