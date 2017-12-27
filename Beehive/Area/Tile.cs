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
			var newLoc = AddPts(loc, Dir.North);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneSouth()
		{
			var newLoc = AddPts(this.loc, Dir.South);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneEast()
		{
			var newLoc = AddPts(this.loc, Dir.East);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneWest()
		{
			var newLoc = AddPts(this.loc, Dir.West);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		private Point AddPts(Point a, Point b)
		{
			return new Point(a.X + b.X, a.Y + b.Y);
		}
	}
}