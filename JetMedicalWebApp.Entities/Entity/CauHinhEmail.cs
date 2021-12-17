using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace JetMedicalWebApp.Entities.Entity
{
    public class CauHinhEmail : BaseEntity
    {
        public string Host { set; get; }
        public string Port { set; get; }
        public bool SSL { set; get; }
        public bool Active { set; get; }
        public string Account { set; get; }
        public string Password { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
