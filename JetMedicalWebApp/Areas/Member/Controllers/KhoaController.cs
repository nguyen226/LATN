using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.FilterAttribute;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Member.Controllers
{
    public class KhoaController : Controller
    {
        KhoaService khoaService = new KhoaService();

        [HttpPost]
        public ActionResult Filter(string selectedFields, string stringFilter, string sortCondition, string top)
        {
            InternalService internalService = new InternalService();
            Dictionary<string, string> outData = null;
            Dictionary<string, string> input = new Dictionary<string, string>();

            input.Add(CommonConstants.StrSelectedFields, selectedFields);
            input.Add(CommonConstants.StrStringFilter, stringFilter);
            input.Add(CommonConstants.StrSortedColumnNames, sortCondition);
            if (!string.IsNullOrEmpty(top))
            {
                input.Add(CommonConstants.StrTop, top);
            }
            Dictionary<string, Dictionary<string, string>> parameters = internalService.SetThamSoChoFilter(input);
            return Json(khoaService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }
    }
}