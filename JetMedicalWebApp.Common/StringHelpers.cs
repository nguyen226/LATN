using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Web;
using System.Text.RegularExpressions;

namespace JetMedicalWebApp.Common
{
    public class StringHelpers
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }

            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace(" ", "-");

            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string str = input.Normalize(System.Text.NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") > 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }

            while (str2.IndexOf("--") > 0)
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2;
        }

    }
}
