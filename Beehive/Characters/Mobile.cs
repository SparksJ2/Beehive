using System;
using System.Drawing;
using System.Windows.Forms;

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

		public HorizontalAlignment myAlign;

		public Mobile()
		{
			loc = new Loc();
		}

		public Mobile(string nameIn, Color useColor)
		{
			loc = new Loc();
			name = nameIn;
			myColor = useColor;
		}

		public void SetXY(int x, int y) => loc = new Loc(x, y);

		public void AddHorny(int h) => horny += h;

		public void SetHorny(int h) => horny = h;

		public int GetHorny() => horny;
	}
}