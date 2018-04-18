using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Beehive
{
	[Serializable()]
	public class Harem
	{
		public List<Cubi> roster; // secure this, use GetId() instead.

		public Harem()
		{
			roster = new List<Cubi>();

			// todo fix hardcoded numbers
			// todo simple AI while we refactor flow stuff
			Cubi a = new Cubi("Ai'nana", 1, Color.HotPink);
			a.SetXY(33, 9);
			a.myStdAi = new CubiStdAi(CubiAi.FleeToRing);
			a.myJbAi = new CubiJailBreak(CubiAi.JailBreak);
			roster.Add(a);

			Cubi b = new Cubi("Bel'lona ", 2, Color.RosyBrown);
			b.SetXY(34, 9);
			b.myStdAi = new CubiStdAi(CubiAi.FlowOutAndBack);
			b.myJbAi = new CubiJailBreak(CubiAi.JailBreak);
			roster.Add(b);

			Cubi c = new Cubi("Cy'rene", 3, Color.MediumVioletRed);
			c.SetXY(35, 9);
			c.myStdAi = new CubiStdAi(CubiAi.FleeToRing);
			c.myJbAi = new CubiJailBreak(CubiAi.JailBreak);
			roster.Add(c);

			Cubi d = new Cubi("Del'ta", 4, Color.SeaGreen);
			d.SetXY(34, 8);
			d.myStdAi = new CubiStdAi(CubiAi.FleeToRing);
			d.myJbAi = new CubiJailBreak(CubiAi.JailBreak);
			roster.Add(d);
		}

		public static Cubi GetId(int id)
		{
			return Refs.h.roster.Where(x => x.myIdNo == id).ToList().First();
		}

		public static int MaxId()
		{
			return Refs.h.roster.Count; // todo dodgy way of working it out...
		}
	}
}