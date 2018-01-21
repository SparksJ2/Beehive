using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	internal class FlowSquareComp : IEqualityComparer<FlowSquare>
	{
		public bool Equals(FlowSquare fs1, FlowSquare fs2)
		{
			if (fs1 == null && fs2 == null) return true;
			else if (fs1 == null || fs2 == null) return false;

			return ((fs1.loc.X == fs2.loc.X) && (fs1.loc.Y == fs2.loc.Y));
		}

		public int GetHashCode(FlowSquare n)
		{
			return n.loc.X + (n.loc.Y << 8);
		}
	}
}