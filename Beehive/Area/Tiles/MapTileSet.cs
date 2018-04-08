using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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
	}
}