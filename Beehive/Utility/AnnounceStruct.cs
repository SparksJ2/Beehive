using System.Drawing;
using System.Windows.Forms;

namespace Beehive
{
	internal class AnnounceStruct
	{
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