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
		public Player(Map m) : base(m)
		{
		}

		public void HandlePlayerInput(PreviewKeyDownEventArgs e)
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
			}
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