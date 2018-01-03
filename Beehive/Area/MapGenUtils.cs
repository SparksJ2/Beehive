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
			var x = new List<Tile> { t.OneNorth(), t.OneSouth(), t.OneEast(), t.OneWest() };
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
				if (t.OneNorth() == null || t.OneNorth().clear == false) sum++;
				if (t.OneSouth() == null || t.OneSouth().clear == false) sum++;
				if (t.OneEast() == null || t.OneEast().clear == false) sum++;
				if (t.OneWest() == null || t.OneWest().clear == false) sum++;
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
				if (t.OneNorth() == null || t.OneNorth().clear == false) sum++;
				if (t.OneSouth() == null || t.OneSouth().clear == false) sum++;
				if (t.OneEast() == null || t.OneEast().clear == false) sum++;
				if (t.OneWest() == null || t.OneWest().clear == false) sum++;

				if (t.OneNorthEast() == null || t.OneNorthEast().clear == false) sum++;
				if (t.OneSouthEast() == null || t.OneSouthEast().clear == false) sum++;
				if (t.OneNorthWest() == null || t.OneNorthWest().clear == false) sum++;
				if (t.OneSouthWest() == null || t.OneSouthWest().clear == false) sum++;

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

		public void DelFromClearTileCache(Tile t)
		{
			clearCache.Remove(t);
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