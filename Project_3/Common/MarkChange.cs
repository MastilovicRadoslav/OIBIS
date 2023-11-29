using Common.Cryptography;
using System.Collections.Generic;
using System.Security.Cryptography;

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
		public readonly Aes myAes = Aes.Create();
		public readonly AES aes = new AES();
		public readonly SecretMasks sm = new SecretMasks();
	}
}