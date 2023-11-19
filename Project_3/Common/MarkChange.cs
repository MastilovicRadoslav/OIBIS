using System.Collections.Generic;

namespace Common
{
	public class MarkChange
	{
		public List<Measurement> dataToModify;
		public List<Measurement> listWithNewEntities;
		public List<Measurement> listWithDeletedEntities;
		public bool newEntitiesAdded = false;
		public bool entitiesDeleted = false;
		public bool dataChanged = false;
	}
}