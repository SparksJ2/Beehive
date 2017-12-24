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
		public KeyEventHandler kh;
		public PreviewKeyDownEventHandler eh;

		public Stopwatch turnTimer;

		public MainForm()
		{
			InitializeComponent();

			// generate map
			Refs.m = new MazeGenerator().Create(65, 25);
			Refs.p = new Player();
			Refs.p.SetXY(1, 1);
			Refs.c = new Cubi();
			Refs.c.SetXY(4, 3);     //s.SetXY(65 - 2, 25 - 2);
			Refs.mf = this;

			// draw initial map
			new Flow().RemakeFlow(Refs.p.loc);
			var bitMapMap = Refs.m.AsBitmap();

			// add to window
			MainBitmap.Image = bitMapMap;

			// init key handlers
			turnTimer = new Stopwatch(); turnTimer.Start();

			MessageBox.Show(
				"In your vast bed, tucked deep in a dreamworld, far outside time and space,\n" +
				"you play in eternal bliss with your horned lover.\n\n" +
				"But she has escaped her pentagram... catch her and give her a good spanking!\n\n" +
				"Keys:\n" +
				"\tWASD or arrow keys to move.\n" +
				"\tShift+Direction to pick up or put down pillows.\n" +
				"\tCtrl+Direction to throw pillows!\n");

			this.KeyPreview = true;

			eh = new PreviewKeyDownEventHandler(PreviewKeyDownHandler);
			this.PreviewKeyDown += eh;
		}

		//private bool isKeyPressed = false;
		//private bool isKeyHeld = false;

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
			MainBitmap.Image = Refs.m.AsBitmap();
			Refresh();
		}
	}
}

//private void KeyDownHandler(object sender, KeyEventArgs e)
//{
//	//e.SuppressKeyPress = isKeyPressed;
//	//if (isKeyPressed) isKeyHeld = true;
//	isKeyPressed = true;
//}

//public void KeyPressHandler(object sender, KeyPressEventArgs e)
//{
//	e.Handled = true;
//}

//private void KeyUpHandler(object sender, KeyEventArgs e)
//{
//	//isKeyPressed = false;
//	//isKeyHeld = false;
//}

//this.KeyPress += new KeyPressEventHandler(KeyPressHandler);
//this.KeyDown += new KeyEventHandler(KeyDownHandler);
//this.KeyPress += new KeyPressEventHandler(KeyPressHandler);
//this.KeyUp += new KeyEventHandler(KeyUpHandler);