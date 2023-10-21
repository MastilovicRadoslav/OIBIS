using Common;
using System.Collections.Generic;

namespace CentralDatabase
{
    public class DBConnection : IDBConnection
    {
        public DataBase data = new DataBase();
        public List<Measurement> DataTransfer()
        {
            List<Measurement> returnList = new List<Measurement>();
            returnList = data.ReadMeasurementsFromFile("measuredData.txt");
            return returnList;
        }
    }
}