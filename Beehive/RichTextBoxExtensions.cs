using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beehive
{
	public static class RichTextBoxExtensions
	{
		public static void FancyAppendText(this RichTextBox box, string text,
			Color color, HorizontalAlignment align)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;
			CrankyMonoSetColor(box, color);
			box.SelectionAlignment = align;
			box.AppendText(text);
			CrankyMonoSetColor(box, box.ForeColor);
		}

		private static bool alreadyToldYou = false;

		private static void CrankyMonoSetColor(RichTextBox box, Color color)
		{
			try
			{
				// Mono frequently throws an exception on setting colors in
				//    RichTextBoxs, but does it anyway, so here is the workaround.
				box.SelectionColor = box.ForeColor;
			}
			catch (Exception ex)
			{
				// ¯\_(ツ)_/¯
				if (!alreadyToldYou)
				{
					Console.WriteLine("SelectionColor fail: " + ex.Message);
					alreadyToldYou = true;
				}
			}
		}
	}
}