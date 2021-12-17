using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class StaffDto
    {
        public int id { set; get; }
        public string img { set; get; }
        public string brandname { set; get; }
        public string fullname { set; get; }
        public string position { set; get; }
        public string summary { set; get; }
        public string description { set; get; }
        public bool isactive { set; get; }
        public int? newscategoryid { set; get; }
        public int languageId { set; get; }
        public int departmentid { set; get; }
        public string departmentname { set; get; }
        public string newcategoryname { set; get; }
        public string code { set; get; }
        public bool Isexamination { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
        public string CreatedDate { set; get; }
        public string ModifiedDate { set; get; }
        public int order { set; get; }
    }
}
