using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace Beehive
{
	public class Player : Mobile
	{
		public int heldPillows = 0;
		public int heldCubiId = 0;

		public HorizontalAlignment myAlign = HorizontalAlignment.Left;

		public Player(string name, Color useColor) : base(name, useColor)
		{
			glyph = "☿";
		}

		public int HandlePlayerInput(PreviewKeyDownEventArgs e)
		{
			int timepass = 1;
			if (e.Shift)
			{
				// time doesn't progress when moving pillows, for now
				timepass = 0;
				switch (e.KeyCode)
				{
					case Keys.Down: case Keys.S: PlaceItemSouth(); break;
					case Keys.D: case Keys.Right: PlaceItemEast(); break;
					case Keys.Up: case Keys.W: PlaceItemNorth(); break;
					case Keys.Left: case Keys.A: PlaceItemWest(); break;
					case Keys.Space: break; // allow waiting
					default: timepass = 0; break;
				}
			}
			else if (e.Control)
			{
				// time doesn't progress when throwing pillows either, for now
				timepass = 0;
				switch (e.KeyCode)
				{
					case Keys.Down: case Keys.S: ThrowPillowSouth(); break;
					case Keys.D: case Keys.Right: ThrowPillowEast(); break;
					case Keys.Up: case Keys.W: ThrowPillowNorth(); break;
					case Keys.Left: case Keys.A: ThrowPillowWest(); break;
					case Keys.Space: break; // allow waiting
					default: timepass = 0; break;
				}
			}
			else
			{
				switch (e.KeyCode)
				{
					case Keys.S: case Keys.Down: RunSouth(); break;
					case Keys.D: case Keys.Right: RunEast(); break;
					case Keys.W: case Keys.Up: RunNorth(); break;
					case Keys.A: case Keys.Left: RunWest(); break;
					case Keys.Space: break; // allow waiting
					default: timepass = 0; break;
				}
			}

			Tile here = Refs.m.TileByLoc(loc);

			if (here.hasNectar && here.nectarCol != myColor) // yum
			{
				horny++;
				here.hasNectar = false;
			}

			if (horny > 15) // having fun
			{
				Refs.mf.Announce("Awwww yeah! *splurt*", myAlign, myColor);
				timepass += 5;
				Map.SplurtNectar(here, myColor);
				horny = 0;
			}

			return timepass;
		}

		private void ThrowPillow(Loc vector)
		{
			Refs.mf.Announce("You throw a pillow!", myAlign, myColor);
			// can't throw without pillow!
			if (heldPillows <= 0)
			{ return; }
			else
			{ heldPillows--; UpdateInventory(); }

			// determine release point of throw
			Loc startloc = Loc.AddPts(this.loc, vector);
			Tile activeTile = Refs.m.TileByLoc(startloc);
			string pillowGlyph = "O";

			// if the next tile now is our lover, extra spank stun!
			string moveClear = CheckClearForThrown(vector, activeTile);
			if (moveClear == "spank")
			{
				Tile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
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
					Tile victimTile = Refs.m.TileByLoc(Loc.AddPts(activeTile.loc, vector));
					Cubi victim = Refs.m.CubiAt(victimTile.loc);

					HashSet<Tile> escapes = new HashSet<Tile>(new TileComp());

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
						victim.loc = Tile.RandomFromList(escapes).loc;
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

		private string CheckClearForThrown(Loc vector, Tile activeTile)
		{
			Loc newloc = Loc.AddPts(vector, activeTile.loc);
			if (!Refs.m.TileByLoc(newloc).clear) return "wall";

			foreach (Cubi c in Refs.h.roster)
			{
				if (Refs.m.TileByLoc(newloc).loc == c.loc) { return "spank"; }
			}

			return "clear";
		}

		private void Animate(Tile activeTile, string pillowGlyph)
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

		private void ThrowPillowNorth()
		{
			Tile t = Refs.m.TileByLoc(this.loc).OneNorth();
			if (t.clear) ThrowPillow(Dir.North);
		}

		private void ThrowPillowWest()
		{
			Tile t = Refs.m.TileByLoc(this.loc).OneWest();
			if (t.clear) ThrowPillow(Dir.West);
		}

		private void ThrowPillowEast()
		{
			Tile t = Refs.m.TileByLoc(this.loc).OneEast();
			if (t.clear) ThrowPillow(Dir.East);
		}

		private void ThrowPillowSouth()
		{
			Tile t = Refs.m.TileByLoc(this.loc).OneSouth();
			if (t.clear) ThrowPillow(Dir.South);
		}

		private void PlaceItemOnTile(Tile t)
		{
			if (Refs.m.EdgeLoc(t.loc)) return;

			if (heldCubiId > 0 && t.clear) // put down lover
			{
				Cubi myHeldCubi = Refs.h.GetId(heldCubiId);

				myHeldCubi.loc = t.loc;
				myHeldCubi.beingCarried = false;

				heldCubiId = 0;
				Refs.mf.Announce("You're free to go... if you can.", myAlign, myColor);
			}
			else if (heldCubiId == 0 && Refs.m.ContainsCubi(t.loc)) // pick up lover
			{
				Cubi caughtCubi = Refs.m.CubiAt(t.loc);
				caughtCubi.loc = this.loc;
				caughtCubi.beingCarried = true;
				heldCubiId = caughtCubi.IdNo;

				Refs.mf.Announce("Gotcha!", myAlign, myColor);
				Refs.mf.Announce("EEEK!!", caughtCubi.myAlign, caughtCubi.myColor);
			}
			else if (t.clear && heldPillows > 0)
			{
				t.clear = false;
				heldPillows--;
				Refs.mf.Announce("You place the pillow to make a wall. You have " + heldPillows + " left.", myAlign, myColor);
			}
			else if (!t.clear)
			{
				t.clear = true;
				heldPillows++;
				Refs.mf.Announce("You pick up a pillow. You now have " + heldPillows + ".", myAlign, myColor);
			}
			UpdateInventory();
		}

		public void UpdateInventory()
		{
			Refs.mf.miniInventory.Text =
				"pillows: " + heldPillows + "\n" +
				"succubi: " + heldCubiId;
		}

		private void PlaceItemNorth()
		{
			Tile t = Refs.m.TileByLoc(loc).OneNorth();
			PlaceItemOnTile(t);
		}

		private void PlaceItemEast()
		{
			Tile t = Refs.m.TileByLoc(loc).OneEast();
			PlaceItemOnTile(t);
		}

		private void PlaceItemSouth()
		{
			Tile t = Refs.m.TileByLoc(loc).OneSouth();
			PlaceItemOnTile(t);
		}

		private void PlaceItemWest()
		{
			Tile t = Refs.m.TileByLoc(loc).OneWest();
			PlaceItemOnTile(t);
		}

		private void RunNorth()
		{
			Tile t = Refs.m.TileByLoc(loc).OneNorth();
			if (t.clear) loc = t.loc;
		}

		private void RunEast()
		{
			Tile t = Refs.m.TileByLoc(loc).OneEast();
			if (t.clear) loc = t.loc;
		}

		private void RunSouth()
		{
			Tile t = Refs.m.TileByLoc(loc).OneSouth();
			if (t.clear) loc = t.loc;
		}

		private void RunWest()
		{
			Tile t = Refs.m.TileByLoc(loc).OneWest();
			if (t.clear) loc = t.loc;
		}

		private bool IsVertical(Loc v)
		{
			return v.Y != 0;
		}
	}
}