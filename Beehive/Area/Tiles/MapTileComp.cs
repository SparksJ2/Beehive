using System;
using System.Collections.Generic;

namespace Beehive
{
	[Serializable()]
	internal class MapTileComp : IEqualityComparer<MapTile>
	{
		/// standard compare interface

		public bool Equals(MapTile n1, MapTile n2)
		{
			if (n1 == null && n2 == null) return true;
			else if (n1 == null || n2 == null) return false;

			return ((n1.loc.X == n2.loc.X) && (n1.loc.Y == n2.loc.Y));
		}

		public int GetHashCode(MapTile n) => n.loc.X + (n.loc.Y << 8);
	}
}