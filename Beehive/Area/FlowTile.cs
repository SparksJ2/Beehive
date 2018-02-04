namespace Beehive
{
	public class FlowTile
	{
		public double flow;
		public bool mask;
		public Loc loc;
		public Flow myFlowMap;

		public FlowTile(Loc p, Flow f)
		{
			loc = p;
			myFlowMap = f;
		}

		// todo de-duplicate with Tile class
		public FlowTile OneNorth()
		{
			var newLoc = Loc.AddPts(loc, Dir.North);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}

		public FlowTile OneSouth()
		{
			var newLoc = Loc.AddPts(loc, Dir.South);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}

		public FlowTile OneEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.East);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}

		public FlowTile OneWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.West);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}
	}
}