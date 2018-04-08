using System;

namespace Beehive
{
	[Serializable()]
	public class BaseMap<T>
	{
		protected int xLen, yLen;
		protected T[,] tiles;

		public int GetXLen() => xLen;

		public int GetYLen() => yLen;

		public bool ValidLoc(Loc p)
		{
			return (p.X >= 0 && p.X < xLen && p.Y >= 0 && p.Y < yLen) ? true : false;
		}

		public T TileByLoc(Loc p) => tiles[p.X, p.Y];
	}
}