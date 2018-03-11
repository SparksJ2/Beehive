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

			// if the next tile now is our lover, extra spank stun!
			string moveClear = CheckClearForThrown(vector, activeTile);
			if (moveClear == "spank")
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
				Animate(activeTile, pillowGlyph);

				// is the next tile clear?
				moveClear = CheckClearForThrown(vector, activeTile);

				// nope, it has your cubi in.
				if (moveClear == "spank")
				{
					MapTile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
					Cubi victim = Refs.m.CubiAt(victimTile.loc);

					MapTileSet escapes = new MapTileSet();

					if (IsVertical(vector))
					{
						escapes = victimTile.GetPossibleMoves(Dir.DodgeVertical);
					}
					else
					{
						escapes = victimTile.GetPossibleMoves(Dir.DodgeHorizontal);
					}

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

		private void Animate(MapTile activeTile, string pillowGlyph)
		{
			// todo animation code is a bit makeshift and needs to be cleaned up andmoved to Map.cs
			activeTile.clear = false;
			activeTile.gly = pillowGlyph;

			Refs.mf.UpdateMap();

			Thread.Sleep(75);
			activeTile.clear = true;
			activeTile.gly = " ";
			Refs.mf.UpdateMap();
		}

		private string CheckClearForThrown(Loc vector, MapTile activeTile)
		{
			Loc newloc = Loc.AddPts(vector, activeTile.loc);
			if (!Refs.m.TileByLoc(newloc).clear) return "wall";

			foreach (Cubi c in Refs.h.roster)
			{
				if (Refs.m.TileByLoc(newloc).loc == c.loc) { return "spank"; }
			}

			return "clear";
		}
	}
}