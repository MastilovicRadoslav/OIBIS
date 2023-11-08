using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IDBConnection
    {
        [OperationContract]
        List<Measurement> DataTransfer();

        [OperationContract]
        void UpdatingCentralDataBase(List<Measurement> updatedList);

        [OperationContract]
        void AddNewEntityToCentralDB(List<Measurement> listOfNewEntities);

        [OperationContract]
        void DeleteEntityFromCentralDB(List<Measurement> listWithDeletedEntities);
    }
}