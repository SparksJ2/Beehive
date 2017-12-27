using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace Beehive
{
	public partial class MainForm : Form
	{
		public PreviewKeyDownEventHandler eh;

		public Stopwatch turnTimer;

		public MainForm()
		{
			InitializeComponent();

			// generate map
			Refs.m = new Map(65, 25); // hacky tmp map for utils access
			Refs.m = new MazeGenerator().Create(65, 25);
			Refs.p = new Player();
			Refs.p.SetXY(1, 1);
			Refs.c = new Cubi();
			Refs.c.SetXY(4, 3);     //s.SetXY(65 - 2, 25 - 2);
			Refs.mf = this;

			// draw initial map
			new Flow().RemakeFlow(Refs.p.loc);
			UpdateMap();

			// init key handlers
			turnTimer = new Stopwatch(); turnTimer.Start();

			this.KeyPreview = true;

			eh = new PreviewKeyDownEventHandler(PreviewKeyDownHandler);
			this.PreviewKeyDown += eh;

			// don't let the feedback / inventory windows become selected
			feedbackBox.Enter += DenyFocus;
			miniInventory.Enter += DenyFocus;
		}

		private void DenyFocus(object sender, EventArgs e)
		{
			this.ActiveControl = null;
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			MessageBox.Show(
				"In your vast bed, tucked deep in a dreamworld, far outside time and space,\n" +
				"you play in eternal bliss with your horned lover.\n\n" +
				"But she has escaped her pentagram... catch her and give her a good spanking!\n\n" +
				"Keys:\n" +
				"\tWASD or arrow keys to move.\n" +
				"\tShift+Direction to pick up or put down pillows.\n" +
				"\tCtrl+Direction to throw pillows!\n");

			this.Announce("Welcome to the underworld. Look out, she's getting away!", Dir.Left);
			this.Announce("You'll never catch meeee!", Dir.Right);
			this.Announce("We'll see about that!", Dir.Left);
			this.Announce("Whee! *giggle*", Dir.Right);
		}

		public void UpdateMap()
		{
			Refs.mf.MainBitmap.Image = Refs.m.AsBitmap();
			Refs.mf.Refresh();
		}

		public void PreviewKeyDownHandler(object sender, PreviewKeyDownEventArgs e)
		{
			if (turnTimer.ElapsedMilliseconds < 300) return;
			turnTimer.Start();
			Console.WriteLine(e.KeyCode);

			bool timePass = Refs.p.HandlePlayerInput(e);
			new Flow().RemakeFlow(Refs.p.loc);
			if (timePass)
			{
				// check win condition first
				if (Refs.m.Touching(Refs.p, Refs.c)) { MessageBox.Show("Winners you are!"); }

				// run ai
				Refs.c.AiMove();
			}

			// update screen
			Refs.m.HealWalls();
			UpdateMap();
		}

		private List<string> feedbacks;
		private List<bool> aligns;

		internal void Announce(string v, bool a)
		{
			try
			{
				if (feedbacks == null)
				{
					feedbacks = new List<string>();
					aligns = new List<bool>();
				}

				feedbacks.Add(v);
				aligns.Add(a);

				if (feedbacks.Count > 6)
				{
					feedbacks.RemoveAt(0);
					aligns.RemoveAt(0);
				}

				feedbackBox.Text = "";

				int max = feedbacks.Count;
				for (int i = 0; i < max; i++)
				{
					if (aligns[i] == Dir.Right)
					{
						feedbackBox.SelectionAlignment = HorizontalAlignment.Right;
						feedbackBox.SelectionColor = Color.HotPink;
					}
					else
					{
						feedbackBox.SelectionAlignment = HorizontalAlignment.Left;
						feedbackBox.SelectionColor = Color.Cyan;
					}
					feedbackBox.AppendText(feedbacks[i] + "\n");
				}
				Refresh();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Caught exception " + ex.ToString() +
					" with message " + ex.Message);
			}
		}
	}
}

//public void KeyDownHandler(object sender, KeyEventArgs e)
//{
//	//e.SuppressKeyPress = isKeyPressed;
//	//if (isKeyPressed) isKeyHeld = true;
//	isKeyPressed = true;
//}

//public void KeyPressHandler(object sender, KeyPressEventArgs e)
//{
//	e.Handled = true;
//}

//public void KeyUpHandler(object sender, KeyEventArgs e)
//{
//	//isKeyPressed = false;
//	//isKeyHeld = false;
//}

//this.KeyPress += new KeyPressEventHandler(KeyPressHandler);
//this.KeyDown += new KeyEventHandler(KeyDownHandler);
//this.KeyPress += new KeyPressEventHandler(KeyPressHandler);
//this.KeyUp += new KeyEventHandler(KeyUpHandler);