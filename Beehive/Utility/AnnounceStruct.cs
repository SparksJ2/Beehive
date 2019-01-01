using System.Drawing;
using System.Windows.Forms;

namespace Beehive
{
	internal class AnnounceStruct
	{
		/// convenient structure for holding text strings that have color and
		///    left / right alignment

		public string say;
		public HorizontalAlignment align;
		public Color color;

		public AnnounceStruct(string sayIn, HorizontalAlignment alignIn, Color colIn)
		{
			say = sayIn;
			align = alignIn;
			color = colIn;
		}
	}
}