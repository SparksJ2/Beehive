using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Drawing2D;
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

		public static bool Left = false;
		public static bool Right = true;

		public static HashSet<Point> KnightMoves()
		{
			var r = new HashSet<Point>
			{
				AddPts(North, NorthEast),
				AddPts(North, NorthWest),
				AddPts(East, NorthEast),
				AddPts(East, SouthEast),
				AddPts(South, SouthEast),
				AddPts(South, SouthWest),
				AddPts(West, NorthWest),
				AddPts(West, SouthWest)};

			return r;
		}

		public static HashSet<Point> DodgeMoves()
		{
			return new HashSet<Point> { North, East, South, West };
		}

		public static HashSet<Point> DodgeHorizontal()
		{
			return new HashSet<Point> { North, South };
		}

		public static HashSet<Point> DodgeVertical()
		{
			return new HashSet<Point> { East, West };
		}

		public static HashSet<Point> LeapMoves()
		{
			return new HashSet<Point>
			{ AddPts(North, North),
				AddPts(East, East),
				AddPts(South, South),
				AddPts(West, West),
			};
		}

		public static Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}

		public static Point AddPts(Point a, Point b, Point c)
		{
			return new Point(a.X + b.X + c.X, a.Y + b.Y + c.Y);
		}
	}
}