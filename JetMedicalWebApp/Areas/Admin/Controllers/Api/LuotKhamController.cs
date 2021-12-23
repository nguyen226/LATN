using System;
using System.Web.Http;
using System.Globalization;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Services;
using JetMedicalWebApp.Common;

namespace JetMedicalWebApp.Areas.Admin.Controllers.Api
{
    public class LuotKhamController : ApiController
    {
        RegisterServiceService reg = new RegisterServiceService();
        public string Get(string ngay)
        {

            RegisterService data =null;
            DateTime ngayValue;
            if ((DateTime.TryParseExact(ngay, CommonConstants.DateFormat, null, DateTimeStyles.None, out ngayValue)))
            {
                data = reg.GetRegisterServiceApi(ngayValue);
            }
            return data != null ? ("Khung giờ khám: " +  data.RegisterDate.ToString("dd/MM/yyyy HH:mm") +  " - " + "Lượt khám :" + data.RegisterNo): null;
        }
    }
}
