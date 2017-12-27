using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class Tile
	{
		public Point loc;
		public bool clear = false;
		public char gly = '#';
		public int flow = 0;

		// for use with KnightMoves(), DodgeMoves(), LeapMoves()
		public List<Tile> GetPossibleMoves(List<Point> options)
		{
			var result = new List<Tile>();

			foreach (Point p in options)
			{
				Point newloc = AddPts(loc, p);
				if (Refs.m.ValidLoc(newloc) && Refs.m.ClearLoc(newloc))
				{
					result.Add(Refs.m.TileByLoc(newloc));
				}
			}
			return result;
		}

		public static List<Tile> FilterOutNotClear(List<Tile> ts)
		{
			return ts.Where(t => !t.clear).ToList();
		}

		public static Tile RandomFromList(List<Tile> tileList)
		{
			var rng = Refs.c.rng;
			return tileList[rng.Next(tileList.Count)];
		}

		public Tile OneNorth()
		{
			var newLoc = AddPts(loc, Dir.North);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneSouth()
		{
			var newLoc = AddPts(loc, Dir.South);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneEast()
		{
			var newLoc = AddPts(loc, Dir.East);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneWest()
		{
			var newLoc = AddPts(loc, Dir.West);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneNorthEast()
		{
			var newLoc = AddPts(loc, Dir.NorthEast);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneSouthEast()
		{
			var newLoc = AddPts(loc, Dir.SouthEast);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneNorthWest()
		{
			var newLoc = AddPts(loc, Dir.NorthWest);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneSouthWest()
		{
			var newLoc = AddPts(loc, Dir.SouthWest);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		private Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}
	}
}