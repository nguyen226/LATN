using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class NhanTuVanDto
    {
        public int Id { set; get; }
        public string HoVaTen { set; get; }
        public string SoDienThoai { set; get; }
        public string Email { set; get; }
        public string TieuDe { set; get; }
        public string NoiDung { set; get; }
        public int TrangThai { set; get; } // 1 mới / 2 đã xử lý
        // thêm vào db mới
        public string CreateDate { set; get; }
        public string ModifiedDate { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }
    }
}
