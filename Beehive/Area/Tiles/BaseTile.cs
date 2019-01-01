using System;

namespace Beehive
{
	[Serializable()]
	public class BaseTile<M, T>
	{
		public BaseMap<T> myMap;
		public Loc loc;

		public BaseTile(Loc p, BaseMap<T> map)
		{
			loc = p;
			myMap = map;
		}

		public T OneNorth()
		{
			var newLoc = Loc.AddPts(loc, Dir.North);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		public T OneSouth()
		{
			var newLoc = Loc.AddPts(loc, Dir.South);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		public T OneEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.East);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		public T OneWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.West);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		public T OneNorthEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.NorthEast);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		public T OneSouthEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.SouthEast);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		public T OneNorthWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.NorthWest);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		public T OneSouthWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.SouthWest);
			return (myMap.ValidLoc(newLoc)) ? myMap.TileByLoc(newLoc) : default(T);
		}

		// todo use loc class distance calculator
		public static double Distance(MapTile t1, MapTile t2)
		{
			double a = Math.Pow(t1.loc.X - t2.loc.X, 2);
			double b = Math.Pow(t1.loc.Y - t2.loc.Y, 2);
			double c = Math.Sqrt(a + b);
			return c;
		}
	}
}