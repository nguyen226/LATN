using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class AppSlide : BaseEntity
    {       
        public string Images { set; get; }
        public bool Active { set; get; }
        public int NoCategory { set; get; }
        public int NoID { set; get; }
        public int Odx { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
