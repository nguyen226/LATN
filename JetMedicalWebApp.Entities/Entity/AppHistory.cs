using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace JetMedicalWebApp.Entities.Entity
{
    public class AppHistory : BaseEntity
    {
        public string Ma { set; get; }
        public string Ten { set; get; }
        public string ThaoTac { set; get; }
        public int UserId { set; get; }
        public string GhiChu { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
