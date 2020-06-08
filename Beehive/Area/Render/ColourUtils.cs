using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beehive.Area.Render
{
	internal class ColourUtils
	{
		private static Color GetColorMix(MapTile mt)
		{
			// speculative -- not sure if I like nectar mixing
			Int32 mergeR = Refs.p.myColor.R;
			Int32 mergeG = Refs.p.myColor.G;
			Int32 mergeB = Refs.p.myColor.B;
			for (int nLoop = 1; nLoop < mt.nectarLevel.Length - 1; nLoop++) // skip player nectar
			{
				mergeR += Harem.GetId(nLoop).myColor.R;
				mergeG += Harem.GetId(nLoop).myColor.G;
				mergeB += Harem.GetId(nLoop).myColor.B;
			}
			double factor = 1 + Refs.h.roster.Count;
			mergeR = (Int32)(mergeR / factor);
			mergeG = (Int32)(mergeG / factor);
			mergeB = (Int32)(mergeB / factor);
			return Color.FromArgb(0, mergeR, mergeG, mergeB);
		}
	}
}