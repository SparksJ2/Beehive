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

		public Player(MainForm f, Map m) : base(f, m)
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
			if (heldPillows <= 0)
			{ return; }
			else
			{ heldPillows--; UpdateInventory(); }

			Point startloc = AddPts(this.loc, vector);
			Tile activeTile = map.TileByLoc(startloc);
			char pillowGlyph = 'O';

			while (map.TileByLoc(AddPts(vector, activeTile.loc)).clear)
			{
				Animate(activeTile, pillowGlyph);
				activeTile = map.TileByLoc(AddPts(vector, activeTile.loc));
			}
			activeTile.clear = false;
			map.HealWalls();
			mf.MainBitmap.Image = map.AsBitmap();
			mf.Refresh();
		}

		private void Animate(Tile activeTile, char pillowGlyph)
		{
			activeTile.clear = false;
			activeTile.gly = pillowGlyph;
			mf.MainBitmap.Image = map.AsBitmap();
			mf.Refresh();
			Thread.Sleep(75);
			activeTile.clear = true;
			activeTile.gly = ' ';
			mf.MainBitmap.Image = map.AsBitmap();
			mf.Refresh();
		}

		private void ThrowPillowNorth()
		{
			if (map.OneNorth(map.TileByLoc(this.loc)).clear) ThrowPillow(new Point(0, -1));
		}

		private void ThrowPillowWest()
		{
			if (map.OneWest(map.TileByLoc(this.loc)).clear) ThrowPillow(new Point(-1, 0));
		}

		private void ThrowPillowEast()
		{
			if (map.OneEast(map.TileByLoc(this.loc)).clear) ThrowPillow(new Point(1, 0));
		}

		private void ThrowPillowSouth()
		{
			if (map.OneSouth(map.TileByLoc(this.loc)).clear) ThrowPillow(new Point(0, 1));
		}

		private void TogglePillowMode()
		{
			pillowMode = !pillowMode;
		}

		private void ToggleClearTile(Tile t)
		{
			if (map.IsEdge(t.loc)) return;

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
			mf.miniInventory.Text = "pillows: " + heldPillows;
		}

		private void PlacePillowNorth()
		{
			Tile t = map.OneNorth(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void PlacePillowEast()
		{
			Tile t = map.OneEast(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void PlacePillowSouth()
		{
			Tile t = map.OneSouth(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void PlacePillowWest()
		{
			Tile t = map.OneWest(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void RunNorth()
		{
			Tile t = map.OneNorth(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		private void RunEast()
		{
			Tile t = map.OneEast(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		private void RunSouth()
		{
			Tile t = map.OneSouth(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		private void RunWest()
		{
			Tile t = map.OneWest(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		// todo fix 'public'?
	}
}