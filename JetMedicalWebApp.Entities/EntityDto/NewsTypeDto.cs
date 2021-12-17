using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class NewsTypeDto 
    {
        public int id { set; get; }
        public string code { set; get; }
        public string name { set; get; }
        public string show { set; get; }
        public bool isactive { set; get; }
        public bool isMennu { set; get; }
        public int languageId { set; get; }
        public string languageCode { set; get; }
        public int orderBy { set; get; }
        public bool isCate { set; get; }


        // thêm vào db mới
        public string CreatedDate { set; get; }
        public string ModifiedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
        public string slug { set; get; }
    }
}
