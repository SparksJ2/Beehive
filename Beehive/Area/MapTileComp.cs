using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	internal class MapTileComp : IEqualityComparer<MapTile>
	{
		public bool Equals(MapTile n1, MapTile n2)
		{
			if (n1 == null && n2 == null) return true;
			else if (n1 == null || n2 == null) return false;

			return ((n1.loc.X == n2.loc.X) && (n1.loc.Y == n2.loc.Y));
		}

		public int GetHashCode(MapTile n)
		{
			return n.loc.X + (n.loc.Y << 8);
		}

		//return base.GetHashCode();
	}
}