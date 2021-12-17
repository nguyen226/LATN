using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class VideoDto
    {
        public int id { set; get; }
        public string title { set; get; }
        public string keywords { set; get; }
        public string descriptions { set; get; }
        public string avatar { set; get; }
        public string url { set; get; }
        public string name { set; get; }
        public string shorttitle { set; get; }
        public string description { set; get; }
        public int position { set; get; }
        public int totalView { set; get; }
        public bool isactive { set; get; }
        public int languageId { set; get; }
        public string code { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
    }
}
