namespace Beehive
{
	partial class Player
	{
		private void PlaceItemNorth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneNorth();
			PlaceItemOnTile(t);
		}

		private void PlaceItemEast()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneEast();
			PlaceItemOnTile(t);
		}

		private void PlaceItemSouth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneSouth();
			PlaceItemOnTile(t);
		}

		private void PlaceItemWest()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneWest();
			PlaceItemOnTile(t);
		}

		private void PlaceItemOnTile(MapTile t)
		{
			if (Refs.m.EdgeLoc(t.loc)) return;

			if (heldCubiId > 0 && t.clear && !Refs.m.ContainsCubi(t.loc))
			{
				// put down lover
				Cubi myHeldCubi = Harem.GetId(heldCubiId);
				myHeldCubi.loc = t.loc;
				myHeldCubi.beingCarried = false;
				heldCubiId = 0;
				Refs.mf.Announce("You're free to go... if you can.", myAlign, myColor);
			}
			else if (heldCubiId == 0 && Refs.m.ContainsCubi(t.loc))
			{
				// pick up lover
				Cubi caughtCubi = Refs.m.CubiAt(t.loc);
				caughtCubi.loc = this.loc;
				caughtCubi.beingCarried = true;
				heldCubiId = caughtCubi.myIdNo;
				Refs.mf.Announce("Gotcha!", myAlign, myColor);
				Refs.mf.Announce("EEEK!!", caughtCubi.myAlign, caughtCubi.myColor);
			}
			else if (t.clear && heldPillows > 0 && !Refs.m.ContainsCubi(t.loc))
			{
				// make wall
				t.clear = false;
				heldPillows--;
				Refs.mf.Announce("You place the pillow to make a wall. You have " + heldPillows + " left.", myAlign, myColor);
			}
			else if (heldCubiId == 0 && !t.clear)
			{
				// clear wall
				t.clear = true;
				heldPillows++;
				Refs.mf.Announce("You pick up a pillow. You now have " + heldPillows + ".", myAlign, myColor);
			}
			else
			{
				// can't figure out when they want to do, so...
				Refs.mf.Announce("Not enough space for that here.", myAlign, myColor);
			}
			UpdateInventory();
		}
	}
}