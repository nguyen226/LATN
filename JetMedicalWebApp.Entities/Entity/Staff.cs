using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace JetMedicalWebApp.Entities.Entity
{
    public class Staff
    {
        public Staff()
        {
            CreatedDate = System.DateTime.Now;
            ModifiedDate = System.DateTime.Now;
            isactive = false;
            Isexamination = false;
        }

        [Key]
        public int id { set; get; }
        public string img { set; get; }
        public string brandname { set; get; }
        public string fullname { set; get; }
        public string position { set; get; }
        public string summary { set; get; }
        public string description { set; get; }
        public bool isactive { set; get; }
        public int languageId { set; get; }
        public int? newscategoryid { set; get; }
        public int? departmentid { set; get; }
        public string code { set; get; }
        public bool Isexamination { set; get; }
        public System.DateTime CreatedDate { set; get; }
        public System.DateTime ModifiedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
        public int order { set; get; }
    }
}
