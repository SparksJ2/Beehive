namespace Beehive
{
	internal class ConvertTiles
	{
		/// turns MapTileSets into FlowTileSets and back again, used by AI.
		// todo can these classes be better integrated?

		static internal FlowTileSet FlowSquaresFromTileSet(MapTileSet tiles, FlowMap fm)
		{
			var result = new FlowTileSet();

			foreach (MapTile ti in tiles)
			{
				result.Add(fm.TileByLoc(ti.loc));
			}

			return result;
		}

		static internal MapTileSet TileSetFromFlowSquares(FlowTileSet squares)
		{
			var result = new MapTileSet();

			foreach (FlowTile fs in squares)
			{
				result.Add(Refs.m.TileByLoc(fs.loc));
			}

			return result;
		}
	}
}