using System.Collections.Generic;

namespace Beehive
{
	public static class Dir
	{
		public static Loc North = new Loc(0, -1);
		public static Loc East = new Loc(1, 0);
		public static Loc South = new Loc(0, 1);
		public static Loc West = new Loc(-1, 0);

		public static Loc NorthEast = Loc.AddPts(North, East);
		public static Loc SouthEast = Loc.AddPts(South, East);
		public static Loc SouthWest = Loc.AddPts(South, West);
		public static Loc NorthWest = Loc.AddPts(North, West);

		public static HashSet<Loc> AllAround =
			new HashSet<Loc> {
				North, East, South, West,
			 	NorthEast, SouthEast, SouthWest, NorthWest };

		public static HashSet<Loc> Cardinals =
			new HashSet<Loc> {
				North, East, South, West };

		public static HashSet<Loc> KnightMoves =
			new HashSet<Loc> {
				Loc.AddPts(North, NorthEast), Loc.AddPts(North, NorthWest),
				Loc.AddPts(East, NorthEast),  Loc.AddPts(East, SouthEast),
				Loc.AddPts(South, SouthEast), Loc.AddPts(South, SouthWest),
				Loc.AddPts(West, NorthWest),  Loc.AddPts(West, SouthWest)};

		public static HashSet<Loc> DodgeHorizontal =
			new HashSet<Loc> { North, South };

		public static HashSet<Loc> DodgeMoves =
			new HashSet<Loc> { North, East, South, West };

		public static HashSet<Loc> DodgeVertical =
			new HashSet<Loc> { East, West };

		public static HashSet<Loc> LeapMoves =
			new HashSet<Loc> {
				Loc.AddPts(North, North), Loc.AddPts(East, East),
				Loc.AddPts(South, South), Loc.AddPts(West, West)};
	}
}