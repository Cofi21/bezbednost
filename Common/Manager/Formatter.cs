﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Manager
{
    public class Formatter
    {
        public static string ParseName(string logName)
        {
            string[] parts = new string[] { };

            if (logName.Contains("@"))
            {
                ///UPN format
                parts = logName.Split('@');
                return parts[0];
            }
            else if (logName.Contains("\\"))
            {
                /// SPN format
                parts = logName.Split('\\');
                return parts[1];
            }
            else if (logName.Contains("CN"))
            {
                // sertifikati, name je formiran kao CN=imeKorisnika;
                int startIndex = logName.IndexOf("=") + 1;
                int endIndex = logName.IndexOf(";");
                string s = logName.Substring(startIndex, endIndex - startIndex);
                return s;
            }
            else
            {
                return logName;
            }
        }
    }
}
