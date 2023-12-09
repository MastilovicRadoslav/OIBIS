using SecurityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
	public enum AuditEventTypes
	{
		Add = 0,
		Delete = 1,
		Modify = 2,
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
	}
}
