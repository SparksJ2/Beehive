using System;
using System.Drawing;
using System.Windows.Forms;

namespace Beehive
{
	public static class RichTextBoxExtensions
	{
		/// custom text box interface for dealing with color, alighment, and mono bugs

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
				//    RichTextBoxes, but does it anyway, so here is the workaround.
				box.SelectionColor = color;
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