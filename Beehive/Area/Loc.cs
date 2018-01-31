using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class Loc
	{
		public int X;
		public int Y;

		public Loc()
		{
			X = 0; Y = 0;
		}

		public Loc(int xIn, int yIn)
		{
			X = xIn; Y = yIn;
		}

		public static Loc AddPts(Loc a, Loc b)
		{
			return new Loc(a.X + b.X, a.Y + b.Y);
		}

		public static Loc AddPts(Loc a, Loc b, Loc c)
		{
			return new Loc(a.X + b.X + c.X, a.Y + b.Y + c.Y);
		}

		public static Loc SubPts(Loc a, Loc b)
		{
			return new Loc(a.X - b.X, a.Y - b.Y);
		}

		public static bool Same(Loc a, Loc b)
		{
			return a.X - b.X == 0 && a.Y - b.Y == 0;
		}

		public static double Distance(Loc l1, Loc l2)
		{
			double a = Math.Pow(l1.X - l2.X, 2);
			double b = Math.Pow(l1.Y - l2.Y, 2);
			double c = Math.Sqrt(a + b);
			return c;
		}
	}
}