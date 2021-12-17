using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class ContactViewModels : BaseViewModels
    {
        public int LanguageId { set; get; }
        public ContactDto ContactVN { set; get; }
        public ContactDto ContactEN { set; get; }
        public Menu_GroupPermissionDto Menu_GroupPermission { set; get; }
    }
}