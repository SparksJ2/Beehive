using System;
using System.Collections.Generic;
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
			Grimoire.Load("Beehive.grim");

			// todo fix hardcoded numbers
			// todo simple AI while we refactor flow stuff

			Cubi a = new Cubi(1);
			Grimoire.FillCubi(a);
			a.SetXY(33, 9);
			roster.Add(a);

			Cubi b = new Cubi(2);
			Grimoire.FillCubi(b);
			b.SetXY(34, 9);
			roster.Add(b);

			Cubi c = new Cubi(3);
			Grimoire.FillCubi(c);
			c.SetXY(35, 9);
			roster.Add(c);

			Cubi d = new Cubi(4);
			Grimoire.FillCubi(d);
			d.SetXY(34, 8);
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