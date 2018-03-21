using System.Threading;

namespace Beehive
{
	partial class Player
	{
		private void ThrowNorth()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneNorth();
			if (t.clear) ThrowDirection(Dir.North);
		}

		private void ThrowWest()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneWest();
			if (t.clear) ThrowDirection(Dir.West);
		}

		private void ThrowEast()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneEast();
			if (t.clear) ThrowDirection(Dir.East);
		}

		private void ThrowSouth()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneSouth();
			if (t.clear) ThrowDirection(Dir.South);
		}

		private void ThrowDirection(Loc vector)
		{
			Player p = Refs.p;

			if (p.HoldingCubi()) { ThrowCubiMain(vector); }
			else if (heldPillows > 0) { ThrowPillowMain(vector); }
			else { Refs.mf.Announce("You don't have anything to throw.", myAlign, myColor); }
		}

		private void ThrowCubiMain(Loc vector)
		{
			// todo lots of duplication here
			Player p = Refs.p;
			Cubi c = Harem.GetId(p.heldCubiId);

			Refs.mf.Announce("You throw " + c.name + " through the air!", myAlign, myColor);
			Refs.mf.Announce("*flap* *flap* *flap*", c.myAlign, c.myColor);
			c.beingCarried = false;
			p.heldCubiId = 0;

			// determine release point of throw
			// todo check for hit on very first tile
			Loc startloc = Loc.AddPts(this.loc, vector);
			MapTile activeTile = Refs.m.TileByLoc(startloc);

			// if the next tile is another cubi, throw short
			// todo consolidate long and short throws
			string moveClear = CheckClearForThrown(vector, activeTile);
			if (moveClear == "cubi")
			{
				MapTile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
				Cubi victim = Refs.m.CubiAt(victimTile.loc);

				Refs.mf.Announce("Owf!", c.myAlign, c.myColor);
				victim.Spank(5);

				Refs.mf.Announce("Oof too!", c.myAlign, c.myColor);
				c.Spank(5); // and a good time was had by both
			}

			// todo refresh player square so she's not superimposed on player square

			while (moveClear == "clear")
			{
				// blip activeTile with cubi symbol
				c.loc = activeTile.loc;
				AnimateMobile(activeTile, c);

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
						victim.loc = MapTile.RandomFromList(escapes).loc;
						Refs.mf.Announce("Nyahhh missed me!", victim.myAlign, victim.myColor);
						moveClear = "clear";
					}
					else
					{
						victim.Spank(5);
						Refs.mf.Announce("Owwwww!", victim.myAlign, victim.myColor);
					}
				}

				// just a wall. stop here.
				if (moveClear == "wall")
				{ Refs.mf.Announce("You didn't hit anything interesting.", myAlign, myColor); }

				// it's clear, so move activeTile up and iterate
				if (moveClear == "clear")
				{ activeTile = Refs.m.TileByLoc(Loc.AddPts(vector, activeTile.loc)); }
			}

			// deposit cubi here
			c.loc = activeTile.loc;

			Refs.mf.UpdateMap();
		}

		private void ThrowPillowMain(Loc vector)
		{
			Refs.mf.Announce("You throw a pillow!", myAlign, myColor);

			// can't throw without pillow!
			if (heldPillows <= 0)
			{ return; }
			else
			{ heldPillows--; UpdateInventory(); }

			// determine release point of throw
			// todo check for hit on very first tile -- where to put pillow?
			Loc startloc = Loc.AddPts(this.loc, vector);
			MapTile activeTile = Refs.m.TileByLoc(startloc);
			string pillowGlyph = "O";

			// if the next tile now is a lover, extra spank stun!
			// todo consolidate long and short throws
			string moveClear = CheckClearForThrown(vector, activeTile);
			if (moveClear == "cubi")
			{
				MapTile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
				Cubi victim = Refs.m.CubiAt(victimTile.loc);

				Refs.mf.Announce("POINT BLANK PILLOW SPANK!", myAlign, myColor);
				victim.Spank(5);
				Refs.mf.Announce("oww! *moan*", victim.myAlign, victim.myColor);
			}

			while (moveClear == "clear")
			{
				// blip activeTile with pillow symbol
				AnimatePillow(activeTile, pillowGlyph);

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
						victim.loc = MapTile.RandomFromList(escapes).loc;
						Refs.mf.Announce("Nyahhh missed me!", victim.myAlign, victim.myColor);
						moveClear = "clear";
					}
					else
					{
						victim.Spank(5);
						Refs.mf.Announce("Owwwww!", victim.myAlign, victim.myColor);
					}
				}

				// just a wall. stop here.
				if (moveClear == "wall")
				{ Refs.mf.Announce("You didn't hit anything interesting.", myAlign, myColor); }

				// it's clear, so move activeTile up and iterate
				if (moveClear == "clear")
				{ activeTile = Refs.m.TileByLoc(Loc.AddPts(vector, activeTile.loc)); }
			}
			// leave pillow on ground to form new obstruction
			activeTile.clear = false;
			Refs.m.HealWalls();
			Refs.mf.UpdateMap();
		}

		private void AnimatePillow(MapTile activeTile, string glyph)
		{
			// todo animation code is a bit makeshift and needs to be cleaned up andmoved to Map.cs
			activeTile.clear = false;
			activeTile.gly = glyph;

			Refs.mf.UpdateMap();

			Thread.Sleep(75);
			activeTile.clear = true;
			activeTile.gly = " ";

			Refs.mf.UpdateMap();
			// todo also fix nectar drops.
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

		private string CheckClearForThrown(Loc vector, MapTile activeTile)
		{
			Loc newloc = Loc.AddPts(vector, activeTile.loc);
			if (!Refs.m.TileByLoc(newloc).clear) return "wall";

			foreach (Cubi c in Refs.h.roster)
			{
				if (Refs.m.TileByLoc(newloc).loc == c.loc) { return "cubi"; }
			}

			return "clear";
		}
	}
}