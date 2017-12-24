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

		public Tile OneNorth()
		{
			var loc = AddPts(this.loc, Dir.North);
			return (Refs.m.ValidLoc(loc)) ? Refs.m.TileByLoc(loc) : null;
		}

		public Tile OneSouth()
		{
			var loc = AddPts(this.loc, Dir.South);
			return (Refs.m.ValidLoc(loc)) ? Refs.m.TileByLoc(loc) : null;
		}

		public Tile OneEast()
		{
			var loc = AddPts(this.loc, Dir.East);
			return (Refs.m.ValidLoc(loc)) ? Refs.m.TileByLoc(loc) : null;
		}

		public Tile OneWest()
		{
			var loc = AddPts(this.loc, Dir.West);
			return (Refs.m.ValidLoc(loc)) ? Refs.m.TileByLoc(loc) : null;
		}

		private Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}
	}
}