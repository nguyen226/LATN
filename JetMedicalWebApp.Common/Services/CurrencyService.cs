
namespace JetMedicalWebApp.Common.Services
{
    public class CurrencyService
    {
        public static string convertCurrencyVND(double chuoi)
        {
            var info = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");
            return chuoi.ToString("#,###", info.NumberFormat);
        }
    }
}
