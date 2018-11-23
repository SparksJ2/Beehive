using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	partial class Player
	{
		private int BoinkHeld()
		{
			MapTile here = Refs.m.TileByLoc(loc);
			Cubi partner = Harem.GetId(heldCubiId);

			horny += 5;
			partner.horny += 5;

			// todo consolidating orgasms.
			// todo more descriptions.
			int timepass = 0;
			if (horny > 15 && partner.horny > 15) // cumming together
			{
				Refs.mf.Announce("Ohhh Yes! Yes! *splurt* (together)", partner.myAlign, partner.myColor);
				Refs.mf.Announce("Awwww yeah! *splurt* (together)", myAlign, myColor);
				timepass = 8;

				MainMap.SplurtNectar(here, partner.myIdNo);
				partner.horny -= 10;

				MainMap.SplurtNectar(here, myIndex: 0);
				horny -= 10;
			}
			else if (partner.horny > 15) // partner only orgasm
			{
				Refs.mf.Announce("Ohhh Yes! Yes! *splurt*", partner.myAlign, partner.myColor);
				timepass = 1;
				MainMap.SplurtNectar(here, partner.myIdNo);
				partner.horny -= 10;
			}
			else if (horny > 15) // player only orgasm
			{
				Refs.mf.Announce("Awwww yeah! *splurt*", myAlign, myColor);
				timepass = 5;
				MainMap.SplurtNectar(here, myIndex: 0);
				horny -= 10;
			}
			else // nobody cums (yet)
			{
				Refs.mf.Announce("Loud boinking noises...", myAlign, myColor);
				timepass = 1;
			}

			return timepass;
		}
	}
}