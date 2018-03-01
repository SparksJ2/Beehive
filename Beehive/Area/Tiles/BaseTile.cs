using System;
using System.Collections.Generic;
using System.Linq;

namespace Beehive
{
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

		private static Random rng = new Random();

		public static T RandomFromList(HashSet<T> tileList)
		{
			return tileList.ElementAt(rng.Next(tileList.Count));
		}
	}
}