using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class ServiceViewModels : BaseViewModels
    {
        public ServiceDto ServiceVN { set; get; }
        public ServiceDto Service { set; get; }
        public NewsCategoryDto NewsCategory { set; get; }
        //public NewsCategoryDto ChuyenMuc { set; get; }
        public NewsCategoryDto ChuyenMucVN { set; get; }
        public NewsCategoryDto NewsCategoryVN { set; get; }
        public ServiceDto Package { set; get; }
        public ServiceDto PackageVN { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
        public int LanguageId { set; get; }
        public string ServiceCode { set; get; }
    }
}
