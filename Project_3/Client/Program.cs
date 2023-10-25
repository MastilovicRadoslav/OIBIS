using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IConnection> channel = new ChannelFactory<IConnection>("ServiceName");
            IConnection proxy = channel.CreateChannel();

            while (true)
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Choose an option :");
                Console.WriteLine("\t1. Read");
                Console.WriteLine("\t2. Read, Calculate");
                Console.WriteLine("\t3. Read, Modify");
                Console.WriteLine("\t4. Read, Administrate");
                Console.WriteLine("\t0. Exit the application!");
                string option = Console.ReadLine();
                Console.WriteLine("--------------------------------");

                switch (option)
                {
                    case "1":
                        Console.WriteLine("\nPrint data from the local database :\n");
                        List<Measurement> listToPrint = proxy.PrintMeasurements();

                        foreach (var item in listToPrint)
                        {
                            Console.Write(item.ToString());
                        }
                        Console.WriteLine();
                        break;

                    case "2":
                        // Bira se opcija
                        Console.WriteLine("Choose whether to calculate the average consumption for the city or region :");
                        Console.WriteLine("\t1. City");
                        Console.WriteLine("\t2. Region");
                        string cityOrRegion = Console.ReadLine();
                        switch (cityOrRegion)
                        {
                            case "1":
                                // Unosi se grad
                                Console.WriteLine("Enter city name :");
                                string city = Console.ReadLine();
                                // Računa se potrošnja za grad i ispisuje se
                                Double meanForCity = proxy.CalculateConsumptionMeanCity(city);
                                Console.WriteLine("\nThe average value of spending for city " + city + " is : " + meanForCity + "\n");
                                break;

                            case "2":
                                // Unosi se region
                                Console.WriteLine("Enter a name for the region :");
                                string region = Console.ReadLine();
                                // Računa se potrošnja za region i ispisuje se
                                Double meanForRegion = proxy.CalculateConsumptionMeanRegion(region);
                                Console.WriteLine("\nThe average value of spending for region " + region + " is : " + meanForRegion + "\n");
                                break;
                        }
                        break;

                    case "3":
                        Console.WriteLine("Please enter your ID first :");
                        string id = Console.ReadLine();
                        Console.WriteLine("Enter a consumption value for the current month :");
                        string value = Console.ReadLine();
                        bool feedback = proxy.Modify(id, value);
                        if (feedback == true)
                        {
                            Console.WriteLine("The modification has been successfully made!!!");
                        }
                        else
                        {
                            Console.WriteLine("!!!!!!!!!!!!!");

                        }
                        break;
                    case "4":
						Console.WriteLine("Choose one option:");
						Console.WriteLine("\t1. Add Entities");
						Console.WriteLine("\t2. Delete Entities");
						string addOrDelete = Console.ReadLine();
						switch (addOrDelete)
						{
							case "1":
								// Dodavanje novog entiteta
								Console.WriteLine("Enter the following information:");

								int entityId;
								bool validId = false;
								do
								{
									Console.Write("ID: ");
									validId = int.TryParse(Console.ReadLine(), out entityId);
									if (!validId)
									{
										Console.WriteLine("Invalid ID. Please enter a valid integer.");
									}
								} while (!validId);

								Console.Write("Region: ");
								string region = Console.ReadLine();

								Console.Write("City: ");
								string city = Console.ReadLine();

								Console.Write("Year: ");
								string year = Console.ReadLine();

								Dictionary<string, string> consumption = new Dictionary<string, string>();

								for (int month = 1; month <= 12; month++)
								{
									Console.Write($"Enter consumption for Month {month}: ");
									string consumptionValue = Console.ReadLine();
									consumption[$"Month {month}"] = consumptionValue;
								}

								// Sada možete koristiti ove podatke za dodavanje novog entiteta u bazu podataka
								break;
							case "2":
								// Brisanje entiteta
								int idForDelete;
								bool validDeleteId = false;
								do
								{
									Console.Write("Please enter the ID you want to delete: ");
									validDeleteId = int.TryParse(Console.ReadLine(), out idForDelete);
									if (!validDeleteId)
									{
										Console.WriteLine("Invalid ID. Please enter a valid integer.");
									}
								} while (!validDeleteId);

								bool feedbackForDelete = proxy.DeleteEntity(idForDelete);
								if (feedbackForDelete)
								{
									Console.WriteLine("Deleted successfully!");
								}
								else
								{
									Console.WriteLine("Entity with that ID was not found.");
								}
								break;
						}
						break;

					case "0":
                        return;

                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }
    }
}