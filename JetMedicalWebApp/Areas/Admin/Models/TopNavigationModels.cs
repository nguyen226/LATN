using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class TopNavigationModels
    {
        public UsersDto User { set; get; }
        public string Logo { set; get; }
    }
}