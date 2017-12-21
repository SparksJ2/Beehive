using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class Tile
	{
		public Point loc;
		public bool clear = false;
		public char gly = '#';
		public int flow = 0;
	}
}