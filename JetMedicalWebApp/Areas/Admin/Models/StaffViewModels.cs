using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class StaffViewModels : BaseViewModels
    {
        public StaffDto StaffVN { set; get; }
        public StaffDto Staff { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
        public int LanguageId { set; get; }
    }
}