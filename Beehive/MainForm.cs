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
		public Map map;
		public Player p;
		public Cubi s;

		public KeyEventHandler kh;
		public PreviewKeyDownEventHandler eh;

		public Stopwatch turnTimer;

		public MainForm()
		{
			InitializeComponent();

			// generate map
			map = new MazeGenerator().Create(65, 25);

			p = new Player(this, map);
			p.SetXY(1, 1);
			s = new Cubi(this, map, p);
			//s.SetXY(65 - 2, 25 - 2);
			s.SetXY(4, 3);
			map.SetMobiles(p, s);
			p.SetCubi(s);

			// draw initial map
			new Flow(map, p, s).RemakeFlow(p.loc);
			var bitMapMap = map.AsBitmap();

			// add to window
			MainBitmap.Image = bitMapMap;

			// init key handlers
			turnTimer = new Stopwatch(); turnTimer.Start();

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

			bool timePass = p.HandlePlayerInput(e);
			new Flow(map, p, s).RemakeFlow(p.loc);
			if (timePass)
			{
				// check win condition first
				if (map.Touching(p, s)) { MessageBox.Show("Winners you are!"); }

				// run ai
				s.AiMove();
			}

			// update screen
			map.HealWalls();
			MainBitmap.Image = map.AsBitmap();
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