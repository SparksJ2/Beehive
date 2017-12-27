using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beehive
{
	public partial class Map
	{
		public List<Tile> GetNextTo(Tile t)
		{
			var x = new List<Tile> { OneNorth(t), OneSouth(t), OneEast(t), OneWest(t) };
			x.RemoveAll(i => i == null);
			// leave border intact
			x.RemoveAll(i => i.loc.X == 0);
			x.RemoveAll(i => i.loc.Y == 0);
			x.RemoveAll(i => i.loc.X == xLen - 1);
			x.RemoveAll(i => i.loc.Y == yLen - 1);
			return x;
		}

		public List<Tile> GetClosed3Sides(List<Tile> input)
		{
			var r = new List<Tile>();
			foreach (Tile t in input)
			{
				int sum = 0;
				if (OneNorth(t) == null || OneNorth(t).clear == false) sum++;
				if (OneSouth(t) == null || OneSouth(t).clear == false) sum++;
				if (OneEast(t) == null || OneEast(t).clear == false) sum++;
				if (OneWest(t) == null || OneWest(t).clear == false) sum++;
				if (sum >= 3) r.Add(t);
			}
			return r;
		}

		public List<Tile> GetClosed5Sides(List<Tile> input)
		{
			var r = new List<Tile>();
			foreach (Tile t in input)
			{
				int sum = 0;
				if (OneNorth(t) == null || OneNorth(t).clear == false) sum++;
				if (OneSouth(t) == null || OneSouth(t).clear == false) sum++;
				if (OneEast(t) == null || OneEast(t).clear == false) sum++;
				if (OneWest(t) == null || OneWest(t).clear == false) sum++;

				if (OneNorthEast(t) == null || OneNorthEast(t).clear == false) sum++;
				if (OneSouthEast(t) == null || OneSouthEast(t).clear == false) sum++;
				if (OneNorthWest(t) == null || OneNorthWest(t).clear == false) sum++;
				if (OneSouthWest(t) == null || OneSouthWest(t).clear == false) sum++;

				if (sum >= 5) r.Add(t);
			}
			return r;
		}

		// cached clear tiles list for maze generator. not for general use.
		private List<Tile> clearCache;

		public void AddToClearTileCache(Tile t)
		{
			clearCache.Add(t);
		}

		public void InitClearTilesCache()
		{
			clearCache = TileList().Where(t => t.clear).ToList();
		}

		public List<Tile> GetClearTilesCache()
		{
			return clearCache;
			//return TileList().Where(t => t.clear).ToList();
		}
	}
}