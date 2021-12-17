using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Contact
    {
        [Key]
        public int id { set; get; }
        public string address { set; get; }
        public string phone { set; get; }
        public string hotline { set; get; }
        public string email { set; get; }
        public string fax { set; get; }
        public string note { set; get; }
        public string footer { set; get; }
        public string maps { set; get; }
        public string latitude { set; get; }
        public string longitude { set; get; }
        public int languageId { set; get; }
        public string languageCode { set; get; }

        public DateTime ModifiedDate { set; get; }
        public DateTime CreatedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
    }
}
