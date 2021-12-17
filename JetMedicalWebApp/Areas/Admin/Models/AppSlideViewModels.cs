using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class AppSlideViewModels : BaseViewModels
    {
        public AppSlideDto AppSlide { set; get; }
        public NewsContentDto NewsContent { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
    }
}