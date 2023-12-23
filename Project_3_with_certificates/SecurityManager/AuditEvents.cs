using System.Reflection;
using System.Resources;

namespace SecurityManager
{
	public enum AuditEventTypes
	{
		Add = 0,
		Delete = 1,
		Modify = 2,
		NoCertificateCDB = 3,
		NoCertificateLDB = 4 ,
		AuthorizationFailed = 5,
	}

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string Add
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.Add.ToString());
			}
		}

		public static string Delete
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.Delete.ToString());
			}
		}

		public static string Modify
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.Modify.ToString());
			}
		}

		public static string NoCertificateCDB
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.NoCertificateCDB.ToString());
			}
		}
		public static string NoCertificateLDB
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.NoCertificateLDB.ToString());
			}
		}
		public static string AuthorizationFailed
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
			}
		}
	}
}
