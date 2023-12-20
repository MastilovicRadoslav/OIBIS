using System;
using System.Diagnostics;

namespace Manager
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "Manager.Audit";
        const string LogName = "CentralDBLogs";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void Add()
        {
            if (customLog != null)
            {
                string UserAddingSuccess = AuditEvents.Add;
                string message = String.Format(UserAddingSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Add));
            }
        }

        public static void Delete()
        {
            if (customLog != null)
            {
                string UserDeleteSuccess = AuditEvents.Delete;
                string message = String.Format(UserDeleteSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Delete));
            }
        }

        public static void Modify()
        {
            if (customLog != null)
            {
                string UserUpdatingSuccess = AuditEvents.Modify;
                string message = String.Format(UserUpdatingSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Modify));
            }
        }
        public static void AuthorizationFailed()
        {
            if (customLog != null)
            {
                string NotAuthorized = AuditEvents.AuthorizationFailed;
                string message = String.Format(NotAuthorized);
                customLog.WriteEntry(message, EventLogEntryType.Warning);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationFailed));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
