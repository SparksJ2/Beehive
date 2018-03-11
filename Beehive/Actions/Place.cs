namespace Beehive
{
	partial class Player
	{
		// todo fix duplication?
		private void ActionNorth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneNorth();
			DoActionWithTile(t);
		}

		private void ActionEast()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneEast();
			DoActionWithTile(t);
		}

		private void ActionSouth()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneSouth();
			DoActionWithTile(t);
		}

		private void ActionWest()
		{
			MapTile t = Refs.m.TileByLoc(loc).OneWest();
			DoActionWithTile(t);
		}

		//action decision matrix
		//inventory  / tile is:    / action
		//-P, -C,    / empty       / can't do anything
		//-P, +C,    / empty       / put cubi down here
		//+P, -C     / empty       / put pillow down here
		//+P, +C     / empty       / put cubi down here

		//-P, -C,    / has pillow  / pick up pillow
		//-P, +C,    / has pillow  / pick up pillow
		//+P, -C     / has pillow  / pick up pillow
		//+P, +C     / has pillow  / pick up pillow

		//-P, -C,    / has cubi    / pick up cubi
		//-P, +C,    / has cubi    / swap held cubi
		//+P, -C     / has cubi    / pick up cubi
		//+P, +C     / has cubi    / swap held cubi

		private void DoActionWithTile(MapTile t)
		{
			if (Refs.m.EdgeLoc(t.loc)) return; // can't break out of map

			if (NothingHere(t))
			{
				if (HoldingCubi()) PutCubi(t);
				else if (HoldingPillow()) PutPillow(t);
				else Refs.mf.Announce("Nothing to do here.", myAlign, myColor);
			}
			else if (PillowHere(t))
			{
				GrabPillow(t);
			}
			else if (CubiHere(t))
			{
				if (HoldingCubi()) SwapCubi(t);
				else GrabCubi(t);
			}

			UpdateInventory();
		}

		private void GrabPillow(MapTile t)
		{
			t.clear = true;
			heldPillows++;
			Refs.mf.Announce("You pick up a pillow. You now have " + heldPillows + ".", myAlign, myColor);
		}

		private void PutPillow(MapTile t)
		{
			t.clear = false;
			heldPillows--;
			Refs.mf.Announce("You place the pillow to make a wall. You have " + heldPillows + " left.", myAlign, myColor);
		}

		private void GrabCubi(MapTile t)
		{
			Cubi caughtCubi = Refs.m.CubiAt(t.loc);
			caughtCubi.loc = this.loc;
			caughtCubi.beingCarried = true;
			heldCubiId = caughtCubi.myIdNo;
			Refs.mf.Announce("Gotcha!", myAlign, myColor);
			Refs.mf.Announce("EEEK!!", caughtCubi.myAlign, caughtCubi.myColor);
		}

		private void PutCubi(MapTile t)
		{
			Cubi myHeldCubi = Harem.GetId(heldCubiId);
			myHeldCubi.loc = t.loc;
			myHeldCubi.beingCarried = false;
			heldCubiId = 0;
			Refs.mf.Announce("You're free to go... if you can.", myAlign, myColor);
		}

		private void SwapCubi(MapTile t)
		{
			Cubi myHeldCubi = Harem.GetId(heldCubiId);
			Cubi caughtCubi = Refs.m.CubiAt(t.loc);

			myHeldCubi.loc = t.loc;
			myHeldCubi.beingCarried = false;
			heldCubiId = 0;

			caughtCubi.loc = this.loc;
			caughtCubi.beingCarried = true;
			heldCubiId = caughtCubi.myIdNo;

			Refs.mf.Announce("Hang on, let's grab you instead!", myAlign, myColor);
		}

		private bool HoldingPillow() => heldPillows > 0;

		private bool HoldingCubi() => heldCubiId > 0;

		private bool PillowHere(MapTile t) => !t.clear;

		private bool NothingHere(MapTile t) => !CubiHere(t) && !PillowHere(t);

		private bool CubiHere(MapTile t) => Refs.m.ContainsCubi(t.loc);
	}
}