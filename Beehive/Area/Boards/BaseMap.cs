namespace Beehive
{
	public class BaseMap<T>
	{
		protected int xLen, yLen;
		protected T[,] tiles;

		public int GetXLen()
		{
			return xLen;
		}

		public int GetYLen()
		{
			return yLen;
		}

		public bool ValidLoc(Loc p)
		{
			return (p.X >= 0 && p.X < xLen && p.Y >= 0 && p.Y < yLen) ? true : false;
		}

		public T TileByLoc(Loc p)
		{
			return tiles[p.X, p.Y];
		}
	}
}