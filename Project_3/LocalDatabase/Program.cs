using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;

namespace LocalDatabase
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Konekcija lokal - central DB
			ChannelFactory<IDBConnection> channel = new ChannelFactory<IDBConnection>("DBConnection");
			IDBConnection proxy = channel.CreateChannel();
			List<Measurement> temp = proxy.DataTransfer();

			string databaseFileName = "measuredDataForRegion.txt";
			File.WriteAllText(databaseFileName, string.Empty);


			DataBase db = new DataBase();
			ServiceHost host = new ServiceHost(typeof(Connection));
			try
			{
				// Konekcija lokal - klijent
				host.Open();
				Console.WriteLine("The user who started the server:" + WindowsIdentity.GetCurrent().Name);
				Console.WriteLine();

				// Unos regiona
				string option;
				do
				{
					Console.WriteLine("Enter the region or regions for which you want the local base to be formed: ");
					option = Console.ReadLine();

					string[] words = option.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

					bool validInput = true;

					if (words.Length == 1 || words.Length >= 2)
					{
						foreach (var word in words)
						{
							if (word.Any(char.IsDigit) || string.IsNullOrWhiteSpace(word) || word.StartsWith(" "))
							{
								validInput = false;
								break;
							}
						}
					}
					else
					{
						validInput = false;
					}

					if (validInput)
					{
						break;
					}
					Console.WriteLine("Error!! Enter either one region or several regions in the format 'region,region' without numbers and unnecessary words!\n");
				} while (true);

				// Pozovem pomoćnu funkciju, da se parsira uneti text
				string[] region = ParseRegions(option);

				// Formiranje lokalne baye za dat region/regione
				List<Measurement> newList = temp;
				Connection connection = new Connection();

				foreach (var item in newList)
				{
					foreach (var i in region)
					{
						if (item.Region.Equals(i))
						{
							connection.specificRegionList.Add(item);
						}
					}
				}

				// Kreiranje lokalne baze
				foreach (var item in connection.specificRegionList)
				{
					db.WriteMeasurementToFile(item, databaseFileName);
				}
				Console.ReadKey();
			}
			finally
			{
				host.Close();
			}
		}

		// Funkcija koja parsira unet region ili regione
		public static string[] ParseRegions(string input)
		{
			string[] regions = input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (regions.Length == 1)
			{
				return new string[] { regions[0] };
			}
			if (regions.Length >= 2)
			{
				string[] retV = new string[regions.Length];
				for (int i = 0; i < regions.Length; i++)
				{
					retV[i] = regions[i].Trim();
				}

				return retV;
			}

			return null;
		}

	}
}
