using System.Collections.Generic;

namespace Beehive
{
	public static class Dir
	{
		/// provides Locs and hashsets of Locs to locate various nearby tiles, all nearby tiles, knight-moves, etc.

		public static Loc North = new Loc(0, -1);
		public static Loc East = new Loc(1, 0);
		public static Loc South = new Loc(0, 1);
		public static Loc West = new Loc(-1, 0);

		public static Loc North2 = new Loc(0, -2);
		public static Loc East2 = new Loc(2, 0);
		public static Loc South2 = new Loc(0, 2);
		public static Loc West2 = new Loc(-2, 0);

		public static Loc NorthEast = Loc.AddPts(North, East);
		public static Loc SouthEast = Loc.AddPts(South, East);
		public static Loc SouthWest = Loc.AddPts(South, West);
		public static Loc NorthWest = Loc.AddPts(North, West);

		public static Loc NorthEast2 = Loc.AddPts(North2, East2);
		public static Loc SouthEast2 = Loc.AddPts(South2, East2);
		public static Loc SouthWest2 = Loc.AddPts(South2, West2);
		public static Loc NorthWest2 = Loc.AddPts(North2, West2);

		public static HashSet<Loc> AllAround =
			new HashSet<Loc> {
				North, East, South, West,
			 	NorthEast, SouthEast, SouthWest, NorthWest };

		public static HashSet<Loc> AllAround2 =
			new HashSet<Loc> {
				North2, East2, South2, West2,
			 	NorthEast2, SouthEast2, SouthWest2, NorthWest2 };

		public static HashSet<Loc> Cardinals =
			new HashSet<Loc> {
				North, East, South, West };

		public static HashSet<Loc> Cardinals2 =
			new HashSet<Loc> {
				North2, East2, South2, West2 };

		public static HashSet<Loc> KnightMoves =
			new HashSet<Loc> {
				Loc.AddPts(North, NorthEast), Loc.AddPts(North, NorthWest),
				Loc.AddPts(East, NorthEast),  Loc.AddPts(East, SouthEast),
				Loc.AddPts(South, SouthEast), Loc.AddPts(South, SouthWest),
				Loc.AddPts(West, NorthWest),  Loc.AddPts(West, SouthWest)};

		public static HashSet<Loc> DodgeHorizontal =
			new HashSet<Loc> { North, South };

		public static HashSet<Loc> DodgeHorizontal2 =
			new HashSet<Loc> { North2, South2 };

		public static HashSet<Loc> DodgeVertical =
			new HashSet<Loc> { East, West };

		public static HashSet<Loc> DodgeVertical2 =
			new HashSet<Loc> { East2, West2 };

		public static HashSet<Loc> DodgeMoves = Cardinals;

		public static HashSet<Loc> LeapMoves = Cardinals2;
	}
}