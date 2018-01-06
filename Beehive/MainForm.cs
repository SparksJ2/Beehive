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

			// init key handlers
			turnTimer = new Stopwatch(); turnTimer.Start();

			this.KeyPreview = true;

			eh = new PreviewKeyDownEventHandler(PreviewKeyDownHandler);
			this.PreviewKeyDown += eh;

			// don't let the feedback / inventory windows become selected
			feedbackBox.Enter += DenyFocus;
			miniInventory.Enter += DenyFocus;

			Refs.mf = this;
		}

		private void DenyFocus(object sender, EventArgs e)
		{
			this.ActiveControl = null;
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			// generate map
			Refs.p = new Player("The Protagonist", Color.Cyan);
			Refs.p.SetXY(32, 12); // todo fix hardcoded numbers
			Refs.c = new Cubi("Ai'nana", Color.HotPink);
			Refs.c.SetXY(33, 9);    // todo fix hardcoded numbers

			Refs.m = new MazeGenerator().Create(65, 25);

			// draw initial map
			new Flow().RemakeFlow(Refs.p.loc);
			UpdateMap();

			MessageBox.Show(
				"In your vast bed, tucked deep in a dreamworld, far outside time and space,\n" +
				"you play in eternal bliss with your horned lover.\n\n" +
				"But she has escaped her pentagram... catch her and give her a good spanking!\n\n" +
				"Keys:\n" +
				"\tWASD or arrow keys to move.\n" +
				"\tShift+Direction to pick up or put down pillows.\n" +
				"\tCtrl+Direction to throw pillows!\n");

			Player p = Refs.p;
			Cubi c = Refs.c;
			Announce("Welcome to the underworld. Look out, she's getting away!", p.myAlign, p.myColor);
			Announce("You'll never catch meeee!", c.myAlign, c.myColor);
			Announce("We'll see about that!", p.myAlign, p.myColor);
			Announce("Whee! *giggle*", c.myAlign, c.myColor);
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

		private List<AnnounceLine> annLines;

		internal void Announce(string say, HorizontalAlignment align, Color col)
		{
			try
			{
				if (annLines == null) annLines = new List<AnnounceLine>();

				annLines.Add(new AnnounceLine(say, align, col));
				if (annLines.Count > 6) annLines.RemoveAt(0);

				//feedbackBox.Text = "";
				feedbackBox.Clear();

				int max = annLines.Count;
				for (int i = 0; i < max; i++)
				{
					//// putting the color change first fixes exception in Mono... sometimes
					////    ¯\_(ツ)_/¯
					//feedbackBox.SelectionColor = annLines[i].color;
					//feedbackBox.SelectionAlignment = annLines[i].align;

					//// usage as in https://msdn.microsoft.com/en-us/library/system.windows.forms.richtextbox.selectionalignment(v=vs.110).aspx
					//// so it really should work
					//feedbackBox.SelectedText = (annLines[i].say + "\n");
					FancyAppendText(feedbackBox, annLines[i].say + "\n", annLines[i].color, annLines[i].align);
				}
				Refresh();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Caught exception " + ex.ToString() +
					" with message " + ex.Message);
			}
		}

		private bool alreadyToldYou = false;

		public void FancyAppendText(RichTextBox box, string text, Color color, HorizontalAlignment align)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			try { box.SelectionColor = color; }
			catch (Exception ex)
			{
				// Mono just won't play nice with RichTextBox colors so...
				////    ¯\_(ツ)_/¯
				if (!alreadyToldYou) { Console.WriteLine("SelectionColor fail: " + ex.Message); alreadyToldYou = true; }
			}

			box.SelectionAlignment = align;
			box.AppendText(text);

			try { box.SelectionColor = box.ForeColor; }
			catch (Exception ex)
			{
				if (!alreadyToldYou) { Console.WriteLine("SelectionColor fail: " + ex.Message); alreadyToldYou = true; }
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