using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
					case Keys.Down: PillowSouth(); break;
					case Keys.Right: PillowEast(); break;
					case Keys.Up: PillowNorth(); break;
					case Keys.Left: PillowWest(); break;
					case Keys.S: PillowSouth(); break;
					case Keys.D: PillowEast(); break;
					case Keys.W: PillowNorth(); break;
					case Keys.A: PillowWest(); break;
					case Keys.Space: break; // allow waiting
					default: timepass = false; break;
				}
			}
			else
			{
				switch (e.KeyCode)
				{
					case Keys.Down: South(); break;
					case Keys.Right: East(); break;
					case Keys.Up: North(); break;
					case Keys.Left: West(); break;
					case Keys.S: South(); break;
					case Keys.D: East(); break;
					case Keys.W: North(); break;
					case Keys.A: West(); break;
					case Keys.Space: break; // allow waiting
					default: timepass = false; break;
				}
			}
			return timepass;
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
			// update screen
			// todo de-duplicate?
			map.HealWalls();
		}

		private void UpdateInventory()
		{
			mf.miniInventory.Text = "pillows: " + heldPillows;
		}

		private void PillowNorth()
		{
			Tile t = map.OneNorth(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void PillowEast()
		{
			Tile t = map.OneEast(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void PillowSouth()
		{
			Tile t = map.OneSouth(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void PillowWest()
		{
			Tile t = map.OneWest(map.TileByLoc(loc));
			ToggleClearTile(t);
		}

		private void North()
		{
			Tile t = map.OneNorth(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		private void East()
		{
			Tile t = map.OneEast(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		private void South()
		{
			Tile t = map.OneSouth(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		private void West()
		{
			Tile t = map.OneWest(map.TileByLoc(loc));
			if (t.clear) loc = t.loc;
		}

		// todo fix 'public'?
	}
}