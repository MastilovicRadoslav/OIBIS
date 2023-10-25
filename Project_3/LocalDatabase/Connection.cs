using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDatabase
{
    public class Connection : IConnection
    {
        public List<Measurement> specificRegionList = new List<Measurement>();
        public DataBase db = new DataBase();
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
    }
}
