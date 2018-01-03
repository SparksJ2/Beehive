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
		public Loc loc;
		public string name;
		public Color myColor;

		public Mobile(string nameIn, Color useColor)
		{
			loc = new Loc();
			name = nameIn;
			myColor = useColor;
		}

		public void SetXY(int x, int y)
		{
			loc = new Loc(x, y);
		}
	}
}