using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class feedback
    {
        [Key]
        public int id { set; get; }
        public string title { set; get; }
        public string avatar { set; get; }
        public string fullname { set; get; }
        public string content { set; get; }
        public bool approved { set; get; }
        public bool isactive { set; get; }
        public int languageId { set; get; }
        public string code { set; get; }
        public DateTime created_at { set; get; }
        public DateTime ModifiedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
    }
}
