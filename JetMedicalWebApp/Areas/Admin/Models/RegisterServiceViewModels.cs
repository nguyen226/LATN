using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class RegisterServiceViewModels : BaseViewModels
    {
        public RegisterServiceDto RegisterService { set; get; }
        public StaffDto Staff { set; get; }
        public NewsContentDto Package { set; get; }
        public DepartmentDto Department { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
        public int NamBatDauSuDungPhamMem { set; get; }
    }
}