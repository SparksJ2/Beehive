namespace Beehive
{
	partial class Player
	{
		/// for player throwing actions

		private void ThrowNorth()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneNorth();
			if (t.clear) ThrowDirection(Dir.North);
		}

		private void ThrowWest()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneWest();
			if (t.clear) ThrowDirection(Dir.West);
		}

		private void ThrowEast()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneEast();
			if (t.clear) ThrowDirection(Dir.East);
		}

		private void ThrowSouth()
		{
			MapTile t = Refs.m.TileByLoc(this.loc).OneSouth();
			if (t.clear) ThrowDirection(Dir.South);
		}

		private void ThrowDirection(Loc vector)
		{
			Player p = Refs.p;

			if (p.HoldingCubi()) { ThrowCubiMain(vector); }
			else if (heldPillows > 0) { ThrowPillowMain(vector); }
			else { Refs.mf.Announce("You don't have anything to throw.", myAlign, myColor); }
		}

		private string CheckClearForThrown(Loc vector, MapTile activeTile)
		{
			Loc newLoc = Loc.AddPts(vector, activeTile.loc);
			if (!Refs.m.TileByLoc(newLoc).clear) return "wall";

			foreach (Cubi c in Refs.h.roster)
			{ if (Loc.Same(newLoc, c.loc)) { return "cubi"; } }

			foreach (Loc pent in Refs.m.pents)
			{ if (Loc.Same(newLoc, pent)) { return "pent"; } }

			return "clear";
		}
	}
}