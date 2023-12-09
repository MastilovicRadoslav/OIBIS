using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;

namespace LocalDatabase
{
    public class Connection : IConnection
    {
        MarkChange markChange = Help.HelpForChange;
        public bool changeDB = false;
        public List<Measurement> specificRegionList = new List<Measurement>();
        public DataBase db = new DataBase();
        public Measurement help = new Measurement();

        [PrincipalPermission(SecurityAction.Demand, Role = "Read")]
        public (List<byte[]>, byte[], byte[]) PrintMeasurements()
        {
            specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");

            List<byte[]> encryptedMeasurements = new List<byte[]>();

            foreach (var item in specificRegionList)
            {
                byte[] encryptedMeasurement = markChange.aes.EncryptStringToBytes_Aes(item.ToString(), markChange.myAes.Key, markChange.myAes.IV);
                encryptedMeasurements.Add(encryptedMeasurement);
            }

            (byte[], byte[]) results = markChange.sm.KeyAndIVEncryption(markChange.myAes.Key, markChange.myAes.IV);
            byte[] yourKey = results.Item1;
            byte[] yourIV = results.Item2;

            return (encryptedMeasurements, yourKey, yourIV);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public (byte[], byte[], byte[]) CalculateConsumptionMeanCity(string city)
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

            (byte[], byte[]) results = markChange.sm.KeyAndIVEncryption(markChange.myAes.Key, markChange.myAes.IV);
            byte[] yourKey = results.Item1;
            byte[] yourIV = results.Item2;
            if (specificRegionList.Count > 0)
            {

                return (markChange.aes.EncryptStringToBytes_Aes((sumCity / specificRegionList.Count).ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV);
            }
            else
            {
                return (markChange.aes.EncryptStringToBytes_Aes((0).ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Calculate")]
        public (byte[], byte[], byte[]) CalculateConsumptionMeanRegion(string region)
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

            (byte[], byte[]) results = markChange.sm.KeyAndIVEncryption(markChange.myAes.Key, markChange.myAes.IV);
            byte[] yourKey = results.Item1;
            byte[] yourIV = results.Item2;
            if (specificRegionList.Count > 0)
            {
                return (markChange.aes.EncryptStringToBytes_Aes((sumRegion / specificRegionList.Count).ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV);
            }
            else
            {
                return (markChange.aes.EncryptStringToBytes_Aes((0).ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Modify")]
        public (byte[], byte[], byte[]) Modify(string id, string value)
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

            (byte[], byte[]) results = markChange.sm.KeyAndIVEncryption(markChange.myAes.Key, markChange.myAes.IV);
            byte[] yourKey = results.Item1;
            byte[] yourIV = results.Item2;
            if (counter == 0)
            {
                return (markChange.aes.EncryptStringToBytes_Aes(false.ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV);
            }
            return (markChange.aes.EncryptStringToBytes_Aes(true.ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV); ;
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
                Console.WriteLine("\nWaiting for change...\n");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public (byte[], byte[], byte[], int) AddNewEntity(string region, string city, string year, Dictionary<string, string> consumption)
        {
            (byte[], byte[]) results = markChange.sm.KeyAndIVEncryption(markChange.myAes.Key, markChange.myAes.IV);
            byte[] yourKey = results.Item1;
            byte[] yourIV = results.Item2;

            int newId = GenerateId();

            Measurement newEntity = new Measurement
            {
                Id = newId,
                Region = region,
                City = city,
                Year = year,
                Consumption = new Dictionary<string, string>(consumption)
            };

            specificRegionList = db.ReadMeasurementsFromFile("measuredDataForRegion.txt");

			int counter = 0;

			for (int i = 0; i < specificRegionList.Count; i++)
			{
				if (specificRegionList[i].Region == newEntity.Region)
				{
					counter++;
				}
			}

			if (counter > 0)
			{
				specificRegionList.Add(newEntity);
			}
			else
			{
				return (markChange.aes.EncryptStringToBytes_Aes(false.ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV, newId);
			}

			File.WriteAllText("measuredDataForRegion.txt", string.Empty);

            foreach (var entity in specificRegionList)
            {
                db.WriteMeasurementToFile(entity, "measuredDataForRegion.txt");
            }

            return (markChange.aes.EncryptStringToBytes_Aes(true.ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV, newId);
        }

        // Funkcija za dodavnje novog entiteta u centralnu bazu ako se doda u lokalnu bazu
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
                Console.WriteLine("\nWaiting for change...\n");
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public (byte[], byte[], byte[]) DeleteEntity(int id)
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
                    help = measurement;
                    count++;
                }
            }

            File.WriteAllText("measuredDataForRegion.txt", string.Empty);

            foreach (var measurement in updatedList)
            {
                db.WriteMeasurementToFile(measurement, "measuredDataForRegion.txt");
            }

            (byte[], byte[]) results = markChange.sm.KeyAndIVEncryption(markChange.myAes.Key, markChange.myAes.IV);
            byte[] yourKey = results.Item1;
            byte[] yourIV = results.Item2;
            return (markChange.aes.EncryptStringToBytes_Aes((count > 0).ToString(), markChange.myAes.Key, markChange.myAes.IV), yourKey, yourIV); ;
        }

        // Funkcija za brisanje entiteta iz centralne baze ako se obriše iz lokalne baze
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
                Console.WriteLine("\nWaiting for change...\n");
            }
        }

        // Funkcija koja generiše ID za dodavanje novog entiteta
        public int GenerateId()
        {
            markChange.counterForID++;
            int nextId = markChange.counterForID;

            return nextId;
        }
    }
}