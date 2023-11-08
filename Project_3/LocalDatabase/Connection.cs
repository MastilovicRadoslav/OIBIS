﻿using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LocalDatabase
{
	public class Connection : IConnection
	{
		MarkChange markChange = Help.HelpForChange;
		public bool changeDB = false;
		public static Dictionary<string, User> UserAccountsDB = new Dictionary<string, User>();
		public List<Measurement> specificRegionList = new List<Measurement>();
		public DataBase db = new DataBase();
		public Measurement help = new Measurement();
		List<Measurement> IConnection.PrintMeasurements()
		{
			specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");
			return specificRegionList;
		}

		public bool Modify(string id, string value)
		{
			specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");
			int localId = int.Parse(id);
			DateTime currentTime = DateTime.Now;
			string currentMonth = currentTime.ToString("MMMM");
			int counter = 0;

			for (int iIndex = 0; iIndex < specificRegionList.Count; iIndex++)
			{
				var i = specificRegionList[iIndex];

				if (i.Id == localId)
				{
					var consumption = i.Consumption;
					for (int cIndex = 0; cIndex < consumption.Count; cIndex++)
					{
						var c = consumption.ElementAt(cIndex);
						if (c.Key.Equals(currentMonth))
						{
							consumption[c.Key] = value;
							counter++;
						}
					}
				}
			}


			// Otvorite datoteku za pisanje i odmah je zatvorite, čime ćete je očistiti.
			File.WriteAllText("measuredDataForRegion.txt", string.Empty);

			foreach (var i in specificRegionList)
			{
				db.WriteMeasurementToFile(i, "measuredDataForRegion.txt");
			}


			if (counter == 0)
			{
				return false;
			}
			return true;
		}

        // Funkcija za ažuriranje centralne baze ako se izmeni lokalna baza
        public void ChangeInDB(bool change)
        {
            changeDB = change;
            specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");

            if (changeDB)
            {
                for (int i = 0; i < markChange.dataToModify.Count; i++)
                {
                    for (int j = 0; j < specificRegionList.Count; j++)
                    {
                        if (markChange.dataToModify[i].Id == specificRegionList[j].Id)
                        {
                            markChange.dataToModify[i].Consumption = specificRegionList[j].Consumption;
                        }
                    }
                }
                markChange.dataChanged = true;
            }
            else
            {
                Console.WriteLine("Waiting for change...");
                Console.ReadKey();
            }
        }
        public double CalculateConsumptionMeanCity(string city)
		{
			double sumCity = 0;
			List<Measurement> specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");

			foreach (var measurement in specificRegionList)
			{
				if (measurement.City.Equals(city))
				{
					foreach (var value in measurement.Consumption.Values)
					{
						// Uklonite nepotrebne vitičaste zagrade na kraju
						string cleanedString = value.Trim(' ', '}');

						// Zamenite tačku sa zarezom ako je potrebno
						cleanedString = cleanedString.Replace('.', ',');

						// Dodajte vrednost na sumu
						sumCity += Double.Parse(cleanedString);
					}
				}
			}

			// Proverite da li ima podataka pre nego što podelite
			if (specificRegionList.Count > 0)
			{
				return sumCity / specificRegionList.Count;
			}
			else
			{
				return 0; // Vratite nulu ako nema podataka za dati grad
			}
		}


		public double CalculateConsumptionMeanRegion(string region)
		{
			double sumRegion = 0;
			List<Measurement> specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");

			foreach (var measurement in specificRegionList)
			{
				if (measurement.Region.Equals(region))
				{
					foreach (var value in measurement.Consumption.Values)
					{
						// Uklonite nepotrebne zatvorene vitičaste zagrade na kraju
						string cleanedString = value.Trim(' ', '}');

						// Zamenite tačku sa zarezom ako je potrebno
						cleanedString = cleanedString.Replace('.', ',');

						// Dodajte vrednost na sumu
						sumRegion += Double.Parse(cleanedString);
					}
				}
			}

			// Proverite da li ima podataka pre nego što podelite
			if (specificRegionList.Count > 0)
			{
				return sumRegion / specificRegionList.Count;
			}
			else
			{
				return 0; // Vratite nulu ako nema podataka za dati region
			}
		}

		public bool DeleteEntity(int id)
		{
			specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");
			int count = 0;

			// Napravite novu listu za čuvanje podataka koji se ne brišu
			List<Measurement> updatedList = new List<Measurement>();

			foreach (var measurement in specificRegionList)
			{
				if (measurement.Id != id)
				{
					// Dodajte podatke koji se ne brišu u novu listu
					updatedList.Add(measurement);
				}
				else
				{
					count++;
				}
			}

			// Nakon iteracije, obrišite postojeću datoteku
			File.Delete("measuredDataForRegion.txt");

			// Zatim pišite ažuriranu listu u istu datoteku
			foreach (var measurement in updatedList)
			{
				db.WriteMeasurementToFile(measurement, "measuredDataForRegion.txt");
			}

			// Vratite `true` ako je barem jedan podatak obrisan
			return count > 0;
		}


		// Funkcija za brisanje entiteta iz centralne baze ako se obriše iz lokalne baze
		public void DeletedEntitiesInDB(bool change)
		{
			if (change)
			{
				Console.WriteLine("!!!!");
				for (int i = 0; i < markChange.listWithDeletedEntities.Count; i++)
				{
					if (markChange.listWithDeletedEntities[i].Id == help.Id)
					{
						markChange.listWithDeletedEntities.RemoveAt(i);
					}
				}
				markChange.entitiesDeleted = true;
			}
			else
			{
				Console.WriteLine("Waiting for change...");
				Console.ReadKey();
			}
		}

		public void AddUser(string username, string password)
		{
			if (!UserAccountsDB.ContainsKey(username))
			{
				UserAccountsDB.Add(username, new User(username, password));
			}
		}

		public bool AddNewEntity(Measurement newEntity)
		{
			specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");
			specificRegionList = specificRegionList.OrderBy(e => e.Id).ToList();
			int indexToInsert = 0;

			while (indexToInsert < specificRegionList.Count && specificRegionList[indexToInsert].Id < newEntity.Id)
			{
				indexToInsert++;
			}

			// TREBA DRUGI USLOVI
			// Proverite da li ID već postoji u listi
			if (indexToInsert < specificRegionList.Count && specificRegionList[indexToInsert].Id == newEntity.Id)
			{
				return false; // ID već postoji, ne može se dodati isti ID.
			}

			specificRegionList.Insert(indexToInsert, newEntity);

			File.WriteAllText("measuredDataForRegion.txt", string.Empty);

			foreach (var entity in specificRegionList)
			{
				db.WriteMeasurementToFile(entity, "measuredDataForRegion.txt");
			}

			return true;
		}

        public void NewEntitiesInDB(bool change, int id)
        {
            specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");

            if (change)
            {
                Console.WriteLine("!!!!");
                Measurement m = new Measurement();

                for (int i = 0; i < specificRegionList.Count; i++)
                {
                    if (specificRegionList[i].Id == id)
                    {
                        m = specificRegionList[i];
                    }
                }
                markChange.listWithNewEntities.Add(m);
                markChange.newEntitiesAdded = true;

            }
            else
            {
                Console.WriteLine("Waiting for change...");
                Console.ReadKey();
            }
        }

    }
}
