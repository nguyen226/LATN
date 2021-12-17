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
    public class Menu_GroupPermissionController : Controller
    {
        Menu_GroupPermissionService menu_GroupPermissionService = new Menu_GroupPermissionService();

        [HttpPost]
        public ActionResult GetListByGroupPermissionId(string groupPermissionId)
        {
            string selectedFields = "Id, GroupPermission.Id AS GroupPermissionId, Menu.Id AS MenuId, SystemView,";
            selectedFields += "PersonalView, SystemEdit, PersonalEdit, PersonalDelete, SystemDelete";
            return Json(menu_GroupPermissionService.GetListByGroupPermissionIdExposeDto(groupPermissionId, selectedFields), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddOrUpdate(int groupPermissionId, int menuId, string loai, string value)
        {
            string resultMessage = string.Empty;
            bool boolValue = true;
            if (bool.TryParse(value, out boolValue))
            {
                Dictionary<string, bool> updatedValues = new Dictionary<string, bool>();
                updatedValues.Add(loai, boolValue);
                resultMessage = menu_GroupPermissionService.AddOrUpdate(groupPermissionId, menuId, updatedValues);
            }
            else
            {
                resultMessage = "Giá trị nhập vào không phù hợp.";
            }

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteByIds(List<int> ids)
        {
            string resultMessage = string.Empty;
            if (ids.Count() > 0)
            {
                resultMessage = menu_GroupPermissionService.DeleteByIds(ids);
            }
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

            return Json(menu_GroupPermissionService.GetListExposeDto(parameters[CommonConstants.StrFilters], (Dictionary<string, string>)parameters[CommonConstants.StrInData], out outData), JsonRequestBehavior.AllowGet);
        }
    }
}