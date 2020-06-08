using System;
using System.Collections.Generic;
using System.Linq;

namespace Beehive
{
	[Serializable()]
	public class BaseMap<T>
	{
		/// base class for mainmaps and flowmaps

		public int xLen, yLen;
		protected T[,] tiles;

		public int GetXLen() => xLen;

		public int GetYLen() => yLen;

		public bool ValidLoc(Loc p)
		{
			return (p.X >= 0 && p.X < xLen && p.Y >= 0 && p.Y < yLen) ? true : false;
		}

		private static Random rng = new Random();

		public static T RandomFromList(HashSet<T> tileList)
		{
			return tileList.ElementAt(rng.Next(tileList.Count));
		}

		public T TileByLoc(Loc p) => tiles[p.X, p.Y];
	}
}