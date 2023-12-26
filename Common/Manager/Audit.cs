using Common.Models;
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
        public static void BankingAuditSuccess(string name, string account, DateTime dateTime, double ammount)
        {
            //TO DO

            if (customLog != null)
            {
                string BankingAuditSuccess =
                    AuditEvents.BankingAuditSuccess;
                string message = String.Format(BankingAuditSuccess, name, account, dateTime, ammount);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.BankingAuditSuccess));
            }
        }
        public static void BankingAuditFailed(string name, string account, DateTime dateTime, double ammount)
        {
            //TO DO

            if (customLog != null)
            {
                string BankingAuditFailed =
                    AuditEvents.BankingAuditFailed;
                string message = String.Format(BankingAuditFailed, name, account, dateTime, ammount);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.BankingAuditFailed));
            }
        }
        public static void PullAndCreateSuccess(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string PullAndCreateSuccess =
                    AuditEvents.PullAndCreateSuccess;
                string message = String.Format(PullAndCreateSuccess, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PullAndCreateSuccess));
            }
        }
        public static void PullAndCreateFailed(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string PullAndCreateFailed =
                    AuditEvents.PullAndCreateFailed;
                string message = String.Format(PullAndCreateFailed, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PullAndCreateFailed));
            }
        }

        public static void PaymentSuccess(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string PaymentSuccess =
                    AuditEvents.PaymentSuccess;
                string message = String.Format(PaymentSuccess, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PaymentSuccess));
            }
        }
        public static void PaymentFailed(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string PaymentFailed =
                    AuditEvents.PaymentFailed;
                string message = String.Format(PaymentFailed, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.PaymentFailed));
            }
        }
        public static void TransactionFailed(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string TransactionFailed =
                    AuditEvents.TransactionFailed;
                string message = String.Format(TransactionFailed, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.TransactionFailed));
            }
        }
        public static void ResetPinCodeSuccess(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string ResetPinCodeSuccess =
                    AuditEvents.ResetPinCodeSuccess;
                string message = String.Format(ResetPinCodeSuccess, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ResetPinCodeSuccess));
            }
        }
        public static void ResetPinCodeFailed(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string ResetPinCodeFailed =
                    AuditEvents.ResetPinCodeFailed;
                string message = String.Format(ResetPinCodeFailed, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ResetPinCodeFailed));
            }
        }
        public static void TransactionRequestSuccess(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string TransactionRequestSuccess =
                    AuditEvents.TransactionRequestSuccess;
                string message = String.Format(TransactionRequestSuccess, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.TransactionRequestSuccess));
            }
        }
        public static void TransactionRequestFailed(string name)
        {
            //TO DO

            if (customLog != null)
            {
                string TransactionRequestFailed =
                    AuditEvents.TransactionRequestFailed;
                string message = String.Format(TransactionRequestFailed, name);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.TransactionRequestFailed));
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
