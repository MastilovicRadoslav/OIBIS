using Common;
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

        // Funkcija za ipis lokalne baze
        List<Measurement> IConnection.PrintMeasurements()
        {
            specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");
            return specificRegionList;
        }

        // Funkcija za ažuriranje lokalne baze
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
                Console.WriteLine("\nWaiting for change ...\n");
            }
        }

        // Funkcija za računanje prosečne potrošnje za grad
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
                        string cleanedString = value.Trim(' ', '}');
                        cleanedString = cleanedString.Replace('.', ',');
                        sumCity += Double.Parse(cleanedString);
                    }
                }
            }

            if (specificRegionList.Count > 0)
            {
                return sumCity / specificRegionList.Count;
            }
            else
            {
                return 0;
            }
        }

        // Funkcija za računanje prosečne potrošnje za region
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
                        string cleanedString = value.Trim(' ', '}');
                        cleanedString = cleanedString.Replace('.', ',');
                        sumRegion += Double.Parse(cleanedString);
                    }
                }
            }

            if (specificRegionList.Count > 0)
            {
                return sumRegion / specificRegionList.Count;
            }
            else
            {
                return 0;
            }
        }

        // Funkcija za brisanje entiteta iz lokalne baza
        public bool DeleteEntity(int id)
        {
            specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");
            int count = 0;
            List<Measurement> updatedList = new List<Measurement>();

            foreach (var measurement in specificRegionList)
            {
                if (measurement.Id != id)
                {
                    updatedList.Add(measurement);
                }
                else
                {
                    count++;
                }
            }

            File.WriteAllText("measuredDataForRegion.txt", string.Empty);

            foreach (var measurement in updatedList)
            {
                db.WriteMeasurementToFile(measurement, "measuredDataForRegion.txt");
            }
            return count > 0;
        }

        // Funkcija za brisanje entiteta iz centralne baza
        public void DeletedEntitiesInDB(bool change)
        {
            if (change)
            {
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
                Console.WriteLine("\nWaiting for change ...\n");
            }
        }

        // Funkcija za dodavanje korsinika
        public void AddUser(string username, string password)
        {
            if (!UserAccountsDB.ContainsKey(username))
            {
                UserAccountsDB.Add(username, new User(username, password));
            }
        }

        // Funkcija za dodavanje entiteta u lakalnu bazu
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

        // Funkcija za dodavanje entiteta u centralnu bazu
        public void NewEntitiesInDB(bool change, int id)
        {
            specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");

            if (change)
            {
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
                Console.WriteLine("\nWaiting for change ...\n");
            }
        }
    }
}