using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class District
    {
        [Key]
        public string DistrictID { set; get; }
        public string ProvinceID { set; get; }
        public string DistrictName { set; get; }
        public bool IsActive { set; get; }
        public int ODX { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
