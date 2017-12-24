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
		public bool pillowMode = false;
		public int heldPillows = 0;

		public Player() : base()
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

		private Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		private void ThrowPillow(Point vector)
		{
			// can't throw without pillow!
			if (heldPillows <= 0)
			{ return; }
			else
			{ heldPillows--; UpdateInventory(); }

			// determine release point of throw
			Point startloc = AddPts(this.loc, vector);
			Tile activeTile = Refs.m.TileByLoc(startloc);
			char pillowGlyph = 'O';

			// if the next tile now is our lover, extra spank stun!
			string moveClear = CheckClearForThrown(vector, activeTile);
			if (moveClear == "spank")
			{ Refs.c.spanked += 5; Console.WriteLine("POINT BLANK PILLOW SPANK!"); }

			while (moveClear == "clear")
			{
				// blip activeTile with pillow symbol
				Animate(activeTile, pillowGlyph);

				// is the next tile clear?
				moveClear = CheckClearForThrown(vector, activeTile);

				// nope, it has your cubi in. spank!
				if (moveClear == "spank")
				{ Refs.c.spanked += 3; Console.WriteLine("PILLOW SPANK!"); }

				// just a wall. stop here.
				if (moveClear == "wall")
				{ Console.WriteLine("thump"); }

				// it's clear, so move activeTile up and iterate
				if (moveClear == "clear")
				{ activeTile = Refs.m.TileByLoc(AddPts(vector, activeTile.loc)); }
			}
			// leave pillow on ground to form new obstruction
			activeTile.clear = false;
			Refs.m.HealWalls();
			Refs.mf.UpdateMap();
		}

		private string CheckClearForThrown(Point vector, Tile activeTile)
		{
			Point newloc = AddPts(vector, activeTile.loc);
			if (!Refs.m.TileByLoc(newloc).clear) return "wall";
			if (Refs.m.TileByLoc(newloc).loc == Refs.c.loc) return "spank";
			return "clear";
		}

		private void Animate(Tile activeTile, char pillowGlyph)
		{
			// todo animation code is a bit makeshift and needs to be cleaned up andmoved to Map.cs
			activeTile.clear = false;
			activeTile.gly = pillowGlyph;

			Refs.mf.UpdateMap();

			Thread.Sleep(75);
			activeTile.clear = true;
			activeTile.gly = ' ';
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

		private void TogglePillowMode()
		{
			pillowMode = !pillowMode;
		}

		private void ToggleClearTile(Tile t)
		{
			if (Refs.m.IsEdge(t.loc)) return;

			if (t.clear && heldPillows > 0)
			{
				t.clear = false;
				heldPillows--;
				UpdateInventory();
			}
			else if (!t.clear)
			{
				t.clear = true;
				heldPillows++;
				UpdateInventory();
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

		// todo fix 'public'?
	}
}