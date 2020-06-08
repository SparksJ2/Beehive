using System;

namespace Beehive
{
	[Serializable()]
	public class Loc
	{
		/// a 2d X/Y vector location, with utility stuff

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

		public static bool operator ==(Loc l, Loc m) => Same(l, m);

		public static bool operator !=(Loc l, Loc m) => !Same(l, m);

		public static Loc AddPts(Loc a, Loc b) => new Loc(a.X + b.X, a.Y + b.Y);

		public static Loc AddPts(Loc a, Loc b, Loc c) => new Loc(a.X + b.X + c.X, a.Y + b.Y + c.Y);

		public static Loc SubPts(Loc a, Loc b) => new Loc(a.X - b.X, a.Y - b.Y);

		public static bool Same(Loc a, Loc b) => a.X - b.X == 0 && a.Y - b.Y == 0;

		public static double Distance(Loc l1, Loc l2)
		{
			double a = Math.Pow(l1.X - l2.X, 2);
			double b = Math.Pow(l1.Y - l2.Y, 2);
			double c = Math.Sqrt(a + b);
			return c;
		}

		public override bool Equals(object obj)
		{
			var loc = obj as Loc;
			return loc != null && X == loc.X && Y == loc.Y;
		}

		public override int GetHashCode()
		{
			var hashCode = 1861411795;
			hashCode = hashCode * -1521134295 + X.GetHashCode();
			hashCode = hashCode * -1521134295 + Y.GetHashCode();
			return hashCode;
		}
	}
}