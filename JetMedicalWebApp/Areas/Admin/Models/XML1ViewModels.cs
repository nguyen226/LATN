using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class XML1ViewModels : BaseViewModels
    {
        public XML1Dto XML1 { set; get; }
        public XML3Dto XML3 { set; get; }
        public UserInfoDto UserInfo { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
    }
}