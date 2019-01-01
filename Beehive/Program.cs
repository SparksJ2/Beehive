using System;
using System.Windows.Forms;

namespace Beehive
{
	public static class Program
	{
		/// The main entry point for the application.
		[STAThread]
		public static void Main()
		{
			// important windows forms setup stuff.
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// everthing runs from here
			Application.Run(new MainForm());
		}
	}
}