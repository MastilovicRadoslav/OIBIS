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
    }
}
