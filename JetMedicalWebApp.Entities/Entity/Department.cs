using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Department
    {
        public Department()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [Key]
        public int id { set; get; }
        public string code { set; get; }
        public string name { set; get; }
        public int? languageId { set; get; }
        public int? isGroup { set; get; }
        public bool isActive { set; get; }
        public System.DateTime CreatedDate { set; get; }
        public System.DateTime ModifiedDate { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
