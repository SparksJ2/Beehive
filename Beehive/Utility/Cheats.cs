using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	internal class Cheats
	{
		public static void ClearNectar()
		{
			Refs.mf.Announce("Cheat: clearing nectar...", Refs.p.myAlign, Refs.p.myColor);
			Refs.m.ClearNectar();
			Refs.mf.UpdateMap();
		}

		public static void TopOffEnergy()
		{
			Refs.mf.Announce("Cheat: topped off cubi jump energy...",
				Refs.p.myAlign, Refs.p.myColor);
			Refs.h.MaxJumpEnergy();
		}
	}
}