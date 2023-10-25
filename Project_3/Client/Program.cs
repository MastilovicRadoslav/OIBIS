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