using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	partial class Player
	{
		private int CaneHeld()
		{
			MapTile here = Refs.m.TileByLoc(loc);
			Cubi partner = Harem.GetId(heldCubiId);

			// todo different text on repeat spankings.
			Refs.mf.Announce("Time for discpline!", myAlign, myColor);
			Refs.mf.Announce("Yes! I mean no! I mean yes! Owwwww!", partner.myAlign, partner.myColor);

			partner.Spank(15);
			partner.AddHorny(5);

			Refs.mf.Announce("I think I'll stay put... for now.", partner.myAlign, partner.myColor);

			return 2;
		}
	}
}