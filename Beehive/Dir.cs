using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace Beehive
{
	public class Dir
	{
		public static Point North = new Point(0, -1);
		public static Point East = new Point(1, 0);
		public static Point South = new Point(0, 1);
		public static Point West = new Point(-1, 0);

		public static Point NorthEast = AddPts(North, East);
		public static Point SouthEast = AddPts(South, East);
		public static Point SouthWest = AddPts(South, West);
		public static Point NorthWest = AddPts(North, West);

		public static Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}
	}
}