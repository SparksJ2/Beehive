using System.Collections.Generic;

namespace Beehive
{
	// hack: we're not going to be bothering with that right now
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
	public class FlowTileSet : HashSet<FlowTile>
	{
		public FlowTileSet() : base(new FlowTileComp())
		{
		}

		public FlowTileSet(IEnumerable<FlowTile> source) : base(source, new FlowTileComp())
		{
		}
	}
}