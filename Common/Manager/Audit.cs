using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Manager
{
    public class Audit : IDisposable
    {
        //private static EventLog customLog = null;

        private static EventLog customLog = new EventLog();
        //const string SourceName = "ServerEvents";
        //const string LogName = "AuditTest";

        public static void Initialize()
        {
            try
            {
                if (!EventLog.SourceExists("ServerEvents"))
                {
                    EventLog.CreateEventSource("ServerEvents", "AuditTest");


                    Console.WriteLine("Napravljen je event log");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem u podizanju event loga " + e.Message);
            }

            customLog.Source = "ServerEvents";
            customLog.Log = "AuditTest";
        }

        public static void SertificateCreationSuccess(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string SertificateCreationSuccess =
                    AuditEvents.SertificateCreationSuccess;
                string message = String.Format(SertificateCreationSuccess, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.SertificateCreationSuccess));
            }
        }

        public static void SertificateCreationFailed(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string SertificateCreationFailed =
                    AuditEvents.SertificateCreationFailed;
                string message = String.Format(SertificateCreationFailed, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.SertificateCreationFailed));
            }
        }
        public static void TransactionSuccess(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string TransactionSuccess =
                    AuditEvents.TransactionSuccess;
                string message = String.Format(TransactionSuccess, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.TransactionSuccess));
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
