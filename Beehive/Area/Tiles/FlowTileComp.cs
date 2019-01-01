using System;
using System.Collections.Generic;

namespace Beehive
{
	[Serializable()]
	public class FlowTileComp : IEqualityComparer<FlowTile>
	{
		/// standard compare interface

		public bool Equals(FlowTile fs1, FlowTile fs2)
		{
			if (fs1 == null && fs2 == null) return true;
			else if (fs1 == null || fs2 == null) return false;

			return ((fs1.loc.X == fs2.loc.X) && (fs1.loc.Y == fs2.loc.Y));
		}

		public int GetHashCode(FlowTile n) => n.loc.X + (n.loc.Y << 8);
	}
}