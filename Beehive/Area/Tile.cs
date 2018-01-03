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
		public bool Cnectar = false;
		public bool noTunnel = false; // only for maze gen

		// for use with KnightMoves(), DodgeMoves(), LeapMoves()
		public HashSet<Tile> GetPossibleMoves(HashSet<Point> options)
		{
			var result = new HashSet<Tile>(new TileComp());

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

		public static HashSet<Tile> FilterOutClear(HashSet<Tile> ts)
		{
			return ts.Where(t => !t.clear).ToTileHashSet();
		}

		public static HashSet<Tile> Tunnelable(HashSet<Tile> ts)
		{
			return ts.Where(t => t.noTunnel == false).ToTileHashSet();
		}

		public static Tile RandomFromList(HashSet<Tile> tileList)
		{
			var rng = Refs.c.rng;
			return tileList.ElementAt(rng.Next(tileList.Count));
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