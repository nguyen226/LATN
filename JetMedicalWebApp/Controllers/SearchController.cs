using JetMedicalWebApp.Common;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/

        public ActionResult Index()
        {
            NewsContentService newsContentService = new NewsContentService();

            InternalService internalService = new InternalService();
            Dictionary<string, string> filters = null, inData = null, outData = null, requestParameters = null;
            Dictionary<string, Dictionary<string, string>> parameters = null;
            Dictionary<string, string> inputParam = new Dictionary<string, string>();

            int languageId = Utilities.GetLanguage();


            string txtSearch = Request.QueryString["txtSearch"];
            ViewBag.Title = "Kết quả tìm kiếm: \"" + txtSearch + "\"";

            if (!string.IsNullOrEmpty(txtSearch))
            {
                txtSearch = txtSearch.Trim();
            }
            string selectedFields = "id, name, slug,shorttitle,isactive,TypeCode, created_at, title, avatar";

            inputParam.Add(CommonConstants.StrSortedColumnNames, "created_at DESC");
            inputParam.Add(CommonConstants.StrSelectedFields, selectedFields);

            inputParam.Add(CommonConstants.StrStringFilter, "isactive = 1 AND name LIKE N'%" + txtSearch + "%' OR shorttitle LIKE N'%" + txtSearch + "%'");

            parameters = internalService.SetThamSoChoDatatable_GetList(0, 20, inputParam);

            inData = parameters[CommonConstants.StrInData];
            filters = parameters[CommonConstants.StrFilters];
            requestParameters = parameters[CommonConstants.StrRequestParamters];

            ViewData["searchResult"] = newsContentService.GetListDataFromViewExposeDto(filters, inData, out outData);

            return View();
        }

    }
}
