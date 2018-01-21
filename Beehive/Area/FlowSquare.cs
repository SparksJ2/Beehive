using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	public class FlowSquare
	{
		public int flow;
		public bool mask;
		public Loc loc;
		public Flow myFlowMap;

		public FlowSquare(Loc p, Flow f)
		{
			loc = p;
			myFlowMap = f;
		}

		// todo de-duplicate with Tile class
		public FlowSquare OneNorth()
		{
			var newLoc = Loc.AddPts(loc, Dir.North);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}

		public FlowSquare OneSouth()
		{
			var newLoc = Loc.AddPts(loc, Dir.South);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}

		public FlowSquare OneEast()
		{
			var newLoc = Loc.AddPts(loc, Dir.East);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}

		public FlowSquare OneWest()
		{
			var newLoc = Loc.AddPts(loc, Dir.West);
			return (Refs.m.ValidLoc(newLoc)) ? myFlowMap.FlowSquareByLoc(newLoc) : null;
		}
	}
}