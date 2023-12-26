using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Common.Manager
{
    public enum AuditEventTypes
    {
        SertificateCreationSuccess = 0,
        SertificateCreationFailed = 1,
        TransactionSuccess = 2,
        BankingAuditSuccess = 3,
        BankingAuditFailed = 4,
        PullAndCreateSuccess = 5,
        PullAndCreateFailed = 6,
        PaymentSuccess = 7,
        PaymentFailed = 8,
        TransactionFailed = 9,
        ResetPinCodeSuccess = 10,
        ResetPinCodeFailed = 11,
        TransactionRequestSuccess = 12,
        TransactionRequestFailed = 13
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

        public static string SertificateCreationSuccess
        {
            get
            {
                // TO DO
                return ResourceMgr.GetString(AuditEventTypes.SertificateCreationSuccess.ToString());
            }
        }

        public static string SertificateCreationFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.SertificateCreationFailed.ToString());
            }
        }
        public static string TransactionSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.TransactionSuccess.ToString());
            }
        }
        public static string TransactionFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.TransactionFailed.ToString());
            }
        }
        public static string BankingAuditSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.BankingAuditSuccess.ToString());
            }
        }
        public static string BankingAuditFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.BankingAuditFailed.ToString());
            }
        }
        public static string PullAndCreateSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.PullAndCreateSuccess.ToString());
            }
        }
        public static string PullAndCreateFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.PullAndCreateFailed.ToString());
            }
        }
        public static string PaymentSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.PaymentSuccess.ToString());
            }
        }
        public static string PaymentFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.PaymentFailed.ToString());
            }
        }
        public static string ResetPinCodeSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.ResetPinCodeSuccess.ToString());
            }
        }
        public static string ResetPinCodeFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.ResetPinCodeFailed.ToString());
            }
        }
        public static string TransactionRequestSuccess
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.TransactionRequestSuccess.ToString());
            }
        }
        public static string TransactionRequestFailed
        {
            get
            {
                //TO DO
                return ResourceMgr.GetString(AuditEventTypes.TransactionRequestFailed.ToString());
            }
        }

    }
}
