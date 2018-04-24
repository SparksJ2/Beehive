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

		public HorizontalAlignment myAlign = HorizontalAlignment.Left;

		public Player(string name, Color useColor) : base(name, useColor)
		{
			glyph = "♂";
		}

		public int HandlePlayerInput(PreviewKeyDownEventArgs e)
		{
			MapTile here = Refs.m.TileByLoc(loc);

			// debugging nectar report
			Console.Write("Nectar here is ");
			foreach (int i in here.nectarLevel) { Console.Write(i + ", "); }
			Console.Write(".");

			if (e.KeyCode == Keys.F6)
			{
				Refs.mf.Announce("Saving game...", myAlign, myColor);
				SaveGame();
				return 0;
			}

			if (e.KeyCode == Keys.F9)
			{
				Refs.mf.Announce("Loading game...", myAlign, myColor);
				LoadGame();
				Refs.mf.Announce("Loaded game at " + turnCounter + " turns in.", myAlign, myColor);
				return 0;
			}

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

			Loc newpos = loc;
			lastMove = Loc.SubPts(newpos, lastPos);

			for (int nLoop = 1; nLoop < here.nectarLevel.Length; nLoop++) // actually skip player nectar
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
				// todo we're duplicating this location scanning code a lot...

				// get list of capture tiles
				MapTileSet jails = new MapTileSet();
				foreach (Loc l in Refs.m.pents) { jails.Add(Refs.m.TileByLoc(l)); }

				// get list of cubi locations
				MapTileSet breaker = new MapTileSet();
				foreach (Cubi c in Refs.h.roster) { breaker.Add(Refs.m.TileByLoc(c.loc)); }

				// IntersectWith to get occupied jails
				jails.IntersectWith(breaker);

				if (jails.Count == Refs.m.pents.Count)
				{
					victory = true;
					Refs.mf.Announce("Gotcha all! And in only " + turnCounter + " turns!", myAlign, myColor);
				}
			}

			return timepass;
		}

		private void SaveGame()
		{
			Stream TestFileStream = File.Create("map.bin");
			BinaryFormatter serializer = new BinaryFormatter();
			serializer.Serialize(TestFileStream, Refs.m);
			TestFileStream.Close();

			Stream TestFileStream2 = File.Create("player.bin");
			BinaryFormatter serializer2 = new BinaryFormatter();
			serializer2.Serialize(TestFileStream2, Refs.p);
			TestFileStream2.Close();

			Stream TestFileStream3 = File.Create("harem.bin");
			BinaryFormatter serializer3 = new BinaryFormatter();
			serializer3.Serialize(TestFileStream3, Refs.h);
			TestFileStream3.Close();
		}

		private void LoadGame()
		{
			string FileName = "map.bin";
			if (File.Exists(FileName))
			{
				Stream TestFileStream = File.OpenRead(FileName);
				BinaryFormatter deserializer = new BinaryFormatter();
				Refs.m = (MainMap)deserializer.Deserialize(TestFileStream);
				TestFileStream.Close();
			}

			FileName = "player.bin";
			if (File.Exists(FileName))
			{
				Stream TestFileStream = File.OpenRead(FileName);
				BinaryFormatter deserializer = new BinaryFormatter();
				Refs.p = (Player)deserializer.Deserialize(TestFileStream);
				TestFileStream.Close();
			}

			FileName = "harem.bin";
			if (File.Exists(FileName))
			{
				Stream TestFileStream = File.OpenRead(FileName);
				BinaryFormatter deserializer = new BinaryFormatter();
				Refs.h = (Harem)deserializer.Deserialize(TestFileStream);
				TestFileStream.Close();
			}
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