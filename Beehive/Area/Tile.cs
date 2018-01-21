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
		public Loc loc;
		public bool clear = false;
		public string gly = "#";

		public bool hasNectar = false;
		public Color nectarCol;

		public bool noTunnel = false; // only for maze gen

		public Tile()
		{
			// intentionally left blank
		}

		// for use with KnightMoves(), DodgeMoves(), LeapMoves()
		public HashSet<Tile> GetPossibleMoves(HashSet<Loc> options)
		{
			var result = new HashSet<Tile>(new TileComp());

			foreach (Loc p in options)
			{
				Loc newloc = Loc.AddPts(loc, p);
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

		private static Random rng = new Random();

		public static Tile RandomFromList(HashSet<Tile> tileList)
		{
			return tileList.ElementAt(rng.Next(tileList.Count));
		}

		public Tile OneNorth()
		{
			var newLoc = Loc.AddPts(loc, Dir.North);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneSouth()
		{
			var newLoc = Loc.AddPts(loc, Dir.South);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.East);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.West);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneNorthEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.NorthEast);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneSouthEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.SouthEast);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneNorthWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.NorthWest);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public Tile OneSouthWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.SouthWest);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}
	}
}