using System;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class DataBase
    {
        // Funkcija za upis u txt fajl
        public void WriteMeasurementToFile(Measurement m, string filePath)
        {
            try
            {
                if (!MeasurementExistsInFile(m, filePath))
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.Write(m.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

        // Pomoćna funkcija da bi bila zadovoljena jedinstvenost IDa
        public bool MeasurementExistsInFile(Measurement m, string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains($"Id = {m.Id},"))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Funkcija za čitanje txt fajla
        public List<Measurement> ReadMeasurementsFromFile(string filePath)
        {
            List<Measurement> data = new List<Measurement>();
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');

                int id = int.Parse(parts[0].Split('=')[1].Trim());
                string region = parts[1].Split('=')[1].Trim();
                string city = parts[2].Split('=')[1].Trim();
                string year = parts[3].Split('=')[1].Trim();

                string consumptionStr = parts[4].Replace("Consumption = {", "");
                var consumptionParts = consumptionStr.Split(';');

                Dictionary<string, string> consumption = new Dictionary<string, string>();

                foreach (var item in consumptionParts)
                {
                    string[] itemParts = item.Trim().Split('=');
                    string month = itemParts[0].Trim();
                    string value = itemParts[1].Trim();
                    if (value.Contains("}"))
                    {
                        string cleanedString = value.Replace("}", "");
                        consumption[month] = cleanedString;
                        break;
                    }
                    consumption[month] = value;
                }

                Measurement consumptionData = new Measurement
                {
                    Id = id,
                    Region = region,
                    City = city,
                    Year = year,
                    Consumption = consumption
                };
                data.Add(consumptionData);
            }
            return data;
        }
    }
}