using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace LocalDatabase
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Konekcija lokalDB - centralDB
			ChannelFactory<IDBConnection> channel = new ChannelFactory<IDBConnection>("DBConnection");
			IDBConnection proxy = channel.CreateChannel();
			List<Measurement> temp = proxy.DataTransfer();
			MarkChange markChange = Help.HelpForChange;

			string databaseFileName = "measuredDataForRegion.txt";
			File.WriteAllText(databaseFileName, string.Empty);

			DataBase db = new DataBase();
			ServiceHost host = new ServiceHost(typeof(Connection));
			host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();
			host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new CustomAuthorizationPolicy());
			host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
			try
			{
				// Konekcija lokalDB - klijent
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

				// Parsiranje unetog texta za region
				string[] region = ParseRegions(option);

				// Formiranje lokalne baze za dat region/regione
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

				foreach (var item in connection.specificRegionList)
				{
					db.WriteMeasurementToFile(item, databaseFileName);
				}

				markChange.dataToModify = temp;
				markChange.listWithNewEntities = temp;
				markChange.listWithDeletedEntities = temp;

				while (true)
				{
					if (markChange.dataChanged)
					{
						proxy.UpdatingCentralDataBase(markChange.dataToModify);
					}
					else if (markChange.newEntitiesAdded)
					{
						proxy.AddNewEntityToCentralDB(markChange.listWithNewEntities);
					}
					else if (markChange.entitiesDeleted)
					{
						proxy.DeleteEntityFromCentralDB(markChange.listWithDeletedEntities);
					}

					System.Threading.Thread.Sleep(1000);
				}
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