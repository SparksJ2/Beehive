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
	public class Map
	{
		public int xLen;
		public int yLen;
		public Tile[,] tiles;

		public Map(int xIn, int yIn)
		{
			xLen = xIn;
			yLen = yIn;

			tiles = new Tile[xLen, yLen];

			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					this.tiles[x, y] = new Tile();
					tiles[x, y].loc.X = x;
					tiles[x, y].loc.Y = y;
				}
			}
		}

		public List<Tile> TileList()
		{
			var tList = new List<Tile>();
			foreach (Tile t in tiles) { tList.Add(t); }
			return tList;
		}

		public List<Tile> ClearTiles()
		{
			var tList = new List<Tile>();
			foreach (Tile t in tiles)
			{
				if (t.clear) tList.Add(t); // todo use Where
			}
			return tList;
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

		public Tile OneNorthEast(Tile t)
		{
			var loc = new Point(t.loc.X + 1, t.loc.Y - 1);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneSouthEast(Tile t)
		{
			var loc = new Point(t.loc.X + 1, t.loc.Y + 1);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneNorthWest(Tile t)
		{
			var loc = new Point(t.loc.X - 1, t.loc.Y - 1);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneSouthWest(Tile t)
		{
			var loc = new Point(t.loc.X - 1, t.loc.Y + 1);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

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

		public Tile OneNorth(Tile t)
		{
			var loc = new Point(t.loc.X, t.loc.Y - 1);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneSouth(Tile t)
		{
			var loc = new Point(t.loc.X, t.loc.Y + 1);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneEast(Tile t)
		{
			var loc = new Point(t.loc.X + 1, t.loc.Y);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Tile OneWest(Tile t)
		{
			var loc = new Point(t.loc.X - 1, t.loc.Y);
			return (ValidLoc(loc)) ? TileByLoc(loc) : null;
		}

		public Bitmap AsBitmap(Player p, Cubi s)
		{
			Bitmap bmp = new Bitmap((int)(800), (int)(400));
			Graphics gr = Graphics.FromImage(bmp);

			// clear the canvas to dark
			Rectangle pgRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			SolidBrush solidBlack = new SolidBrush(Color.DarkSlateBlue);
			gr.FillRectangle(solidBlack, pgRect);

			// frame the image with a black border
			Rectangle rc = new Rectangle(5, 5, bmp.Width - 10, bmp.Height - 10);
			gr.DrawRectangle(new Pen(Color.White, 4), rc);

			// add floor stuff
			for (int x = 0; x < xLen; x++)
			{
				for (int y = 0; y < yLen; y++)
				{
					var t = tiles[x, y];
					if (!t.clear) AddChar(bmp, x, y, t.gly.ToString());
				}
			}

			AddChar(bmp, p.loc.X, p.loc.Y, "♂");
			AddChar(bmp, s.loc.X, s.loc.Y, "☿");

			return bmp;
		}

		public static void AddChar(Bitmap bmp, int x, int y, string s)
		{
			int multX = 12;
			int multY = 15;
			int edgeX = 10;
			int edgeY = 12;

			int x1 = (x * multX) + edgeX;
			int y1 = (y * multY) + edgeY;
			int x2 = multX;
			int y2 = multY;

			// Create a rectangle for the entire bitmap
			RectangleF rectf = new RectangleF(x1, y1, x2, y2);

			// Create graphic object that will draw onto the bitmap
			Graphics g = Graphics.FromImage(bmp);
			Graphics g2 = Graphics.FromImage(bmp);
			g2.FillRectangle(Brushes.DarkSlateGray, rectf);
			g2.Flush();

			// ------------------------------------------
			// Ensure the best possible quality rendering
			// ------------------------------------------
			// The smoothing mode specifies whether lines, curves, and the edges of filled areas use smoothing (also called antialiasing). One exception is that path gradient brushes do not obey the smoothing mode. Areas filled using a PathGradientBrush are rendered the same way (aliased) regardless of the SmoothingMode property.
			g.SmoothingMode = SmoothingMode.HighQuality;

			// The interpolation mode determines how intermediate values between two endpoints are calculated.
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			// Use this property to specify either higher quality, slower rendering, or lower quality, faster rendering of the contents of this Graphics object.
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;

			// This one is important
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			// Create string formatting options (used for alignment)
			StringFormat format = new StringFormat()
			{
				Alignment = StringAlignment.Center,
				LineAlignment = StringAlignment.Center
			};

			// Draw the text onto the image
			//  ⌑  Unicode Character 'SQUARE LOZENGE'(U + 2311)

			int gSize = 11;
			string useFont = "Courier";
			if (s == "♂")
			{
				gSize = 8;
				useFont = "Symbola";
			}
			if (s == "☿")
			{
				gSize = 11;
				useFont = "Symbola";
			}

			g.DrawString(s, new Font(useFont, gSize), Brushes.White, rectf, format);
			g.Flush();
		}

		public void ConsoleDump()
		{
			Debug.WriteLine("+--------------------+");
			for (int y = 0; y < yLen; y++)
			{
				var rowofchars = "";
				for (int x = 0; x < xLen; x++)
				{
					var t = tiles[x, y];
					if (t.clear)
						rowofchars += " ";
					else
						rowofchars += t.gly;
				}
				Debug.WriteLine("|" + rowofchars + "|");
			}
			Debug.WriteLine("+--------------------+");
		}

		public Tile TileByLoc(Point p)
		{
			return tiles[p.X, p.Y];
		}

		public bool ValidLoc(Point p)
		{
			return (p.X >= 0 && p.X < xLen && p.Y >= 0 && p.Y < yLen) ? true : false;
		}

		public bool IsEdge(Point p)
		{
			return (p.X == 0 || p.X == xLen - 1 ||
				p.Y == 0 || p.Y == yLen - 1) ? true : false;
		}

		public bool IsSolid(Tile t)
		{
			return (t == null || t.clear == false);
		}

		public void HealWalls()
		{
			foreach (Tile t in tiles)
			{
				var n = IsSolid(OneNorth(t));
				var s = IsSolid(OneSouth(t));
				var e = IsSolid(OneEast(t));
				var w = IsSolid(OneWest(t));

				// ┌─┬┐  ╔═╦╗  ╓─╥╖  ╒═╤╕
				// │ ││  ║ ║║  ║ ║║  │ ││
				// ├─┼┤  ╠═╬╣  ╟─╫╢  ╞═╪╡
				// └─┴┘  ╚═╩╝  ╙─╨╜  ╘═╧╛

				if (n && s && e && w) t.gly = '╬';

				if (!n && s && e && w) t.gly = '╦';
				if (n && !s && e && w) t.gly = '╩';
				if (n && s && !e && w) t.gly = '╣';
				if (n && s && e && !w) t.gly = '╠';

				if (n && s && !e && !w) t.gly = '║';
				if (!n && !s && e && w) t.gly = '═';

				if (!n && s && e && !w) t.gly = '╔';
				if (!n && s && !e && w) t.gly = '╗';
				if (n && !s && !e && w) t.gly = '╝';
				if (n && !s && e && !w) t.gly = '╚';

				if (!n && s && !e && !w) t.gly = '║';
				if (n && !s && !e && !w) t.gly = '║';
				if (!n && !s && e && !w) t.gly = '═';
				if (!n && !s && !e && w) t.gly = '═';

				//if (!n && s && !e && !w) t.gly = '╥';
				//if (n && !s && !e && !w) t.gly = '╨';
				//if (!n && !s && e && !w) t.gly = '╞';
				//if (!n && !s && !e && w) t.gly = '╡';

				//if (!n && !s && !e && !w) t.gly = '╳';
				if (!n && !s && !e && !w) t.gly = 'X';
			}
		} // end healwalls

		public bool Touching(Player p, Cubi s)
		{
			double a = Math.Pow(p.loc.X - s.loc.X, 2);
			double b = Math.Pow(p.loc.Y - s.loc.Y, 2);
			double c = Math.Sqrt(a + b);
			Console.WriteLine(c);
			return (c < 1.01);
		}
	}
}

//public static void AddText(Bitmap bmp, int x, int y, string s)
//{
//	// note: doesn't work yet
//	Graphics g = Graphics.FromImage(bmp);
//	Point p = new Point(x*10, y*10);

//	TextRenderer.DrawText(g, s, new Font("Symbola", 10), p, Color.White, Color.Black);
//}

//public static void AddCharTest(Bitmap bmp, int x, int y, string s, int size)
//{
//	RectangleF rectf = new RectangleF(x, y, 700, 35);
//	Graphics txt = Graphics.FromImage(bmp);
//	Graphics back = Graphics.FromImage(bmp);
//	back.FillRectangle(Brushes.Purple, rectf);
//	back.Flush();

//	txt.SmoothingMode = SmoothingMode.HighQuality;
//	txt.InterpolationMode = InterpolationMode.HighQualityBicubic;
//	txt.PixelOffsetMode = PixelOffsetMode.HighQuality;
//	txt.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
//	StringFormat format = new StringFormat()
//	{
//		Alignment = StringAlignment.Near,
//		LineAlignment = StringAlignment.Near
//	};
//	txt.DrawString(s, new Font("Symbola", size), Brushes.White, rectf, format);
//	txt.Flush();

//}

//int y = 20;

//AddCharTest(bmp, 30, y, "┌─┬┐╔═╦╗╓─╥╖╒═╤╕", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "│X││║X║║║X║║│X││", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "├─┼┤╠═╬╣╟─╫╢╞═╪╡", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "└─┴┘╚═╩╝╙─╨╜╘═╧╛", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "XXXXXXXXXXXXXXXX", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "♈♉♊♋♌♍♎♏♐♑♒♓⛎", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "☿♁♂♃♄♅♆♇", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "⚳⚴⚵⚶⚷", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "⚸♥★⛓⛏", 24); y = y + 40;
//AddCharTest(bmp, 30, y, "♂♀⚲⚢⚣⚤⚥⚦⚧⚨⚩⌑", 24); y = y + 40;