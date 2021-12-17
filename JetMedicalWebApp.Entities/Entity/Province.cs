using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Province
    {
        [Key]
        public string ProvinceID { set; get; }
        public string ProvinceName { set; get; }
        public int ODX { set; get; }
        public bool IsActive { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
