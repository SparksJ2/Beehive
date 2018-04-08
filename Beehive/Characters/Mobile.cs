using System;
using System.Drawing;

namespace Beehive
{
	[Serializable()]
	public class Mobile
	{
		public Loc loc;
		public string name;
		public Color myColor;
		public string glyph;

		protected int horny = 0;

		public Mobile(string nameIn, Color useColor)
		{
			loc = new Loc();
			name = nameIn;
			myColor = useColor;
		}

		public void SetXY(int x, int y) => loc = new Loc(x, y);
	}
}