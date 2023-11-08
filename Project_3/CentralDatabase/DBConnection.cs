using Common;
using System.Collections.Generic;
using System.IO;

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

		public void UpdatingCentralDataBase(List<Measurement> updatedList)
		{
			File.WriteAllText("measuredData.txt", string.Empty);

			foreach (var x in updatedList)
			{
				data.WriteMeasurementToFile(x, "measuredData.txt");
			}
		}

		public void AddNewEntityToCentralDB(List<Measurement> listOfNewEntities)
		{
			File.WriteAllText("measuredData.txt", string.Empty);

			foreach (var x in listOfNewEntities)
			{
				data.WriteMeasurementToFile(x, "measuredData.txt");
			}
		}

		public void DeleteEntityFromCentralDB(List<Measurement> listWithDeletedEntities)
		{
			File.WriteAllText("measuredData.txt", string.Empty);

			foreach (var x in listWithDeletedEntities)
			{
				data.WriteMeasurementToFile(x, "measuredData.txt");
			}
		}
	}
}