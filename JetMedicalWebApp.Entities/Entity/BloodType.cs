using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace JetMedicalWebApp.Entities.Entity
{
    public class BloodType
    {
        public BloodType()
        {
            CreatedDate = System.DateTime.Now;
            ModifiedDate = System.DateTime.Now;
        }

        [Key]
        public int BloodTypeID { set; get; }
        public string BloodName { set; get; }
        public System.DateTime CreatedDate { set; get; }
        public System.DateTime ModifiedDate { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
