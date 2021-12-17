using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class NewsContentViewModels : BaseViewModels
    {
        public NewsContentDto NewsContentVN { set; get; }
        public NewsContentDto NewsContent { set; get; }
        public DepartmentDto Department { set; get; }
        public NewsCategoryDto NewsCategory { set; get; }
        //public NewsCategoryDto ChuyenMuc { set; get; }
        public NewsCategoryDto ChuyenMucVN { set; get; }
        public NewsCategoryDto NewsCategoryVN { set; get; }
        public NewsContentDto Package { set; get; }
        public NewsContentDto PackageVN { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }

        public int LanguageId { set; get; }
    }
}