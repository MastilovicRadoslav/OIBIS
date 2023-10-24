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
                        //ReadDataFromDatabase();
                        //CalculateConsumption();
                        Console.WriteLine("Reading data...");
                        Console.WriteLine("Result:...");
                        break;

                    case "3":
                        break;

                    case "4":
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
