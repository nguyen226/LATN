using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class ContactController : Controller
    {
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            ViewBag.MennuSelected = "Contact";
            BaseViewModel model = new BaseViewModel();

            ContactService contactService = new ContactService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;

            selectedFields = "id, note, languageId";
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
            inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString());
            parameters = internalService.SetThamSoChoDatatable_GetList(0, 999, inputParam);
            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];
            model.Contact = contactService.GetListExposeDto(filters, inData, out outData).FirstOrDefault();

            return View(model);
        }

    }
}
