using System.Collections.Generic;

namespace Beehive
{
	// hack: we're not going to be bothering with that right now
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class MapTileSet : HashSet<MapTile>
	{
		public MapTileSet() : base(new MapTileComp())
		{
		}

		public MapTileSet(IEnumerable<MapTile> source) : base(source, new MapTileComp())
		{
		}
	}
}