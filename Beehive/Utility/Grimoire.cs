using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Beehive
{
	public static class Grimoire
	{
		/// for loading and saving game options

		private static Dictionary<string, string> lore = new Dictionary<string, string>();

		public static void Load(string fileName)
		{
			try
			{
				lore.Clear();
				var lines = File.ReadAllLines(fileName);

				foreach (var l in lines)
				{
					if (string.IsNullOrWhiteSpace(l))
					{
						continue;
					}
					else if (l.Contains('='))
					{
						int splitter = l.IndexOf('=');
						string key = l.Substring(0, splitter).Trim();
						string value = l.Substring(splitter + 1).Trim();
						lore.Add(key, value);
					}
					else
					{
						// must be a comment, then!
					}
				}
			}
			catch (Exception)
			{
				MessageBox.Show("Failed to load .ini file named " + fileName);
			}
		}

		public static void Save(string fileName)
		{
			try
			{
				var sb = new StringBuilder("");
				sb.AppendLine();
				foreach (var entry in lore)
				{
					sb.AppendFormat("{0} = {1}", entry.Key, entry.Value);
					sb.AppendLine();
				}
				sb.AppendLine(); sb.AppendLine();

				File.WriteAllText(fileName, sb.ToString());
			}
			catch (Exception)
			{
				MessageBox.Show("Failed to save .ini file named " + fileName);
			}
		}

		public static void FillCubi(Cubi c)
		{
			string idStr = c.myIdNo.ToString();

			c.name = GetValueString(idStr, "CubiName");
			c.glyph = GetValueString(idStr, "CubiChar");
			c.myColor = GetValueColor(idStr, "CubiColor");
			c.myStdAi = GetValueStdAi(idStr, "CubiStdAi");
			c.myJbAi = GetValueJbAi(idStr, "CubiJbAi");
			c.myHome = GetValueHome(idStr, "CubiHome");
		}

		public static string GetValueString(string id, string key)
		{
			if (lore[key + id] == null) MessageBox.Show("Warning didn't find ini key named " + key + id);
			return lore[key + id];
		}

		public static int GetValueInt(string id, string key)
		{
			if (lore[key + id] == null) MessageBox.Show("Warning didn't find ini key named " + key + id);
			int.TryParse(lore[key + id], out int i);
			return i;
		}

		public static bool GetValueBool(string id, string key)
		{
			if (lore[key + id] == null) MessageBox.Show("Warning didn't find ini key named " + key + id);
			bool.TryParse(lore[key + id].TitleCase(), out bool i);
			return i;
		}

		public static Color GetValueColor(string id, string key)
		{
			if (lore[key + id] == null)
				MessageBox.Show("Warning didn't find ini key named " + key + id);

			Color found = Color.FromName(lore[key + id]);

			if (found.R + found.G + found.B == 0)
				MessageBox.Show("Warning didn't find color named " + key + id);

			return found;
		}

		public static CubiStdAi GetValueStdAi(string id, string key)
		{
			if (lore[key + id] == "CubiAi.FleeToRing")
			{
				return new CubiStdAi(CubiAi.FleeToRing);
			}
			else if (lore[key + id] == "CubiAi.FlowOutAndBack")
			{
				return new CubiStdAi(CubiAi.FlowOutAndBack);
			}
			else
			{
				MessageBox.Show("Warning didn't find ini key named " + key + id);
				return null;
			}
		}

		public static CubiJbAi GetValueJbAi(string id, string key)
		{
			if (lore[key + id] == "CubiAi.JailBreak")
			{
				return new CubiJbAi(CubiAi.JailBreak);
			}
			else
			{
				MessageBox.Show("Warning didn't find ini key named " + key + id);
				return null;
			}
		}

		public static Home GetValueHome(string id, string key)
		{
			string result = lore[key + id];
			if (lore[key + id] == null)
			{
				MessageBox.Show("Warning didn't find ini key named " + key + id);
			}
			else if (result == "Fire") { return Home.Fire; }
			else if (result == "Air") { return Home.Air; }
			else if (result == "Water") { return Home.Water; }
			else if (result == "Earth") { return Home.Earth; }
			return Home.None;
		}

		public static void SetValue(string key, object value)
		{
			if (lore.ContainsKey(key))
			{
				// try not to get stuff out of order
				lore[key] = value.ToString();
			}
			else { lore.Add(key, value.ToString()); }
		}

		public static string TitleCase(this string s)
		{
			TextInfo defaultTI = CultureInfo.InvariantCulture.TextInfo;
			return defaultTI.ToTitleCase(s.ToLowerInvariant());
		}
	}
}