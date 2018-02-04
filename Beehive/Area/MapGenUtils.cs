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
		public HashSet<MapTile> GetNextTo(MapTile t)
		{
			var x = new HashSet<MapTile>(new MapTileComp())
			{ t.OneNorth(), t.OneSouth(), t.OneEast(), t.OneWest() };

			x.RemoveWhere(i => i == null);
			// leave border intact
			x.RemoveWhere(i => i.loc.X == 0);
			x.RemoveWhere(i => i.loc.Y == 0);
			x.RemoveWhere(i => i.loc.X == xLen - 1);
			x.RemoveWhere(i => i.loc.Y == yLen - 1);
			return x;
		}

		public HashSet<MapTile> GetClosed3Sides(HashSet<MapTile> input)
		{
			var r = new HashSet<MapTile>(new MapTileComp());
			foreach (MapTile t in input)
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

		internal void MakeClearArea(Loc point1, Loc point2)
		{
			// todo note clears the area inside, not including the boundary
			var workingList = TileList();
			workingList = workingList.Where(t =>
				t.loc.X > point1.X &&
				t.loc.X < point2.X).ToTileHashSet();

			workingList = workingList.Where(t =>
				t.loc.Y > point1.Y &&
				t.loc.Y < point2.Y).ToTileHashSet();

			foreach (MapTile t in workingList) { t.clear = true; }
		}

		internal void MarkNoTunnel(Loc point1, Loc point2)
		{
			var workingList = TileList();
			workingList = workingList.Where(t =>
				t.loc.X > point1.X &&
				t.loc.X < point2.X).ToTileHashSet();

			workingList = workingList.Where(t =>
				t.loc.Y > point1.Y &&
				t.loc.Y < point2.Y).ToTileHashSet();

			foreach (MapTile t in workingList) { t.noTunnel = true; }
		}

		public HashSet<MapTile> GetClosed5Sides(HashSet<MapTile> input)
		{
			var r = new HashSet<MapTile>(new MapTileComp());
			foreach (MapTile t in input)
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
		private HashSet<MapTile> clearCache;

		public void AddToClearTileCache(MapTile t)
		{
			clearCache.Add(t);
		}

		public void DelFromClearTileCache(MapTile t)
		{
			clearCache.Remove(t);
		}

		public void InitClearTilesCache()
		{
			clearCache = TileList().Where(t => t.clear).ToHashSet(new MapTileComp());
		}

		public HashSet<MapTile> GetClearTilesCache()
		{
			return clearCache;
			//return TileList().Where(t => t.clear).ToList();
		}
	}
}