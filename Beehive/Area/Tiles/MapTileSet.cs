using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

namespace Beehive
{
	[Serializable()]
	public class MapTileSet : HashSet<MapTile>
	{
		public MapTileSet() : base(new MapTileComp())
		{
		}

		public MapTileSet(IEnumerable<MapTile> source) : base(source, new MapTileComp())
		{
		}

		protected MapTileSet(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			Console.WriteLine("note: we were probably supposed to do something serialisation related in MapTileSet...");
		}

		internal void FilterNavHazards(MapTileSet maybeTiles)
		{
			// filter tiles containing wall
			maybeTiles = maybeTiles.Where(t => t.clear).ToMapTileSet();

			// don't move directly onto player
			maybeTiles = maybeTiles.Where(t => t.loc != Refs.p.loc).ToMapTileSet();

			// or right next to the player!
			Loc playerLoc = Refs.p.loc;
			MapTile playerTile = Refs.m.TileByLoc(playerLoc);

			var grabRange = playerTile.GetPossibleMoves(Dir.Cardinals);
			foreach (MapTile g in grabRange)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != g.loc).ToMapTileSet();
			}

			// don't move directly onto another cubi
			foreach (Cubi c in Refs.h.roster)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != c.loc).ToMapTileSet();
			}

			// and don't move directly onto a pent!
			foreach (Loc pent in Refs.m.pents)
			{
				maybeTiles = maybeTiles.Where(t => t.loc != pent).ToMapTileSet();
			}
		}
	}
}