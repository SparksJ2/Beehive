﻿using System;
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
	}
}