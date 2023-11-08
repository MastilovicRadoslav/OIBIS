using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IConnection
    {
        [OperationContract]
        List<Measurement> PrintMeasurements();

        [OperationContract]
        Double CalculateConsumptionMeanCity(string city);

        [OperationContract]
        Double CalculateConsumptionMeanRegion(string region);

        [OperationContract]
        bool Modify(string id, string value);

        [OperationContract]
        void ChangeInDB(bool change);

        [OperationContract]
        bool AddNewEntity(Measurement newEntity);

        [OperationContract]
        void NewEntitiesInDB(bool change, int id);

        [OperationContract]
        bool DeleteEntity(int idForDelete);

        [OperationContract]
        void DeletedEntitiesInDB(bool change);

        [OperationContract]
        void AddUser(string username, string password);
    }
}