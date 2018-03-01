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

		//public static HashSet<MapTile> ToTileHashSet<MapTile>(this IEnumerable<MapTile> source)
		//{
		//	return new HashSet<MapTile>(source, (IEqualityComparer<MapTile>)new MapTileComp());
		//}

		public static MapTileSet ToMapTileSet(this IEnumerable<MapTile> source)
		{
			return new MapTileSet(source);
		}

		//public static HashSet<FlowTile> ToFlowTileHashSet<FlowTile>(this IEnumerable<FlowTile> source)
		//{
		//	return new HashSet<FlowTile>(source,
		//		(IEqualityComparer<FlowTile>)new FlowTileComp());
		//}

		public static FlowTileSet ToFlowTileSet(this IEnumerable<FlowTile> source)
		{
			return new FlowTileSet(source);
		}

		//public static HashSet<Tile> Difference<Tile>(
		//	this HashSet<Tile> source, HashSet<Tile> subtract)
		//{
		//	foreach (Tile item in subtract) { source.Remove(item); }
		//	return source;
		//}
	}
}