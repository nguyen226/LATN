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
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index(int id)
        {
            ViewBag.Id = id;

            BaseViewModel model = new BaseViewModel();

            NewsContentService newsContentService = new NewsContentService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();
            string selectedFields = string.Empty;
            
            if (id > 0)
            {
                selectedFields = "id, fullname, description, isactive, created_at, languageId, TypeName, categoriId";
                inputParam.Add(CommonConstants.StrSortedColumnNames, "CONVERT(Datetime,created_at,103) desc");
                inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);
                inputParam.Add(CommonConstants.StrStringFilter, "languageId = " + languageId.ToString() + " AND categoriId=" + id.ToString());
                parameters = internalService.SetThamSoChoDatatable_GetList(0, 9999, inputParam);
                inData = parameters[CommonConstants.StrInData];
                filters = parameters[CommonConstants.StrFilters];
                requestParameters = parameters[CommonConstants.StrRequestParamters];

                model.NewsContents = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);
            }

            return View(model);
        }


    }
}
