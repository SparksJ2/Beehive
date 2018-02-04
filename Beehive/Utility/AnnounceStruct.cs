using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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