using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class PatientGroup_UsersViewModels : BaseViewModels
    {
        public PatientGroup_UsersDto PatientGroup_Users { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
    }
}