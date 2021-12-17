using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace JetMedicalWebApp.Common
{
    public class Utilities
    {
        public string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }
        public string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }

        public string Mid(string param, int startIndex, int length)
        {
            //start at the specified index in the string ang get N number of
            //characters depending on the lenght and assign it to a variable
            string result = param.Substring(startIndex, length);
            //return the result of the operation
            return result;
        }

        public string Mid(string param, int startIndex)
        {
            //start at the specified index and return all characters after it
            //and assign it to a variable
            string result = param.Substring(startIndex);
            //return the result of the operation
            return result;
        }
        public static string ConvertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public static string BoDauVaLayKyTuDau(string s)
        {
            string result = string.Empty;
            string temp = ConvertToUnSign(s);
            if (temp.IndexOf(' ') > -1)
            {
                string[] temps = temp.Split();
                foreach (string item in temps)
                {
                    result += item[0];
                }
            }
            else
            {
                result = temp;
            }

            return result;
        }
        public static string ChuyenSo(string number)
        {
            return replace_special_word(join_unit(number)).ToUpper().Trim();
        }
        private static string join_unit(string n)
        {
            int sokytu = n.Length;
            int sodonvi = (sokytu % 3 > 0) ? (sokytu / 3 + 1) : (sokytu / 3);
            n = n.PadLeft(sodonvi * 3, '0');
            sokytu = n.Length;
            string chuoi = "";
            int i = 1;
            while (i <= sodonvi)
            {
                if (i == sodonvi) chuoi = join_number((int.Parse(n.Substring(sokytu - (i * 3), 3))).ToString()) + unit(i) + chuoi;
                else chuoi = join_number(n.Substring(sokytu - (i * 3), 3)) + unit(i) + chuoi;
                i += 1;
            }
            return chuoi;
        }

        private static string unit(int n)
        {
            string chuoi = "";
            if (n == 1) chuoi = " đồng ";
            else if (n == 2) chuoi = " nghìn ";
            else if (n == 3) chuoi = " triệu ";
            else if (n == 4) chuoi = " tỷ ";
            else if (n == 5) chuoi = " nghìn tỷ ";
            else if (n == 6) chuoi = " triệu tỷ ";
            else if (n == 7) chuoi = " tỷ tỷ ";
            return chuoi;
        }


        private static string convert_number(string n)
        {
            string chuoi = "";
            if (n == "0") chuoi = "không";
            else if (n == "1") chuoi = "một";
            else if (n == "2") chuoi = "hai";
            else if (n == "3") chuoi = "ba";
            else if (n == "4") chuoi = "bốn";
            else if (n == "5") chuoi = "năm";
            else if (n == "6") chuoi = "sáu";
            else if (n == "7") chuoi = "bảy";
            else if (n == "8") chuoi = "tám";
            else if (n == "9") chuoi = "chín";
            return chuoi;
        }


        private static string join_number(string n)
        {
            string chuoi = "";
            int i = 1, j = n.Length;
            while (i <= j)
            {
                if (i == 1) chuoi = convert_number(n.Substring(j - i, 1)) + chuoi;
                else if (i == 2) chuoi = convert_number(n.Substring(j - i, 1)) + " mươi " + chuoi;
                else if (i == 3) chuoi = convert_number(n.Substring(j - i, 1)) + " trăm " + chuoi;
                i += 1;
            }
            return chuoi;
        }


        private static string replace_special_word(string chuoi)
        {
            chuoi = chuoi.Replace("không mươi không ", "");
            chuoi = chuoi.Replace("không trăm ", "");
            chuoi = chuoi.Replace("không mươi", "lẻ");
            chuoi = chuoi.Replace("i không", "i");
            chuoi = chuoi.Replace("i năm", "i lăm");
            chuoi = chuoi.Replace("một mươi", "mười");
            chuoi = chuoi.Replace("mươi một", "mươi mốt");
            return chuoi;
        }

        public static bool checkFormatEmail(string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            if (!string.IsNullOrEmpty(email.Trim()))
            {
                return regex.IsMatch(email);
            }
            else
            {
                return false;
            }
        }

        public static string CheckExistFile(string filePath, string filename, bool isAvatar, bool isImage)
        {

            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(filePath))
            {
                return isAvatar ? Common.CommonConstants.FileNoImagePerson : (isImage ? Common.CommonConstants.FileNoImage : string.Empty);
            }

            if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(System.IO.Path.Combine(filePath, filename))))
            {
                return isAvatar ? Common.CommonConstants.FileNoImagePerson : (isImage ? Common.CommonConstants.FileNoImage : string.Empty);
            }

            return System.IO.Path.Combine(filePath, filename);
        }

        public static string GetMatchString(string fist, string second, bool isReverse)
        {
            if (isReverse)
            {
                fist = ReverseString(fist);
                second = ReverseString(second);
            }
            StringBuilder builder = new StringBuilder();
            char[] ar1 = fist.ToArray();
            for (int i = 0; i < ar1.Length; i++)
            {
                if (fist.Length > i + 1 && ar1[i].Equals(second[i]))
                {
                    builder.Append(ar1[i]);
                }
                else
                {
                    break;
                }
            }
            if (isReverse)
            {
                return ReverseString(builder.ToString());
            }
            return builder.ToString();
        }

        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static string MD5(string pasword, string hash)
        {
            string output = string.Empty;

            byte[] byte_salt = Encoding.UTF8.GetBytes(hash);
            byte[] byte_password = Encoding.UTF8.GetBytes(pasword);

            byte[] byte_concat = new byte[byte_salt.Length + byte_password.Length];

            System.Buffer.BlockCopy(byte_salt, 0, byte_concat, 0, byte_salt.Length);
            System.Buffer.BlockCopy(byte_password, 0, byte_concat, byte_salt.Length, byte_password.Length);

            MD5CryptoServiceProvider hashMd5 = new MD5CryptoServiceProvider();
            output = BitConverter.ToString(hashMd5.ComputeHash(byte_concat));

            return output;
        }

        public static string GenerateHash(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[length];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        private static readonly string[] VnReplace = new string[] { "aAeEoOuUiIdDyY", "áàạảãâấầậẩẫăắằặẳẵ", "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ", "éèẹẻẽêếềệểễ", "ÉÈẸẺẼÊẾỀỆỂỄ", "óòọỏõôốồộổỗơớờợởỡ", "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ", "úùụủũưứừựửữ", "ÚÙỤỦŨƯỨỪỰỬỮ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };

        public static string VnConcert(string str)
        {
            for (int i = 1; i < VnReplace.Length; i++)
            {
                for (int j = 0; j < VnReplace[i].Length; j++)
                    str = str.Replace(VnReplace[i][j], VnReplace[0][i - 1]);
            }
            return str;
        }

        public static string urlConvert(string str)
        {
            string url = str.Replace(':', '-');
            url = url.Replace(",", "");
            url = url.Replace(',', '-');
            url = url.Replace('\'', '-');
            url = url.Replace('\"', '-');
            url = url.Replace('/', '-');
            url = url.Replace('\\', '-');
            url = url.Replace('(', '-');
            url = url.Replace(')', '-');
            url = url.Replace('.', '-');
            url = url.Replace('&', '-');
            url = url.Replace('?', '-');
            url = url.Replace(' ', '-');
            return url;
        }

        public static int GetLanguage()
        {
            int languageId = 0;
            string DomainName = HttpContext.Current.Request.Url.Host;
            if(DomainName.Contains("199hospital"))
            {
                 languageId = 2;
            }
            else
            {
                languageId = 1;
            }

            //if (System.Web.HttpContext.Current.Session["languageId"] != null)
            //{
            //    int.TryParse(System.Web.HttpContext.Current.Session["languageId"].ToString(), out languageId);
            //}

            //if (languageId < 1)
            //    languageId = CommonConstants.TiengViet;
            return languageId;
        }

        public static string GenerateRandomCode(int length, int maxRange)
        {
            Random random = new Random();
            int value = random.Next(maxRange);

            string mark = "";

            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    mark += "0";
                }
            }
            else
            {
                mark = "0000";
            }

            return value.ToString(mark);
        }
    }
}
