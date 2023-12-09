using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CentralDatabase
{
	public class DBConnection : IDBConnection
	{
		public DataBase data = new DataBase();
		public MarkChange markChange = Help.HelpForChange;


		// Funkcija za prenos podataka između CentralDB i LocalDB
		public List<Measurement> DataTransfer()
		{
			List<Measurement> returnList = new List<Measurement>();
			returnList = data.ReadMeasurementsFromFile("measuredData.txt");
			returnList = returnList.OrderBy(entity => entity.Id).ToList();
			return returnList;
		}

		// Ažuriranje CentralDB, ako je Client pozvao funkciju Modify
		public void UpdatingCentralDataBase(List<Measurement> updatedList)
		{
			File.WriteAllText("measuredData.txt", string.Empty);
			updatedList = updatedList.OrderBy(entity => entity.Id).ToList();

			foreach (var x in updatedList)
			{
				data.WriteMeasurementToFile(x, "measuredData.txt");
			}

			try
			{
				Audit.Modify();

			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

		}

		// Ažuriranje CentralDB, ako je dodat novi entitet u LocalDB
		public void AddNewEntityToCentralDB(List<Measurement> listOfNewEntities)
		{
			File.WriteAllText("measuredData.txt", string.Empty);
			listOfNewEntities = listOfNewEntities.OrderBy(entity => entity.Id).ToList();

			foreach (var x in listOfNewEntities)
			{
				data.WriteMeasurementToFile(x, "measuredData.txt");
			}

			try
			{
				Audit.Add();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		// Ažuriranje CentralDB, ako je obrisan entitet iz LocalDB
		public void DeleteEntityFromCentralDB(List<Measurement> listWithDeletedEntities)
		{
			File.WriteAllText("measuredData.txt", string.Empty);
			listWithDeletedEntities = listWithDeletedEntities.OrderBy(entity => entity.Id).ToList();

			foreach (var x in listWithDeletedEntities)
			{
				data.WriteMeasurementToFile(x, "measuredData.txt");
			}

			try
			{
				Audit.Delete();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}