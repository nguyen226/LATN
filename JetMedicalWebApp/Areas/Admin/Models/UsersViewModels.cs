using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class UsersViewModels : BaseViewModels
    {
        public UsersDto Users { set; get; }
        public UserInfoDto UserInfo { set; get; }
        public GroupsDto Groups { set; get; }
        public ProvinceDto Province { set; get; }
        public DistrictDto District { set; get; }
        public BloodTypeDto BloodType { set; get; }
        public CompanyDto Company { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
        public Company_GroupPermissionDto Company_GroupPermission { set; get; }
    }
}