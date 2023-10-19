using System;

namespace Client
{
	public class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine("-------------------------------");
				Console.WriteLine("Choose an option :");
				Console.WriteLine("\t1. Read");
				Console.WriteLine("\t2. Read, Calculate");
				Console.WriteLine("\t3. Read, Modify");
				Console.WriteLine("\t4. Read, Administer");
				Console.WriteLine("\t0. Exit the application!");
				string option = Console.ReadLine();
				Console.WriteLine("--------------------------------");

				switch (option)
				{
					case "1":
						//ReadDataFromDatabase();
						Console.WriteLine("Reading data...");
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
