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
		private bool throwmode, placemode;

		public HorizontalAlignment myAlign = HorizontalAlignment.Left;

		public Player(string name, Color useColor) : base(name, useColor)
		{
			glyph = "♂";
		}

		public int HandlePlayerInput(PreviewKeyDownEventArgs e)
		{
			// returns number of round passed, 0 for free actions, 1 for normal moves.

			Loc lastPos = loc;
			// visualise flows
			if (e.KeyCode == Keys.D0) { viewFlow = 0; return 0; }
			if (e.KeyCode == Keys.D1) { viewFlow = 1; return 0; }
			if (e.KeyCode == Keys.D2) { viewFlow = 2; return 0; }
			if (e.KeyCode == Keys.D3) { viewFlow = 3; return 0; }
			if (e.KeyCode == Keys.D4) { viewFlow = 4; return 0; }

			int timepass = 1;
			if (e.KeyCode == Keys.Space)
			{
				return 1; // allow waiting at any time
			}

			if (placemode || e.Shift)
			{
				timepass = 0; // place / pickup is a free action for now
				switch (e.KeyCode)
				{
					case Keys.Down: case Keys.S: PlaceItemSouth(); FinishMode(); break;
					case Keys.Right: case Keys.D: PlaceItemEast(); FinishMode(); break;
					case Keys.Up: case Keys.W: PlaceItemNorth(); FinishMode(); break;
					case Keys.Left: case Keys.A: PlaceItemWest(); FinishMode(); break;
					case Keys.Escape: CancelModes(); break;
					default: break;
				}
			}
			else if (throwmode || e.Control)
			{
				timepass = 0; // throw is a free action for now
				switch (e.KeyCode)
				{
					case Keys.Down: case Keys.S: ThrowPillowSouth(); FinishMode(); break;
					case Keys.Right: case Keys.D: ThrowPillowEast(); FinishMode(); break;
					case Keys.Up: case Keys.W: ThrowPillowNorth(); FinishMode(); break;
					case Keys.Left: case Keys.A: ThrowPillowWest(); FinishMode(); break;
					case Keys.Escape: CancelModes(); break;
					default: break;
				}
			}
			else
			{
				timepass = 1;
				switch (e.KeyCode)
				{
					// moves cost 1 turn
					case Keys.Down: case Keys.S: RunSouth(); break;
					case Keys.Right: case Keys.D: RunEast(); break;
					case Keys.Up: case Keys.W: RunNorth(); break;
					case Keys.Left: case Keys.A: RunWest(); break;

					// mode changes are free actions
					case Keys.T: SetThrowMode(); timepass = 0; break;
					case Keys.P: SetPlaceMode(); timepass = 0; break;
					case Keys.Escape: CancelModes(); timepass = 0; break;

					default: timepass = 0; break;
				}
			}

			MapTile here = Refs.m.TileByLoc(loc);

			Loc newpos = loc;
			lastMove = Loc.SubPts(newpos, lastPos);

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

		private void FinishMode()
		{
			Refs.mf.Announce("Back to the chase!", myAlign, myColor);
			placemode = false;
			throwmode = false;
		}

		private void CancelModes()
		{
			placemode = false;
			throwmode = false;
			Refs.mf.Announce("Never mind! Back to the chase!", myAlign, myColor);
		}

		private void SetPlaceMode()
		{
			placemode = true;
			Refs.mf.Announce("Place/pickup where? (esc to cancel)", myAlign, myColor);
		}

		private void SetThrowMode()
		{
			throwmode = true;
			Refs.mf.Announce("Throw which way? (esc to cancel)", myAlign, myColor);
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