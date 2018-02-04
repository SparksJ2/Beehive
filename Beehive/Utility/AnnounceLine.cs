using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beehive
{
	internal class AnnounceLine
	{
		public string say;
		public HorizontalAlignment align;
		public Color color;

		public AnnounceLine(string sayIn, HorizontalAlignment alignIn, Color colIn)
		{
			say = sayIn;
			align = alignIn;
			color = colIn;
		}
	}
}