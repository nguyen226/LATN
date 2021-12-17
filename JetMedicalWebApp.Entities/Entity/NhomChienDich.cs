using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class NhomChienDich : BaseEntity
    {
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
