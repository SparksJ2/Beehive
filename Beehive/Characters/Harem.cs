using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class Harem
	{
		public List<Cubi> roster;

		public Harem()
		{
			roster = new List<Cubi>();

			// todo fix hardcoded numbers
			// todo simple AI while we refactor flow stuff
			Cubi a = new Cubi("Ai'nana", 1, Color.HotPink);
			a.SetXY(33, 9);
			a.myAi = new CubiAiType(CubiAi.SimpleFlee);
			roster.Add(a);

			Cubi b = new Cubi("Bel'lona ", 2, Color.RosyBrown);
			b.SetXY(34, 9);
			b.myAi = new CubiAiType(CubiAi.SimpleFlee);
			roster.Add(b);

			Cubi c = new Cubi("Cy'rene", 3, Color.MediumVioletRed);
			c.SetXY(35, 9);
			c.myAi = new CubiAiType(CubiAi.SimpleFlee);
			roster.Add(c);

			Cubi d = new Cubi("Del'ta", 4, Color.SeaGreen);
			d.SetXY(34, 8);
			d.myAi = new CubiAiType(CubiAi.SimpleFlee);
			roster.Add(d);
		}

		public static Cubi GetId(int id)
		{
			return Refs.h.roster.Where(x => x.myIdNo == id).ToList().First();
		}
	}
}