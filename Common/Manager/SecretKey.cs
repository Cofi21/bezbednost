using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Manager
{
	public class SecretKey
	{
		#region Generate Secret Key
		public static string GenerateKey()
		{
			SymmetricAlgorithm symmAlgorithm = TripleDESCryptoServiceProvider.Create();

			return symmAlgorithm == null ? String.Empty : ASCIIEncoding.ASCII.GetString(symmAlgorithm.Key);
		}

		#endregion

		#region Store Secret Key

		public static void StoreKey(string secretKey, string outFile)
		{
			FileStream fOutput = new FileStream(outFile, FileMode.OpenOrCreate, FileAccess.Write);
			byte[] buffer = Encoding.ASCII.GetBytes(secretKey);

			try
			{
				fOutput.Write(buffer, 0, buffer.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine("SecretKeys.StoreKey:: ERROR {0}", e.Message);
			}
			finally
			{
				fOutput.Close();
			}
		}

		#endregion

		#region Load Secret Key

		public static string LoadKey(string inFile)
		{
			FileStream fInput = new FileStream(inFile, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[(int)fInput.Length];

			try
			{
				fInput.Read(buffer, 0, (int)fInput.Length);
			}
			catch (Exception e)
			{
				Console.WriteLine("SecretKeys.LoadKey:: ERROR {0}", e.Message);
			}
			finally
			{
				fInput.Close();
			}

			return ASCIIEncoding.ASCII.GetString(buffer);
		}

		#endregion
	}
}
