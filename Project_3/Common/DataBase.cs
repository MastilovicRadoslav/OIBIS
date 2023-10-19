using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            List<Measurement> measurements = new List<Measurement>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        var data = line.Split(',').Select(s => s.Trim()).ToList();
                        Measurement m = new Measurement();

                        foreach (var item in data)
                        {
                            var parts = item.Split('=').Select(s => s.Trim()).ToArray();
                            if (parts.Length == 2)
                            {
                                string propertyName = parts[0];
                                string propertyValue = parts[1];

                                switch (propertyName)
                                {
                                    case "Id":
                                        m.Id = int.Parse(propertyValue);
                                        break;
                                    case "Region":
                                        m.Region = propertyValue.Trim('"');
                                        break;
                                    case "City":
                                        m.City = propertyValue.Trim('"');
                                        break;
                                    case "Year":
                                        m.Year = propertyValue.Trim('"');
                                        break;
                                    case "Consumption":
                                        m.Consumption = propertyValue;
                                        break;
                                }
                            }
                        }

                        if (m != null)
                        {
                            measurements.Add(m);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }

            return measurements;
        }
    }
}