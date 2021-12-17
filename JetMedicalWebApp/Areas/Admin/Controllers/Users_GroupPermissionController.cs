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
    public class Users_GroupPermissionController : Controller
    {
        Users_GroupPermissionService users_GroupPermissionService = new Users_GroupPermissionService();

        [HttpPost]
        public ActionResult GetListByGroupPermissionId(string userId)
        {
            string selectedFields = "Id, Users.UserID AS UserID, GroupPermission.Id AS GroupPermissionId, GroupPermission.Name AS GroupPermissionName";
            return Json(users_GroupPermissionService.GetListByGroupUserIdExposeDto(userId, selectedFields), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddOrUpdate(int groupPermissionId, int userId)
        {
            string resultMessage = string.Empty;
            resultMessage = users_GroupPermissionService.AddOrUpdate(groupPermissionId, userId);
            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteById(int id)
        {
            string resultMessage = string.Empty;
            resultMessage = users_GroupPermissionService.DeleteById(id);

            return Json(resultMessage, JsonRequestBehavior.AllowGet);
        }
    }
}