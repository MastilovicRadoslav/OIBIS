using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IConnection
    {
        [OperationContract]
        (List<byte[]>, byte[], byte[]) PrintMeasurements();

        [OperationContract]
        (byte[], byte[], byte[]) CalculateConsumptionMeanCity(string city);

        [OperationContract]
        (byte[], byte[], byte[]) CalculateConsumptionMeanRegion(string region);

        [OperationContract]
        (byte[], byte[], byte[]) Modify(string id, string value);

        [OperationContract]
        void ChangeInDB(bool change);

        [OperationContract]
        (byte[], byte[], byte[], int) AddNewEntity(string region, string city, string year, Dictionary<string, string> consumption);

        [OperationContract]
        void NewEntitiesInDB(bool change, int id);

        [OperationContract]
        (byte[], byte[], byte[]) DeleteEntity(int idForDelete);

        [OperationContract]
        void DeletedEntitiesInDB(bool change);
    }
}