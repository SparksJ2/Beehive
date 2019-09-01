using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Beehive
{
	public partial class MainForm : Form
	{
		public PreviewKeyDownEventHandler eh;
		public Stopwatch turnTimer;
		private Control defaultFocus;

		public MainForm()
		{
			InitializeComponent();

			// init key handlers
			turnTimer = new Stopwatch(); turnTimer.Start();

			this.KeyPreview = true;

			eh = new PreviewKeyDownEventHandler(PreviewKeyDownHandler);
			this.PreviewKeyDown += eh;

			// don't let the feedback / inventory windows become selected
			feedbackBox.Enter += DenyFocus;
			miniInventory.Enter += DenyFocus;

			Refs.mf = this;
			defaultFocus = this.ActiveControl;
		}

		private void DenyFocus(object sender, EventArgs e) => this.ActiveControl = null;

		private void MainForm_Shown(object sender, EventArgs e)
		{
			// generate map
			Refs.p = new Player("The Protagonist", Color.Cyan);
			Refs.p.SetXY(32, 12); // todo fix hardcoded numbers

			Refs.h = new Harem();

			Refs.m = new MazeGenerator().Create(65, 25);

			// draw initial map
			FlowMap.RemakeAllFlows();
			UpdateMap();
			GlowTest();
			HelpPopup();

			Player p = Refs.p;
			Cubi c = Refs.h.roster[0];
			Announce("Welcome to the underworld. Look out, they're getting away!", p.myAlign, p.myColor);
			Announce("You'll never catch meeee!", c.myAlign, c.myColor);
			Announce("We'll see about that!", p.myAlign, p.myColor);
			Announce("Whee! *giggle*", c.myAlign, c.myColor);

			c = Refs.h.roster[1];
			Announce("Run, Master! *nyhha!*", c.myAlign, c.myColor);

			c = Refs.h.roster[2];
			Announce("Chase me Master! *hehe*", c.myAlign, c.myColor);

			Refs.p.UpdateInventory();
		}

		private void HelpPopupEvent(object sender, EventArgs e) => HelpPopup();

		private void MenuSaveEvent(object sender, EventArgs e) => LoadAndSave.SaveGame();

		private void MenuLoadEvent(object sender, EventArgs e) => LoadAndSave.LoadGame();

		private void MenuClearNectarEvent(object sender, EventArgs e) => Cheats.ClearNectar();

		private void MenuTopOffEnergyEvent(object sender, EventArgs e) => Cheats.TopOffEnergy();

		private void HelpPopup()
		{
			MessageBox.Show(
				"In your vast bed, tucked deep in a dreamworld, far outside time and space,\n" +
				"you play in eternal bliss with your horned lovers.\n\n" +
				"But they have escaped their pentagrams...\n" +
				"\tcatch them and bring them back home for a good spanking!\n\n" +
				"Keys:\n" +
				"\tWASD or arrow keys to move.\n" +
				"\tShift+Direction to pick up or put down various things.\n" +
				"\tCtrl+Direction to throw pillows!\n\n" +
				"Alternate controls:\n\n" +
				"\t'P' then Direction to place/pickup.\n" +
				"\t'T' then Direction to throw.\n\n" +
				"F6 and F9 to quicksave and quickload (unlikely to work between version changes!)\n" +
				"F1 to view this help again.\n\n" +
				"\tBeta features:\n" +
				"\t\t'F' to boink held Cubi.\n" +
				"\t\t'C' to discipline them. ;)"
				);
		}

		private static void GlowTest()
		{
			// check glow system is commutative
			Color c1 = Color.SlateBlue;
			Color c2 = Color.HotPink;
			Color c3 = Color.FromArgb(0, 71, 171);

			Color m1a = MainMap.GlowColOffset(c1, c2, 0.5);
			Color m1b = MainMap.GlowColOffset(m1a, c3, 0.5);

			Color m2a = MainMap.GlowColOffset(c1, c3, 0.5);
			Color m2b = MainMap.GlowColOffset(m2a, c2, 0.5);

			//MessageBox.Show(m1b + "\n" + m2b);
		}

		public void UpdateMap()
		{
			Refs.mf.MainBitmap.Image = Refs.m.AsBitmap();
			Refs.mf.Refresh();
		}

		public void PreviewKeyDownHandler(object sender, PreviewKeyDownEventArgs e)
		{
			this.ActiveControl = defaultFocus;

			var sw = new Stopwatch(); sw.Start();
			Console.WriteLine("Key " + e.KeyCode + " Pressed");
			Console.WriteLine("Starting new frame.");
			try
			{
				if (turnTimer.ElapsedMilliseconds < 300) return;
				turnTimer.Start();

				int timePass = Refs.p.HandlePlayerInput(e);

				Refs.m.HealWalls();
				Console.WriteLine("Finished HealWalls at " + sw.ElapsedMilliseconds + "ms in.");

				Console.WriteLine("Finished RunLos at " + sw.ElapsedMilliseconds + "ms in.");

				FlowMap.RemakeAllFlows();
				Console.WriteLine("Finished RemakeAllFlows at " + sw.ElapsedMilliseconds + "ms in.");

				if (timePass == 0)
				{
					UpdateMap();
					Console.WriteLine("Finished UpdateMap at " + sw.ElapsedMilliseconds + "ms in.");
				}
				else
				{
					while (timePass > 0)
					{
						// run ai for multiple turns if needed
						foreach (Cubi c in Refs.h.roster) { c.AiMove(); }
						Console.WriteLine("Finished AiMove at " + sw.ElapsedMilliseconds + "ms in.");

						Refs.m.SpreadNectar();
						UpdateMap();
						Thread.Sleep(75);

						Console.WriteLine("Finished UpdateMap at " + sw.ElapsedMilliseconds + "ms in.");
						timePass--;
						Refs.p.turnCounter++;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Last chance catch of exception " + ex.GetType() +
					" with message " + ex.Message + " at " + ex.Source +
					" with trace " + ex.StackTrace);
			}

			Console.WriteLine("Total time this update = " + sw.ElapsedMilliseconds + "ms. or " +
						1000 / sw.ElapsedMilliseconds + " fps if it mattered.");

			this.ActiveControl = defaultFocus;
		}

		private List<AnnounceStruct> annLines;

		internal void Announce(string say, HorizontalAlignment align, Color col)
		{
			try
			{
				if (annLines == null) annLines = new List<AnnounceStruct>();

				annLines.Add(new AnnounceStruct(say, align, col));
				if (annLines.Count > 10) annLines.RemoveAt(0);

				//feedbackBox.Text = "";
				feedbackBox.Clear();

				int max = annLines.Count;
				for (int i = 0; i < max; i++)
				{
					feedbackBox.FancyAppendText(annLines[i].say + "\n",
						annLines[i].color, annLines[i].align);
				}
				Refresh();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Caught exception in Announce(), " + ex.ToString() +
					" with message " + ex.Message);
			}
		}
	}
}