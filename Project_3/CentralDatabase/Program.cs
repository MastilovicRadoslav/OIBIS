using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CentralDatabase
{
	public class Program
	{
		static void Main(string[] args)
		{
			// Formiranje TEST podataka
			//DataBase db = new DataBase();
			//Measurement m0 = new Measurement
			//{
			//    Id = 1,
			//    Region = "Vojvodina",
			//    City = "Novi Sad",
			//    Year = "2023",
			//    Consumption = "53.33"
			//};

			//Measurement m = new Measurement
			//{
			//    Id = 1,
			//    Region = "Vojvodina",
			//    City = "Novi Sad",
			//    Year = "2023",
			//    Consumption = "53.33"
			//};

			//Measurement m1 = new Measurement
			//{
			//    Id = 2,
			//    Region = "Beograd",
			//    City = "Beograd",
			//    Year = "2023",
			//    Consumption = "88.75"
			//};

			//Measurement m2 = new Measurement
			//{
			//    Id = 3,
			//    Region = "Vojvodina",
			//    City = "Sombor",
			//    Year = "2023",
			//    Consumption = "120.19"
			//};

			//Measurement m3 = new Measurement
			//{
			//    Id = 4,
			//    Region = "Zapadni region",
			//    City = "Čačak",
			//    Year = "2023",
			//    Consumption = "13.59"
			//};

			//Measurement m4 = new Measurement
			//{
			//    Id = 5,
			//    Region = "Južni region",
			//    City = "Niš",
			//    Year = "2023",
			//    Consumption = "54.79"
			//};

			//Measurement m5 = new Measurement
			//{
			//    Id = 6,
			//    Region = "Vojvodina",
			//    City = "Subotica",
			//    Year = "2023",
			//    Consumption = "123.08"
			//};

			//Measurement m6 = new Measurement
			//{
			//    Id = 7,
			//    Region = "Zapadni region",
			//    City = "Novi Pazar",
			//    Year = "2023",
			//    Consumption = "250.23"
			//};

			//Measurement m7 = new Measurement
			//{
			//    Id = 8,
			//    Region = "Vojvodina",
			//    City = "Zrenjanin",
			//    Year = "2023",
			//    Consumption = "70.31"
			//};

			//Measurement m8 = new Measurement
			//{
			//    Id = 9,
			//    Region = "Južni region",
			//    City = "Pirot",
			//    Year = "2023",
			//    Consumption = "300.00"
			//};

			//Measurement m9 = new Measurement
			//{
			//    Id = 10,
			//    Region = "Kosovo i Metohija",
			//    City = "Priština",
			//    Year = "2023",
			//    Consumption = "99.51"
			//};

			//List<Measurement> list = new List<Measurement> { m0, m, m1, m2, m3, m4, m5, m6, m7, m8, m9 };

			//foreach (var x in list)
			//{
			//    db.WriteMeasurementToFile(x, "measuredData.txt");
			//}

			// Redovan deo - konekcija central i lokal DB
			ServiceHost host = new ServiceHost(typeof(DBConnection));
			try
			{
				host.Open();
				Console.WriteLine("Server has started!!!!\n");
				Console.ReadKey();
			}
			finally
			{
				host.Close();
			}
		}
	}
}
