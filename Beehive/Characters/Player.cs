using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Beehive
{
	[Serializable()]
	public partial class Player : Mobile
	{
		public int heldPillows = 0;
		public int heldCubiId = 0;
		public int viewFlow = 0;
		public Loc lastMove;
		private bool throwmode = false, placemode = false, victory = false;
		public int turnCounter = 0;

		public Player(string name, Color useColor) : base(name, useColor)
		{
			glyph = "♂";
			myAlign = HorizontalAlignment.Left;
		}

		// returns number of round passed, 0 for free actions, 1 for normal moves.
		public int HandlePlayerInput(PreviewKeyDownEventArgs e)
		{
			// for convenience
			MapTile here = Refs.m.TileByLoc(loc);

			// debugging nectar report
			Console.Write("Nectar here is ");
			foreach (int i in here.nectarLevel) { Console.Write(i + ", "); }
			Console.Write(".");

			if (e.KeyCode == Keys.F && heldCubiId != 0)
			{
				return BoinkHeld();
			}

			if (e.KeyCode == Keys.C && heldCubiId != 0)
			{
				return CaneHeld();
			}

			Loc lastPos = loc;
			// visualise flows. hotkeys are just pretend this is where we really do it
			if (e.KeyCode == Keys.D0) { viewFlow = 0; return 0; }
			if (e.KeyCode == Keys.D1) { viewFlow = 1; return 0; }
			if (e.KeyCode == Keys.D2) { viewFlow = 2; return 0; }
			if (e.KeyCode == Keys.D3) { viewFlow = 3; return 0; }
			if (e.KeyCode == Keys.D4) { viewFlow = 4; return 0; }

			if (e.KeyCode == Keys.D5)
			{
				Refs.m.flipRenderMode = !Refs.m.flipRenderMode;
				Refs.m.FlushTileBitmapCache();
			}

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
					case Keys.Down: case Keys.S: ActionSouth(); FinishMode(); break;
					case Keys.Right: case Keys.D: ActionEast(); FinishMode(); break;
					case Keys.Up: case Keys.W: ActionNorth(); FinishMode(); break;
					case Keys.Left: case Keys.A: ActionWest(); FinishMode(); break;
					case Keys.Escape: CancelModes(); break;
					default: break;
				}
			}
			else if (throwmode || e.Control)
			{
				timepass = 0; // throw is a free action for now
				switch (e.KeyCode)
				{
					case Keys.Down: case Keys.S: ThrowSouth(); FinishMode(); break;
					case Keys.Right: case Keys.D: ThrowEast(); FinishMode(); break;
					case Keys.Up: case Keys.W: ThrowNorth(); FinishMode(); break;
					case Keys.Left: case Keys.A: ThrowWest(); FinishMode(); break;
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

			// save our current location for next turn
			lastMove = Loc.SubPts(loc, lastPos);

			// starting at 1 skips player nectar processing for now
			for (int nLoop = 1; nLoop < here.nectarLevel.Length; nLoop++)
			{
				if (here.nectarLevel[nLoop] > 0)
				{
					horny += here.nectarLevel[nLoop];
					here.nectarLevel[nLoop] = 0;
				}
			}

			if (horny > 15) // having fun
			{
				Refs.mf.Announce("Awwww yeah! *splurt*", myAlign, myColor);
				timepass += 5;
				MainMap.SplurtNectar(here, myIndex: 0);
				horny = 0;
			}

			if (!victory)
			{
				// we're duplicating this location scanning code a lot...
				// but this will be useful if we ever move jails so I'll leave it

				// get list of capture tiles
				MapTileSet jails = new MapTileSet();
				foreach (Loc l in Refs.m.pents) { jails.Add(Refs.m.TileByLoc(l)); }

				// get list of cubi locations
				MapTileSet breaker = new MapTileSet();
				foreach (Cubi c in Refs.h.roster) { breaker.Add(Refs.m.TileByLoc(c.loc)); }

				// IntersectWith to get occupied jails
				jails.IntersectWith(breaker);

				// if jails filled = total jails, we won!
				if (jails.Count == Refs.m.pents.Count)
				{
					victory = true;
					Refs.mf.Announce("Gotcha all! And in only " + turnCounter + " turns!", myAlign, myColor);
				}
			}

			return timepass;
		}

		private void FinishMode()
		{
			//Refs.mf.Announce("Back to the chase!", myAlign, myColor);
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
				"succubi: " + (heldCubiId == 0 ? 0 : 1);
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