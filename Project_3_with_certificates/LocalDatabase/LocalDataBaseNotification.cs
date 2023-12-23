using Common;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace LocalDatabase
{
	public class LocalDataBaseNotification
	{
		public string[] GetTxtFilesInFolder(string folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				throw new DirectoryNotFoundException($"The specified directory could not be found: {folderPath}");

			}

			string[] txtFiles = Directory.GetFiles(folderPath, "*.txt")
									  .Select(Path.GetFileName)
									  .ToArray();
			return txtFiles;
		}

		public List<Measurement> ReadFileContent(string fileName)
		{
			DataBase db = new DataBase();
			List<Measurement> retV = db.ReadMeasurementsFromFile(fileName);
			return retV;
		}

		public void Synchronization(int ik, List<Measurement>[] array, string[] txtFiles)
		{
			DataBase db = new DataBase();
			List<Measurement> newValue = new List<Measurement>();

			for (int i = 1; i < ik; i++)
			{
				List<Measurement> uporedi = array[1];
				List<Measurement> pom = array[i];

				for (int j = 1; j < pom.Count; j++)
				{
					for (int k = 0; k < uporedi.Count; k++)
					{
						if (uporedi[k].Region.Equals(pom[j].Region) && uporedi[k].Id != pom[j].Id)
						{
							if (!newValue.Contains(pom[j]))
							{
								newValue.Add(pom[j]);
							}
						}
					}
				}
			}

			for (int j = 0; j < newValue.Count; j++)
			{
				for (int i = 1; i < ik; i++)
				{
					if (!array[i].Contains(newValue[j]))
					{
						array[i].Add(newValue[j]);
					}
				}
			}

			for (int i = 1; i < ik; i++)
			{
				foreach (var item in array[i])
				{
					db.WriteMeasurementToFile(item, txtFiles[i]);
				}
			}
		}

		public void ClearFiles(string path)
		{
			// Provjeri postoji li zadani direktorij
			if (!Directory.Exists(path))
			{
				return;
			}

			// Prođi kroz sve datoteke s ekstenzijom ".txt" u direktoriju
			string[] txtFajlovi = Directory.GetFiles(path, "*.txt");
			foreach (string putanjaDoFajla in txtFajlovi)
			{
				// Preskoči prvu iteraciju, tj. prvi .txt file
				if (putanjaDoFajla == txtFajlovi[0])
				{
					continue;
				}

				// Otvori datoteku za pisanje (ako ne postoji, stvorit će se nova)
				using (StreamWriter writer = new StreamWriter(putanjaDoFajla))
				{
					// Upisivanje praznog stringa u datoteku
					writer.Write(string.Empty);
				}
			}
		}
	}
}