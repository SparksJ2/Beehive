using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	partial class Player
	{
		private void BoinkHeld()
		{
			Player p = Refs.p;
			Cubi partner = Harem.GetId(p.heldCubiId);

			Refs.mf.Announce("Boink!", myAlign, myColor);
			Refs.mf.Announce("Yay Boink!", partner.myAlign, partner.myColor);
		}
	}
}