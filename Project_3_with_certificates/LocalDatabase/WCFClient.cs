using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using Formatter = SecurityManager.Formatter;


namespace ClientApp
{
	public class WCFClient : ChannelFactory<IDBConnection>, IDBConnection, IDisposable
	{
		public DataBase data = new DataBase();

		IDBConnection factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
		{
			string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
			this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
			this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

			factory = this.CreateChannel();
		}

		public void TestCommunication()
		{
			try
			{
				factory.TestCommunication();
			}
			catch (Exception e)
			{
				Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
				try
				{
					Audit.NoCertificateLDB();
					Console.ReadLine();
					Environment.Exit(0);
				}
				catch (Exception auditException)
				{
					Console.WriteLine("[AUDIT ERROR] {0}", auditException.Message);
				}
			}
		}

		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			this.Close();
		}

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
