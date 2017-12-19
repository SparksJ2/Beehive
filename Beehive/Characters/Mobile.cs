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
		internal Map map;
		internal Point loc;
		internal MainForm mf;

		public Mobile(MainForm f, Map m)
		{
			map = m;
			mf = f;
			loc = new Point();
		}

		public void SetXY(int x, int y)
		{
			loc = new Point(x, y);
		}
	}
}