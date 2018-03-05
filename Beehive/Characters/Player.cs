using System.Drawing;
using System.Windows.Forms;

namespace Beehive
{
	public partial class Player : Mobile
	{
		public int heldPillows = 0;
		public int heldCubiId = 0;
		public int viewFlow = 0;
		public Loc lastMove;

		public HorizontalAlignment myAlign = HorizontalAlignment.Left;

		public Player(string name, Color useColor) : base(name, useColor)
		{
			glyph = "♂";
		}

		public int HandlePlayerInput(PreviewKeyDownEventArgs e)
		{
			Loc lastPos = loc;
			// visualise flows
			if (e.KeyCode == Keys.D0) { viewFlow = 0; return 0; }
			if (e.KeyCode == Keys.D1) { viewFlow = 1; return 0; }
			if (e.KeyCode == Keys.D2) { viewFlow = 2; return 0; }
			if (e.KeyCode == Keys.D3) { viewFlow = 3; return 0; }
			if (e.KeyCode == Keys.D4) { viewFlow = 4; return 0; }

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

			MapTile here = Refs.m.TileByLoc(loc);

			Loc newpos = loc;
			this.lastMove = Loc.SubPts(newpos, lastPos);

			if (here.hasNectar && here.nectarCol != myColor) // yum
			{
				horny++;
				here.hasNectar = false;
			}

			if (horny > 15) // having fun
			{
				Refs.mf.Announce("Awwww yeah! *splurt*", myAlign, myColor);
				timepass += 5;
				MainMap.SplurtNectar(here, myColor);
				horny = 0;
			}

			return timepass;
		}

		public void UpdateInventory()
		{
			Refs.mf.miniInventory.Text =
				"pillows: " + heldPillows + "\n" +
				"succubi: " + heldCubiId;
		}

		private void RunNorth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneNorth();
			if (t.clear) loc = t.loc;
		}

		private void RunEast()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneEast();
			if (t.clear) loc = t.loc;
		}

		private void RunSouth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneSouth();
			if (t.clear) loc = t.loc;
		}

		private void RunWest()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneWest();
			if (t.clear) loc = t.loc;
		}

		private bool IsVertical(Loc v) => v.Y != 0;
	}
}