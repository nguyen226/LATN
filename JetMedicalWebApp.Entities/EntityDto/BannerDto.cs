using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class BannerDto
    {
        public int id { set; get; }
        public string code { set; get; }
        public string title { set; get; }
        public string url { set; get; }
        public string text { set; get; }
        public bool isactive { set; get; }
        public int position { set; get; }
        public int languageId { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
    }
}
