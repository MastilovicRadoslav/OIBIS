using Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;

namespace Client
{
	public class Program
	{
		static void Main(string[] args)
		{
			ChannelFactory<IConnection> channel = new ChannelFactory<IConnection>("ServiceName");
			IConnection proxy = channel.CreateChannel();
			MarkChange markChange = Help.HelpForChange;

			Console.WriteLine("The user who started the client:" + WindowsIdentity.GetCurrent().Name + "\n");
			string user = WindowsIdentity.GetCurrent().Name.Split('\\')[1];

			while (true)
			{
				Console.WriteLine("-------------------------------");
				Console.WriteLine("Choose an option :");
				Console.WriteLine("\t1. Read all data from the local database");
				Console.WriteLine("\t2. Calculate the average consumption for a city or region");
				Console.WriteLine("\t3. Add consumption for the current month");
				Console.WriteLine("\t4. Administrate - Add a new entity and Delete an entity");
				Console.WriteLine("\t0. Exit the application!");
				string option = Console.ReadLine();
				Console.WriteLine("--------------------------------");

				switch (option)
				{
					case "1":   // Ispis podataka iz lokalne baze
						try
						{
							(List<byte[]>, byte[], byte[]) results = proxy.PrintMeasurements();
							byte[] yourKey = results.Item2;
							byte[] yourIV = results.Item3;

							(byte[], byte[]) retV = markChange.sm.KeyAndIVEncryption(yourKey, yourIV);
							byte[] decryptedyourKey = retV.Item1;
							byte[] decrypteyourIV = retV.Item2;


							Console.Write("Encrypted Data: \n");
							foreach (var encryptedData in results.Item1)
							{

								foreach (byte b in encryptedData)
								{
									Console.Write($"{b:X2} ");
								}

								string print = markChange.aes.DecryptStringFromBytes_Aes(encryptedData, decryptedyourKey, decrypteyourIV);
								Console.WriteLine($"\nDecrypted Data: \n{print}");
							}
						}
						catch
						{
							Console.WriteLine("The user [with username " + user + "] cannot call the Read function !!!");
						}
						break;

					case "2":   // Bira se opcija
						Console.WriteLine("Choose whether to calculate the average consumption for the city or region :");
						Console.WriteLine("\t1. City");
						Console.WriteLine("\t2. Region");
						string cityOrRegion = Console.ReadLine();
						switch (cityOrRegion)
						{
							case "1":   // Unosi se grad
								string city = "";
								do
								{
									Console.WriteLine("Enter city name: ");
									city = Console.ReadLine();

								} while (string.IsNullOrWhiteSpace(city) || city.Any(char.IsDigit) || ContainsMultipleSpaces(city));

								try
								{
									(byte[], byte[], byte[]) results = proxy.CalculateConsumptionMeanCity(city);
									byte[] yourKey = results.Item2;
									byte[] yourIV = results.Item3;

									(byte[], byte[]) retV = markChange.sm.KeyAndIVEncryption(yourKey, yourIV);
									byte[] decryptedyourKey = retV.Item1;
									byte[] decrypteyourIV = retV.Item2;

									Console.Write("\nEncrypted Data: \n");
									foreach (var encryptedData in results.Item1)
									{
										Console.Write($"{encryptedData:X2} ");
									}

									Console.WriteLine($"\nDecrypted Data:");
									string meanForCity = markChange.aes.DecryptStringFromBytes_Aes(results.Item1, decryptedyourKey, decrypteyourIV);
									Console.WriteLine("The average value of spending for city " + city + " is : " + meanForCity + "\n");
								}
								catch
								{
									Console.WriteLine("The user [with username " + user + "] cannot call the Calculate function !!!");
								}
								break;

							case "2":   // Unosi se region
								string region = "";
								do
								{
									Console.WriteLine("Enter a name for the region :");
									region = Console.ReadLine();

								} while (string.IsNullOrWhiteSpace(region) || region.Any(char.IsDigit) || ContainsMultipleSpaces(region));

								try
								{
									(byte[], byte[], byte[]) results = proxy.CalculateConsumptionMeanRegion(region);
									byte[] yourKey = results.Item2;
									byte[] yourIV = results.Item3;

									(byte[], byte[]) retV = markChange.sm.KeyAndIVEncryption(yourKey, yourIV);
									byte[] decryptedyourKey = retV.Item1;
									byte[] decrypteyourIV = retV.Item2;

									Console.Write("\nEncrypted Data: \n");
									foreach (var encryptedData in results.Item1)
									{
										Console.Write($"{encryptedData:X2} ");
									}

									Console.WriteLine($"\nDecrypted Data:");
									string meanForRegion = markChange.aes.DecryptStringFromBytes_Aes(results.Item1, decryptedyourKey, decrypteyourIV);
									Console.WriteLine("The average value of spending for region " + region + " is : " + meanForRegion + "\n");
								}
								catch
								{
									Console.WriteLine("The user [with username " + user + "] cannot call the Calculate function !!!");
								}
								break;

							default:
								Console.WriteLine("\nInvalid option !!!");
								break;
						}
						break;

					case "3":   // Modifikacija entiteta
						int id;
						string inputID = "";
						do
						{
							Console.WriteLine("Please enter your ID first: ");
							inputID = Console.ReadLine();

							if (int.TryParse(inputID, out id) && id >= 0)
							{
								break;
							}

							Console.WriteLine("\nInvalid input. Please enter a valid non-negative integer for your ID.\n");
						} while (true);

						double value;
						string inputValue = "";
						do
						{
							Console.WriteLine("Enter a consumption value for the current month :");
							inputValue = Console.ReadLine();

							if (double.TryParse(inputValue, out value) && value >= 0)
							{
								break;
							}

							Console.WriteLine("\nInvalid input. Please enter a valid non-negative double for the consumption value.\n");
						} while (true);

						try
						{
							(byte[], byte[], byte[]) results = proxy.Modify(inputID, inputValue);
							byte[] yourKey = results.Item2;
							byte[] yourIV = results.Item3;

							(byte[], byte[]) retV = markChange.sm.KeyAndIVEncryption(yourKey, yourIV);
							byte[] decryptedyourKey = retV.Item1;
							byte[] decrypteyourIV = retV.Item2;

							Console.Write("\nEncrypted Data: \n");
							foreach (var encryptedData in results.Item1)
							{
								Console.Write($"{encryptedData:X2} ");
							}

							Console.WriteLine($"\nDecrypted Data:");
							string feedbackS = markChange.aes.DecryptStringFromBytes_Aes(results.Item1, decryptedyourKey, decrypteyourIV);
							Console.WriteLine(feedbackS);

							bool feedback = bool.Parse(feedbackS);
							if (feedback == true)
							{
								Console.WriteLine("The modification has been successfully made !!!");
								proxy.ChangeInDB(true);
							}
							else
							{
								Console.WriteLine("!!!!!!!!!!!!!");
								proxy.ChangeInDB(false);
							}
						}
						catch
						{
							Console.WriteLine("The user [with username " + user + "] cannot call the Modify function !!!");
						}
						break;

					case "4":
						Console.WriteLine("Choose one option:");
						Console.WriteLine("\t1. Add Entities");
						Console.WriteLine("\t2. Delete Entities");
						string addOrDelete = Console.ReadLine();
						switch (addOrDelete)
						{
							case "1":   // Dodavanje novog entiteta
								Console.WriteLine("Enter the following information :");

								string region = "";
								do
								{
									Console.Write("Region : ");
									region = Console.ReadLine();

								} while (string.IsNullOrWhiteSpace(region) || region.Any(char.IsDigit) || ContainsMultipleSpaces(region));

								string city = "";
								do
								{
									Console.Write("City : ");
									city = Console.ReadLine();

								} while (string.IsNullOrWhiteSpace(city) || city.Any(char.IsDigit) || ContainsMultipleSpaces(city));

								string year = "";
								DateTime currentTime = DateTime.Now;
								do
								{
									Console.Write("Year: ");
									int currentYear = int.Parse(currentTime.ToString("yyyy"));

									year = Console.ReadLine();
									int yearInt = int.Parse(year);

									if (yearInt > currentYear || yearInt < 2020)
									{
										Console.WriteLine("The year you entered is incorrect. Please try again!\n");
									}
									else
									{
										break;
									}
								} while (true);

								Dictionary<string, string> consumption = new Dictionary<string, string>();
								string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

								foreach (string monthName in months)
								{
									string consumptionValue = "0";
									consumption[monthName] = consumptionValue;
								}

								string currentMonth = currentTime.ToString("MMMM");
								DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
								int currentMonthInt = formatInfo.MonthNames.ToList().IndexOf(currentMonth) + 1;
								int counter = 1;

								foreach (string monthName in months)
								{
									if (counter <= currentMonthInt)
									{
										string consumptionValue;
										decimal valueC;

										do
										{
											Console.Write($"Enter consumption for {monthName}: ");
											consumptionValue = Console.ReadLine();

										} while (!decimal.TryParse(consumptionValue, out valueC) || valueC < 0);
										consumption[monthName] = consumptionValue;
										counter++;
									}
									else
									{
										break;
									}
								}

								try
								{
									(byte[], byte[], byte[], int) results = proxy.AddNewEntity(region, city, year, consumption);
									byte[] yourKey = results.Item2;
									byte[] yourIV = results.Item3;
									int generatedID = results.Item4;

									(byte[], byte[]) retV = markChange.sm.KeyAndIVEncryption(yourKey, yourIV);
									byte[] decryptedyourKey = retV.Item1;
									byte[] decrypteyourIV = retV.Item2;

									Console.Write("\nEncrypted Data: \n");
									foreach (var encryptedData in results.Item1)
									{
										Console.Write($"{encryptedData:X2} ");
									}

									Console.WriteLine($"\nDecrypted Data:");
									string addedSuccessfullyS = markChange.aes.DecryptStringFromBytes_Aes(results.Item1, decryptedyourKey, decrypteyourIV);
									Console.WriteLine(addedSuccessfullyS);

									bool addedSuccessfully = bool.Parse(addedSuccessfullyS);

									if (addedSuccessfully)
									{
										Console.WriteLine("New entity successfully added to database !!!");
										proxy.NewEntitiesInDB(true, generatedID);
									}
									else
									{
										Console.WriteLine("!!!!!!!!!!");
										proxy.NewEntitiesInDB(false, generatedID);
									}
								}
								catch
								{
									Console.WriteLine("The user [with username " + user + "] cannot call the Administrate (Add) function !!!");
								}
								break;
							case "2":   // Brisanje entiteta
								bool validDeleteId = false;
								int idForDelete = 0;
								do
								{
									Console.Write("Please enter the ID you want to delete: ");
									validDeleteId = int.TryParse(Console.ReadLine(), out idForDelete);

									if (!validDeleteId || idForDelete < 0)
									{
										Console.WriteLine("\nInvalid ID. Please enter a valid non-negative integer.\n");
									}
								} while (!validDeleteId || idForDelete < 0);

								try
								{
									(byte[], byte[], byte[]) results = proxy.DeleteEntity(idForDelete);
									byte[] yourKey = results.Item2;
									byte[] yourIV = results.Item3;

									(byte[], byte[]) retV = markChange.sm.KeyAndIVEncryption(yourKey, yourIV);
									byte[] decryptedyourKey = retV.Item1;
									byte[] decrypteyourIV = retV.Item2;

									Console.Write("\nEncrypted Data: \n");
									foreach (var encryptedData in results.Item1)
									{
										Console.Write($"{encryptedData:X2} ");
									}

									Console.WriteLine($"\nDecrypted Data:");
									string feedbackForDeleteS = markChange.aes.DecryptStringFromBytes_Aes(results.Item1, decryptedyourKey, decrypteyourIV);
									Console.WriteLine(feedbackForDeleteS);
									bool feedbackForDelete = bool.Parse(feedbackForDeleteS);

									if (feedbackForDelete)
									{
										Console.WriteLine("Deleted successfully !!!");
										proxy.DeletedEntitiesInDB(true);
									}
									else
									{
										Console.WriteLine("!!!");
										proxy.DeletedEntitiesInDB(false);
									}
								}
								catch
								{
									Console.WriteLine("The user [with username " + user + "] cannot call the Administrate (Delete) function !!!");
								}
								break;
						}
						break;

					case "0":
						return;

					default:
						Console.WriteLine("Invalid option !!!");
						break;
				}
			}

			bool ContainsMultipleSpaces(string input)
			{
				for (int i = 0; i < input.Length - 1; i++)
				{
					if (input[i] == ' ' && input[i + 1] == ' ')
					{
						return true;
					}
				}
				return false;
			}
		}
	}
}