using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class resource
    {
        [Key]
        public int id { set; get; }
        public int languageId { set; get; }
        public string code { set; get; }
        public string name { set; get; }
        public bool isView { set; get; }
    }
}
