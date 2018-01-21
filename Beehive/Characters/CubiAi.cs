using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public static class CubiAi
	{
		public static void SimpleFlee(int distance, Flow ourFlow)
		{
			HashSet<FlowSquare> heads = new HashSet<FlowSquare>(new FlowSquareComp());

			heads.UnionWith(SetUpInitialRing(distance, ourFlow));
			heads.UnionWith(PlayerNectarTiles(ourFlow));

			foreach (FlowSquare fs in heads)
			{
				fs.flow = 0;
			}

			ourFlow.RunFlow();
		}

		// todo turned off while we work on refactoring Ai
		//public static HashSet<Tile> PreferInfront(int distance)
		//{
		//	Loc front = Refs.p.lastMove ?? new Loc(0, 0);
		//	Loc playerLoc = Refs.p.loc ?? new Loc(0, 0);

		//	HashSet<Tile> heads = new HashSet<Tile>(new TileComp());
		//	heads.UnionWith(SetUpInitialRing(distance));

		//	// remove tiles not to player front
		//	HashSet<Tile> toRemove = new HashSet<Tile>(new TileComp());

		//	foreach (Tile t in heads)
		//	{
		//		Loc relativeLoc = Loc.SubPts(t.loc, playerLoc);

		//		if (front.X * relativeLoc.X < 0) toRemove.Add(t);
		//		if (front.Y * relativeLoc.Y < 0) toRemove.Add(t);
		//	}

		//	heads.Difference(toRemove);

		//	return heads;
		//}

		//public static HashSet<Tile> MirrorIdOne(int distance)
		//{
		//	// not implemented
		//	return SimpleFlee(distance);
		//}

		//public static HashSet<Tile> PreferBehind(int distance)
		//{
		//	// todo some duplication here
		//	Loc front = Refs.p.lastMove ?? new Loc(0, 0);
		//	Loc playerLoc = Refs.p.loc ?? new Loc(0, 0);

		//	HashSet<Tile> heads = new HashSet<Tile>(new TileComp());
		//	heads.UnionWith(SetUpInitialRing(distance));

		//	// remove tiles not to player front
		//	HashSet<Tile> toRemove = new HashSet<Tile>(new TileComp());

		//	foreach (Tile t in heads)
		//	{
		//		Loc relativeLoc = Loc.SubPts(t.loc, playerLoc);

		//		// todo just debuff power by half to prevent player crossing
		//		if (front.X * relativeLoc.X > 0) toRemove.Add(t);
		//		if (front.Y * relativeLoc.Y > 0) toRemove.Add(t);
		//	}

		//	heads.Difference(toRemove);

		//	return heads;
		//}

		//public static TiltedRing<Tile> PreferBehind(int distance)
		//{
		//}

		private static HashSet<FlowSquare> SetUpInitialRing(int distance, Flow f)
		{
			// we'll try to flow to a set distance from the player by
			//    making a ring of target squares and working from there
			var allTiles = Refs.m.TileList();
			var ring = new HashSet<Tile>(new TileComp());
			foreach (Tile t in allTiles)
			{
				double c = Loc.Distance(Refs.p.loc, t.loc);
				if (c > distance - 1 && c < distance + 1) { ring.Add(t); }
			}

			return f.FlowSquaresFromTileSet(ring);
		}

		private static HashSet<FlowSquare> PlayerNectarTiles(Flow f)
		{
			// tiles containing player nectar are targets too
			var allTiles = Refs.m.TileList();
			var nectarTiles = new HashSet<Tile>(new TileComp());
			foreach (Tile t in allTiles)
			{
				if (t.hasNectar && t.nectarCol == Refs.p.myColor)
				{
					nectarTiles.Add(t);
				}
			}
			return f.FlowSquaresFromTileSet(nectarTiles);
		}
	}
}