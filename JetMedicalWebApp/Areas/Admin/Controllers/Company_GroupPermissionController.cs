using JetMedicalWebApp.Common;
using JetMedicalWebApp.Entities.EntityDto;
using JetMedicalWebApp.Areas.Admin.Models;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Admin.Controllers
{
    public class Company_GroupPermissionController : Controller
    {
        Company_GroupPermissionService company_GroupPermissionService = new Company_GroupPermissionService();

        [HttpPost]
        public ActionResult GetListByGroupPermissionId(string groupPermissionId)
        {
            string selectedFields = "Id, GroupPermission.Id AS GroupPermissionId, Company.CompanyID AS CompanyId, Company.ComName AS ComName";
            return Json(company_GroupPermissionService.GetListByGroupPermissionIdExposeDto(groupPermissionId, selectedFields), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddOrUpdate(int groupPermissionId, int companyId)
        {
            string resultMessage = string.Empty;
            resultMessage = company_GroupPermissionService.AddOrUpdate(groupPermissionId, companyId);
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteById(int id)
        {
            string resultMessage = string.Empty;
            resultMessage = company_GroupPermissionService.DeleteById(id);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

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

            return Json(company_GroupPermissionService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }
    }
}