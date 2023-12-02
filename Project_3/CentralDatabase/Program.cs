using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace CentralDatabase
{
	public class Program
	{
		static void Main(string[] args)
		{
			DataBase db = new DataBase();

			#region Formiranje TEST podataka
			Measurement m1 = new Measurement
			{
				Id = 1,
				Region = "Vojvodina",
				City = "Novi Sad",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "53.33" },
					{ "February", "100.88" },
					{ "March", "73.79" },
					{ "April", "93.13" },
					{ "May", "53.55" },
					{ "June", "66.66" },
					{ "July", "12.22" },
					{ "August", "123.09" },
					{ "September", "155.19" },
					{ "October", "233.85" },
					{ "November", "39.09" },
					{ "December", "0" }
				}
			};

			Measurement m2 = new Measurement
			{
				Id = 2,
				Region = "Beograd",
				City = "Beograd",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "75.11" },
					{ "February", "88.25" },
					{ "March", "95.88" },
					{ "April", "110.32" },
					{ "May", "63.79" },
					{ "June", "71.14" },
					{ "July", "20.93" },
					{ "August", "133.46" },
					{ "September", "160.75" },
					{ "October", "185.21" },
					{ "November", "243.15" },
					{ "December", "0" }
				}
			};

			Measurement m3 = new Measurement
			{
				Id = 3,
				Region = "Vojvodina",
				City = "Sombor",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "45.45" },
					{ "February", "110.11" },
					{ "March", "80.88" },
					{ "April", "95.22" },
					{ "May", "70.79" },
					{ "June", "75.14" },
					{ "July", "15.93" },
					{ "August", "122.46" },
					{ "September", "162.75" },
					{ "October", "215.21" },
					{ "November", "53.45" },
					{ "December", "0" }
				}
			};

			Measurement m4 = new Measurement
			{
				Id = 4,
				Region = "Zapadni region",
				City = "Čačak",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "59.22" },
					{ "February", "95.67" },
					{ "March", "70.88" },
					{ "April", "85.32" },
					{ "May", "50.79" },
					{ "June", "73.14" },
					{ "July", "18.93" },
					{ "August", "120.46" },
					{ "September", "160.75" },
					{ "October", "228.21" },
					{ "November", "83.65" },
					{ "December", "0" }
				}
			};

			Measurement m5 = new Measurement
			{
				Id = 5,
				Region = "Južni region",
				City = "Niš",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "58.55" },
					{ "February", "105.88" },
					{ "March", "74.79" },
					{ "April", "93.13" },
					{ "May", "55.55" },
					{ "June", "68.66" },
					{ "July", "15.22" },
					{ "August", "123.09" },
					{ "September", "157.19" },
					{ "October", "230.85" },
					{ "November", "163.91" },
					{ "December", "0" }
				}
			};

			Measurement m6 = new Measurement
			{
				Id = 6,
				Region = "Vojvodina",
				City = "Subotica",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "63.33" },
					{ "February", "110.88" },
					{ "March", "70.79" },
					{ "April", "90.13" },
					{ "May", "52.55" },
					{ "June", "65.66" },
					{ "July", "14.22" },
					{ "August", "125.09" },
					{ "September", "159.19" },
					{ "October", "235.85" },
					{ "November", "101.65" },
					{ "December", "0" }
				}
			};

			Measurement m7 = new Measurement
			{
				Id = 7,
				Region = "Zapadni region",
				City = "Novi Pazar",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "48.33" },
					{ "February", "98.88" },
					{ "March", "72.79" },
					{ "April", "92.13" },
					{ "May", "51.55" },
					{ "June", "67.66" },
					{ "July", "13.22" },
					{ "August", "126.09" },
					{ "September", "156.19" },
					{ "October", "233.85" },
					{ "November", "267.45" },
					{ "December", "0" }
				}
			};

			Measurement m8 = new Measurement
			{
				Id = 8,
				Region = "Vojvodina",
				City = "Zrenjanin",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "50.33" },
					{ "February", "99.88" },
					{ "March", "74.79" },
					{ "April", "94.13" },
					{ "May", "52.55" },
					{ "June", "67.66" },
					{ "July", "15.22" },
					{ "August", "128.09" },
					{ "September", "158.19" },
					{ "October", "230.85" },
					{ "November", "131.85" },
					{ "December", "0" }
				}
			};

			Measurement m9 = new Measurement
			{
				Id = 9,
				Region = "Južni region",
				City = "Pirot",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "56.33" },
					{ "February", "102.88" },
					{ "March", "75.79" },
					{ "April", "91.13" },
					{ "May", "54.55" },
					{ "June", "65.66" },
					{ "July", "13.22" },
					{ "August", "122.09" },
					{ "September", "154.19" },
					{ "October", "228.85" },
					{ "November", "423.35" },
					{ "December", "0" }
				}
			};

			Measurement m10 = new Measurement
			{
				Id = 10,
				Region = "Kosovo i Metohija",
				City = "Priština",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "55.33" },
					{ "February", "101.88" },
					{ "March", "76.79" },
					{ "April", "92.13" },
					{ "May", "54.55" },
					{ "June", "68.66" },
					{ "July", "14.22" },
					{ "August", "124.09" },
					{ "September", "156.19" },
					{ "October", "234.85" },
					{ "November", "333.33" },
					{ "December", "0" }
				}
			};

			Measurement m11 = new Measurement
			{
				Id = 11,
				Region = "Centralni region",
				City = "Kragujevac",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "58.33" },
					{ "February", "108.88" },
					{ "March", "79.79" },
					{ "April", "91.13" },
					{ "May", "55.55" },
					{ "June", "69.66" },
					{ "July", "148.22" },
					{ "August", "193.09" },
					{ "September", "256.19" },
					{ "October", "400.85" },
					{ "November", "236.95" },
					{ "December", "0" }
				}
			};

			Measurement m12 = new Measurement
			{
				Id = 12,
				Region = "Istočni region",
				City = "Niš",
				Year = "2023",
				Consumption = new Dictionary<string, string>
				{
					{ "January", "59.33" },
					{ "February", "21.88" },
					{ "March", "71.29" },
					{ "April", "84.13" },
					{ "May", "41.55" },
					{ "June", "98.66" },
					{ "July", "142.82" },
					{ "August", "159.09" },
					{ "September", "316.19" },
					{ "October", "281.15" },
					{ "November", "113.13" },
					{ "December", "0" }
				}
			};

			List<Measurement> list = new List<Measurement> { m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12 };
			#endregion

			// Upis podataka u txt fajl, koji simulira bazu podataka
			foreach (var x in list)
			{
				db.WriteMeasurementToFile(x, "measuredData.txt");
			}

			// Konekcija CentralDB i LocalDB
			ServiceHost host = new ServiceHost(typeof(DBConnection));
			try
			{
				host.Open();
				Console.WriteLine("Server has started !!!\n");
				Console.ReadKey();
			}
			finally
			{
				host.Close();
			}
		}
	}
}