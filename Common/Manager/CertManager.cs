using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security;

namespace Common.Manager
{
	public class CertManager
	{
		public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
		{
			X509Store store = new X509Store(storeName, storeLocation);
			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);
			foreach (X509Certificate2 c in certCollection)
			{
				if (c.SubjectName.Name.Equals(string.Format("CN={0}", subjectName)))
				{
					return c;
				}
			}

			return null;
		}

		public static X509Certificate2 GetCertificateFromFile(string fileName)
		{
			X509Certificate2 certificate = null;


			return certificate;
		}

		public static X509Certificate2 GetCertificateFromFile(string fileName, SecureString pwd)
		{
			X509Certificate2 certificate = null;


			return certificate;
		}
	}
}
