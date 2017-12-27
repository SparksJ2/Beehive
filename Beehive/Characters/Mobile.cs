using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Beehive
{
	public class Mobile
	{
		public Point loc;
		public string name;

		public Mobile(string nameIn)
		{
			loc = new Point();
			name = nameIn;
		}

		public void SetXY(int x, int y)
		{
			loc = new Point(x, y);
		}
	}
}