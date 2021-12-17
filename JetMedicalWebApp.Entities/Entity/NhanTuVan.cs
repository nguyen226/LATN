using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class NhanTuVan
    {
        public NhanTuVan()
        {
            CreateDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        [Key]
        public int Id { set; get; }
        public string HoVaTen { set; get; }
        public string SoDienThoai { set; get; }
        public string Email { set; get; }
        public string TieuDe { set; get; }
        public string NoiDung { set; get; }
        public int TrangThai { set; get; }
        public int CreatedUserID { set; get; }
        public DateTime CreateDate { set; get; }
        public DateTime ModifiedDate { set; get; }
    }
}
