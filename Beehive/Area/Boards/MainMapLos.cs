using System;
using System.Drawing;

namespace Beehive
{
	public partial class MainMap : BaseMap<MapTile>
	{
		/// line-of-sight calculations and glow effects

		internal void RunLos()
		{
			ResetLos();
			MapTile pt = TileByLoc(Refs.p.loc);

			foreach (MapTile t in tiles)
			{
				//if (MapTile.Distance(pt, t) <= 6) { t.los = true; }
				t.los = true;

				// this looks really inefficent but works okay on our small map
				for (double prog = 0.0; prog < 1.0; prog += 0.01)
				{
					int newx = pt.loc.X + Convert.ToInt32((t.loc.X - pt.loc.X) * prog);
					int newy = pt.loc.Y + Convert.ToInt32((t.loc.Y - pt.loc.Y) * prog);
					Loc testLoc = new Loc(newx, newy);

					if (Refs.m.TileByLoc(testLoc).clear == false)
					{
						t.los = false; break;
					}
				}
			}
		}

		internal void ResetLos()
		{
			foreach (MapTile t in tiles) { t.los = false; }
		}

		internal void RunGlows()
		{
			foreach (MapTile t in tiles) { t.backCol = Color.DarkSlateBlue; }

			foreach (Cubi c in Refs.h.roster) { AddGlow(c.loc, c.myColor); }

			AddGlow(Refs.p.loc, Refs.p.myColor);

			foreach (Loc pen in Refs.m.pents) { AddGlow(pen, Color.Purple); }
		}

		private static void AddGlow(Loc l, Color glowCol)
		{
			// todo what about overlapping glows?
			MapTileSet done = new MapTileSet();

			MapTile source = Refs.m.TileByLoc(l);

			source.backCol = GlowColOffset(source.backCol, glowCol, 0.5);
			done.Add(source);
			Glowinator(glowCol, done, source, 0.25);
		}

		private static void Glowinator(Color glowCol, MapTileSet done, MapTile from, double amt)
		{
			MapTileSet surround = from.GetPossibleMoves(Dir.AllAround);

			while (amt > 0.1)
			{
				MapTileSet newSurround = new MapTileSet();
				foreach (MapTile t in surround)
				{
					if (t.clear && !done.Contains(t))
					{
						t.backCol = GlowColOffset(t.backCol, glowCol, amt);
						done.Add(t);

						MapTileSet more = t.GetPossibleMoves(Dir.AllAround);
						foreach (MapTile m in more) { newSurround.Add(m); }
					}
				}
				surround = newSurround;
				amt -= 0.1;
			}
		}

		public static Color GlowColOffset(Color initial, Color glowCol, double amt)
		{
			//additive model
			int r = Convert.ToInt32(initial.R + (glowCol.R * amt * 0.5));
			int g = Convert.ToInt32(initial.G + (glowCol.G * amt * 0.5));
			int b = Convert.ToInt32(initial.B + (glowCol.B * amt * 0.5));

			// mixing model
			//int r = Convert.ToInt32((initial.R * (1 - amt)) + (glowCol.R * amt));
			//int g = Convert.ToInt32((initial.G * (1 - amt)) + (glowCol.G * amt));
			//int b = Convert.ToInt32((initial.B * (1 - amt)) + (glowCol.B * amt));

			if (r > 255) r = 255;
			if (g > 255) g = 255;
			if (b > 255) b = 255;

			return Color.FromArgb(r, g, b);
		}
	}
}