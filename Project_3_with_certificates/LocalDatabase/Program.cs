using ClientApp;
using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace LocalDatabase
{
	public class Program
	{
		static void Main(string[] args)
		{
			string srvCertCN = "wcfservice";
			MarkChange markChange = Help.HelpForChange;

			LocalDataBaseNotification notification = new LocalDataBaseNotification();
			// Ovo je putanja do bin/Debug
			string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
			List<Measurement>[] array = new List<Measurement>[10];
			notification.ClearFiles(folderPath);

			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
			EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:8001/IDBConnection"),
									  new X509CertificateEndpointIdentity(srvCert));
			using (WCFClient proxy = new WCFClient(binding, address))
			{

				proxy.TestCommunication();
				List<Measurement> temp = proxy.DataTransfer();

				string databaseFileName = "measuredDataForRegion";
				File.WriteAllText(databaseFileName, string.Empty);

				DataBase db = new DataBase();
				ServiceHost host = new ServiceHost(typeof(Connection));

				// Podešavanja za RBAC autorizaciju
				host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();
				host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
				List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
				policies.Add(new CustomAuthorizationPolicy());
				host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

				try
				{
					// Konekcija lokal - klijent
					host.Open();
					Console.WriteLine("The user who started the server: " + WindowsIdentity.GetCurrent().Name);
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

					string allregions = "";
					int count = 0;
					for (int i = 0; i < region.Length; i++)
					{
						if (region != null)
						{
							if (region.Length == 1)
							{
								foreach (var item in connection.specificRegionList)
								{
									db.WriteMeasurementToFile(item, databaseFileName + region[0] + ".txt");
									markChange.fileName = databaseFileName + region[0] + ".txt";
								}
							}

							if (region.Length > 1)
							{
								allregions += region[i];
								count++;
							}
						}
					}

					if (count != 0)
					{
						// Kreiranje lokalne baze
						foreach (var item in connection.specificRegionList)
						{
							db.WriteMeasurementToFile(item, databaseFileName + allregions + ".txt");
							markChange.fileName = databaseFileName + allregions + ".txt";
						}
					}

					markChange.dataToModify = temp;
					markChange.listWithNewEntities = temp;
					markChange.listWithDeletedEntities = temp;
					markChange.counterForID = temp.Max(entity => entity.Id);

					while (true)
					{
						if (markChange.dataChanged)
						{
							proxy.UpdatingCentralDataBase(markChange.dataToModify);
							markChange.dataChanged = false;
						}
						else if (markChange.newEntitiesAdded)
						{
							proxy.AddNewEntityToCentralDB(markChange.listWithNewEntities);
							markChange.newEntitiesAdded = false;
						}
						else if (markChange.entitiesDeleted)
						{
							proxy.DeleteEntityFromCentralDB(markChange.listWithDeletedEntities);
							markChange.entitiesDeleted = false;
						}
						else if (markChange.notification)
						{
							string[] txtFiles = notification.GetTxtFilesInFolder(folderPath);
							if (txtFiles.Length > 1)
							{
								int ik = 0;
								foreach (var fileName in txtFiles)
								{
									if (fileName != null)
									{
										array[ik] = notification.ReadFileContent(fileName);
									}
									ik++;
								}
								notification.Synchronization(ik, array, txtFiles);
							}
							markChange.notification = false;
						}
						System.Threading.Thread.Sleep(3000);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine("[ERROR] {0}", e.Message);
					Console.ReadLine();
				}
				finally
				{
					host.Close();
				}
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
