using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class MapTile
	{
		public Loc loc;
		public bool clear = false;
		public string gly = "#";

		public bool hasNectar = false;
		public Color nectarCol;

		public bool noTunnel = false; // only for maze gen

		public MapTile()
		{
			// intentionally left blank
		}

		// for use with KnightMoves(), DodgeMoves(), LeapMoves()
		public HashSet<MapTile> GetPossibleMoves(HashSet<Loc> options)
		{
			var result = new HashSet<MapTile>(new MapTileComp());

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

		public static HashSet<MapTile> FilterOutClear(HashSet<MapTile> ts)
		{
			return ts.Where(t => !t.clear).ToTileHashSet();
		}

		public static HashSet<MapTile> Tunnelable(HashSet<MapTile> ts)
		{
			return ts.Where(t => t.noTunnel == false).ToTileHashSet();
		}

		private static Random rng = new Random();

		public static MapTile RandomFromList(HashSet<MapTile> tileList)
		{
			return tileList.ElementAt(rng.Next(tileList.Count));
		}

		public MapTile OneNorth()
		{
			var newLoc = Loc.AddPts(loc, Dir.North);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public MapTile OneSouth()
		{
			var newLoc = Loc.AddPts(loc, Dir.South);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public MapTile OneEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.East);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public MapTile OneWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.West);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public MapTile OneNorthEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.NorthEast);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public MapTile OneSouthEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.SouthEast);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public MapTile OneNorthWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.NorthWest);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}

		public MapTile OneSouthWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.SouthWest);
			return (Refs.m.ValidLoc(newLoc)) ? Refs.m.TileByLoc(newLoc) : null;
		}
	}
}