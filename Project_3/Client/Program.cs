using Common;
using System;
using System.Collections.Generic;
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

			proxy.AddUser("Dajana", "dajana");
			proxy.AddUser("Vanja", "vanja");
			proxy.AddUser("Radoslav", "radoslav");
			proxy.AddUser("Kristian", "kristian");

			Console.WriteLine("The user who started the client:" + WindowsIdentity.GetCurrent().Name);
			Console.WriteLine("\n");

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
                                string city = "";
                                do
                                {
                                    Console.WriteLine("Enter city name: ");
                                    city = Console.ReadLine();

                                } while (string.IsNullOrWhiteSpace(city) || city.Any(char.IsDigit) || ContainsMultipleSpaces(city));

                                // Računa se potrošnja za grad i ispisuje se
                                Double meanForCity = proxy.CalculateConsumptionMeanCity(city);
                                Console.WriteLine("\nThe average value of spending for city " + city + " is : " + meanForCity + "\n");
                                break;

                            case "2":
                                // Unosi se region
                                string region = "";
                                do
                                {
                                    Console.WriteLine("Enter a name for the region :");
                                    region = Console.ReadLine();

                                } while (string.IsNullOrWhiteSpace(region) || region.Any(char.IsDigit) || ContainsMultipleSpaces(region));

                                // Računa se potrošnja za region i ispisuje se
                                Double meanForRegion = proxy.CalculateConsumptionMeanRegion(region);
                                Console.WriteLine("\nThe average value of spending for region " + region + " is : " + meanForRegion + "\n");
                                break;

                            default:
                                Console.WriteLine("\nInvalid option!");
                                break;
                        }
                        break;

                    case "3":
                        int id;
                        string inputID = "";

                        do
                        {
                            Console.WriteLine("Please enter your ID first :");
                            inputID = Console.ReadLine();

                            if (int.TryParse(inputID, out id))
                            {
                                break;
                            }

                            Console.WriteLine("\nInvalid input. Please enter a valid integer for your ID.\n");
                        } while (true);

                        double value;
                        string inputValue = "";
                        do
                        {
                            Console.WriteLine("Enter a consumption value for the current month :");
                            inputValue = Console.ReadLine();

                            if (double.TryParse(inputValue, out value))
                            {
                                break;
                            }

                            Console.WriteLine("\nInvalid input. Please enter a valid double for the consumption value.\n");
                        } while (true);

                        bool feedback = proxy.Modify(inputID, inputValue);
                        if (feedback == true)
                        {
                            Console.WriteLine("The modification has been successfully made !!!");
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