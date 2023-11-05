using System.Collections.Generic;
using System.ServiceModel;

namespace Common
{
	[ServiceContract]
	public interface IDBConnection
	{
		[OperationContract]
		List<Measurement> DataTransfer();
	}
}