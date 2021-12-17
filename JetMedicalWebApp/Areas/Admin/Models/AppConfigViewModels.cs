using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class AppConfigViewModels : BaseViewModels
    {
        public AppConfigDto AppConfig { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
        public string NgayBatDauLocDuLieu { set; get; }
        public string NgayKetThucLocDuLieu { set; get; }
    }
}