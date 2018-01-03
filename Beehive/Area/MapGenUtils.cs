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
		public HashSet<Tile> GetNextTo(Tile t)
		{
			var x = new HashSet<Tile>(new TileComp())
			{ t.OneNorth(), t.OneSouth(), t.OneEast(), t.OneWest() };

			x.RemoveWhere(i => i == null);
			// leave border intact
			x.RemoveWhere(i => i.loc.X == 0);
			x.RemoveWhere(i => i.loc.Y == 0);
			x.RemoveWhere(i => i.loc.X == xLen - 1);
			x.RemoveWhere(i => i.loc.Y == yLen - 1);
			return x;
		}

		public HashSet<Tile> GetClosed3Sides(HashSet<Tile> input)
		{
			var r = new HashSet<Tile>(new TileComp());
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

		internal void MakeClearArea(Point point1, Point point2)
		{
			var workingList = TileList();
			workingList = workingList.Where(t =>
				t.loc.X > point1.X &&
				t.loc.X < point2.X).ToTileHashSet();

			workingList = workingList.Where(t =>
				t.loc.Y > point1.Y &&
				t.loc.Y < point2.Y).ToTileHashSet();

			foreach (Tile t in workingList) { t.clear = true; }
		}

		internal void MarkNoTunnel(Point point1, Point point2)
		{
			var workingList = TileList();
			workingList = workingList.Where(t =>
				t.loc.X > point1.X &&
				t.loc.X < point2.X).ToTileHashSet();

			workingList = workingList.Where(t =>
				t.loc.Y > point1.Y &&
				t.loc.Y < point2.Y).ToTileHashSet();

			foreach (Tile t in workingList) { t.noTunnel = true; }
		}

		public HashSet<Tile> GetClosed5Sides(HashSet<Tile> input)
		{
			var r = new HashSet<Tile>(new TileComp());
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
		private HashSet<Tile> clearCache;

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
			clearCache = TileList().Where(t => t.clear).ToHashSet(new TileComp());
		}

		public HashSet<Tile> GetClearTilesCache()
		{
			return clearCache;
			//return TileList().Where(t => t.clear).ToList();
		}
	}
}