using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Service_Tag : BaseEntity
    {
        public virtual Service Service { set; get; }
        public virtual Tag Tag { set; get; }
    }
}
