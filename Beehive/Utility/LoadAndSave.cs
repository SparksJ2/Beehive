using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Beehive
{
	internal class LoadAndSave
	{
		public static void SaveGame()
		{
			Refs.mf.Announce("Saving game...", Refs.p.myAlign, Refs.p.myColor);
			Stream TestFileStream = File.Create("map.bin");
			BinaryFormatter serializer = new BinaryFormatter();
			serializer.Serialize(TestFileStream, Refs.m);
			TestFileStream.Close();

			Stream TestFileStream2 = File.Create("player.bin");
			BinaryFormatter serializer2 = new BinaryFormatter();
			serializer2.Serialize(TestFileStream2, Refs.p);
			TestFileStream2.Close();

			Stream TestFileStream3 = File.Create("harem.bin");
			BinaryFormatter serializer3 = new BinaryFormatter();
			serializer3.Serialize(TestFileStream3, Refs.h);
			TestFileStream3.Close();
		}

		public static void LoadGame()
		{
			Refs.mf.Announce("Loading game...", Refs.p.myAlign, Refs.p.myColor);

			string FileName = "map.bin";
			if (File.Exists(FileName))
			{
				Stream TestFileStream = File.OpenRead(FileName);
				BinaryFormatter deserializer = new BinaryFormatter();
				Refs.m = (MainMap)deserializer.Deserialize(TestFileStream);
				TestFileStream.Close();
			}

			FileName = "player.bin";
			if (File.Exists(FileName))
			{
				Stream TestFileStream = File.OpenRead(FileName);
				BinaryFormatter deserializer = new BinaryFormatter();
				Refs.p = (Player)deserializer.Deserialize(TestFileStream);
				TestFileStream.Close();
			}

			FileName = "harem.bin";
			if (File.Exists(FileName))
			{
				Stream TestFileStream = File.OpenRead(FileName);
				BinaryFormatter deserializer = new BinaryFormatter();
				Refs.h = (Harem)deserializer.Deserialize(TestFileStream);
				TestFileStream.Close();
			}

			Refs.p.UpdateInventory();
			Refs.mf.UpdateMap();

			Refs.mf.Announce("Loaded game at " + Refs.p.turnCounter + " turns in.", Refs.p.myAlign, Refs.p.myColor);
		}
	}
}