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
        BankingAuditFailed = 4
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

    }
}
