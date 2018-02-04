using System.Collections.Generic;

namespace Beehive
{
	public static class HashSetExt
	{
		// really could be better looking...
		public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
		{
			return new HashSet<T>(source, comparer);
		}

		public static HashSet<Tile> ToTileHashSet<Tile>(this IEnumerable<Tile> source)
		{
			return new HashSet<Tile>(source,
				(IEqualityComparer<Tile>)new MapTileComp());
		}

		public static HashSet<FlowSquare> ToFlowSquareHashSet<FlowSquare>(this IEnumerable<FlowSquare> source)
		{
			return new HashSet<FlowSquare>(source,
				(IEqualityComparer<FlowSquare>)new FlowTileComp());
		}

		public static HashSet<Tile> Difference<Tile>(
			this HashSet<Tile> source, HashSet<Tile> subtract)
		{
			foreach (Tile item in subtract) { source.Remove(item); }
			return source;
		}
	}
}