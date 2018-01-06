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
		public HorizontalAlignment myAlign = HorizontalAlignment.Left;

		public Player(string name, Color useColor) : base(name, useColor)
		{
		}

		public bool HandlePlayerInput(PreviewKeyDownEventArgs e)
		{
			bool timepass = true;
			if (e.Shift)
			{
				// time doesn't progress when moving pillows, for now
				timepass = false;
				switch (e.KeyCode)
				{
					case Keys.Down: case Keys.S: PlacePillowSouth(); break;
					case Keys.D: case Keys.Right: PlacePillowEast(); break;
					case Keys.Up: case Keys.W: PlacePillowNorth(); break;
					case Keys.Left: case Keys.A: PlacePillowWest(); break;
					case Keys.Space: break; // allow waiting
					default: timepass = false; break;
				}
			}
			else if (e.Control)
			{
				// time doesn't progress when throwing pillows either, for now
				timepass = false;
				switch (e.KeyCode)
				{
					case Keys.Down: case Keys.S: ThrowPillowSouth(); break;
					case Keys.D: case Keys.Right: ThrowPillowEast(); break;
					case Keys.Up: case Keys.W: ThrowPillowNorth(); break;
					case Keys.Left: case Keys.A: ThrowPillowWest(); break;
					case Keys.Space: break; // allow waiting
					default: timepass = false; break;
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
					default: timepass = false; break;
				}
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
				Refs.mf.Announce("POINT BLANK PILLOW SPANK!", myAlign, myColor);
				Refs.c.Spank(5);
				Refs.mf.Announce("oww! *moan*", Refs.c.myAlign, Refs.c.myColor);
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
					// todo only one possible target?
					Tile victimTile = Refs.m.TileByLoc(Refs.c.loc);
					HashSet<Tile> escapes = new HashSet<Tile>(new TileComp());

					if (IsVertical(vector))
					{
						escapes = victimTile.GetPossibleMoves(Dir.DodgeVertical());
					}
					else
					{
						escapes = victimTile.GetPossibleMoves(Dir.DodgeHorizontal());
					}

					if (escapes.Count > 0)
					{
						Refs.c.loc = Tile.RandomFromList(escapes).loc;
						Refs.mf.Announce("Nyahhh missed me!", Refs.c.myAlign, Refs.c.myColor);
						moveClear = "clear";
					}
					else
					{
						Refs.c.Spank(5);
						Refs.mf.Announce("Owwwww!", Refs.c.myAlign, Refs.c.myColor);
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
			if (Refs.m.TileByLoc(newloc).loc == Refs.c.loc) return "spank";
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

		private void ToggleClearTile(Tile t)
		{
			if (Refs.m.EdgeLoc(t.loc)) return;

			if (t.clear && heldPillows > 0)
			{
				t.clear = false;
				heldPillows--;
				UpdateInventory();
				Refs.mf.Announce("You place the pillow to make a wall. You have " + heldPillows + " left.", myAlign, myColor);
			}
			else if (!t.clear)
			{
				t.clear = true;
				heldPillows++;
				UpdateInventory();
				Refs.mf.Announce("You pick up a pillow. You now have " + heldPillows + ".", myAlign, myColor);
			}
		}

		private void UpdateInventory()
		{
			Refs.mf.miniInventory.Text = "pillows: " + heldPillows;
		}

		private void PlacePillowNorth()
		{
			Tile t = Refs.m.TileByLoc(loc).OneNorth();
			ToggleClearTile(t);
		}

		private void PlacePillowEast()
		{
			Tile t = Refs.m.TileByLoc(loc).OneEast();
			ToggleClearTile(t);
		}

		private void PlacePillowSouth()
		{
			Tile t = Refs.m.TileByLoc(loc).OneSouth();
			ToggleClearTile(t);
		}

		private void PlacePillowWest()
		{
			Tile t = Refs.m.TileByLoc(loc).OneWest();
			ToggleClearTile(t);
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