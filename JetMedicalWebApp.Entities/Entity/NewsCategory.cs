using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class NewsCategory
    {
        [Key]
        public int id { set; get; }
        public string title { set; get; }
        public string keywords { set; get; }
        public string descriptions { set; get; }
        public string url { set; get; }
        public string name { set; get; }
        public string description { set; get; }
        public int typeId { set; get; }
        public string banner { set; get; }
        public bool isHome { set; get; }
        public bool isactive { set; get; }
        public int languageId { set; get; }
        public string code { set; get; }
        public int isGroup { set; get; }
        public bool ActivePhone { set; get; }
        public string IconPhone { set; get; }


        public DateTime ModifiedDate { set; get; }
        public DateTime CreatedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
        public string slug { set; get; }
        public string parent_ids { set; get; }
        public string child_ids { set; get; }
        public int odx { set; get; }
        public int parentId { set; get; }

    }
}
