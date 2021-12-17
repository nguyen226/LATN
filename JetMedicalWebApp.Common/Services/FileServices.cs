using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace JetMedicalWebApp.Common.Services
{
    public class FileServices
    {
        public static byte[] Download(string filename)
        {
            return System.IO.File.ReadAllBytes(filename);
        }

        public static string Unzip(string sourceFile, string destinationFolderName)
        {
            string resultMessage = string.Empty;
            try
            {
                ZipFile.ExtractToDirectory(sourceFile, destinationFolderName);
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }
        public static List<ZipArchiveEntry> GetContent(string sourceFile)
        {            
            return ZipFile.OpenRead(sourceFile).Entries.ToList();
        }

    }
}
