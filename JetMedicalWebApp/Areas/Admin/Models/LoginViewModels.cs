using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Areas.Admin.Models
{
    public class LoginViewModels
    {
        [Required]
        [Display(Name = "User name")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string ReturnUrl { set; get; }
        public string Message { set; get; }
        public string Logo { set; get; }
    }
}