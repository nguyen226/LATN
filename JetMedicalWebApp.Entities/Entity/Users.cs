using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Users 
    {
        [Key]
        public int UserID { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string EmailID { set; get; } 
        public string Phone { set; get; }
        public string Password { set; get; }
        public bool IsVerified { set; get; }
        public string ActivationCode { set; get; }
        public string ResetPasswordCode { set; get; }
        public int GroupID { set; get; }
        public bool Active { set; get; }
        public DateTime? LastLoginDate { set; get; }
        public DateTime? LastAccessedDate { set; get; }
        public int LoginFailedCount { set; get; }
        public string Avartar { set; get; }
        public DateTime CreateDate { set; get; }
        public bool IsMobile { set; get; }
        public bool IsAdmin { set; get; }
        public int CreatedUserID { set; get; }
        public int ModifiedUserID { set; get; }
        public virtual ICollection<Users_GroupPermission> Users_GroupPermissions { set; get; }
    }
}
