using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class LichSuGuiChienDich : BaseEntity
    {
        public int ChienDichId { set; get; }
        public int UserId { set; get; }
        public int CompanyId { set; get; }
        public int TrangThai { set; get; }//1 Thành công , 2 thất bại
        public string EmailId { set; get; }
        public string TieuDe { set; get; }
        public string NoiDung { set; get; }
        public string TomTat { set; get; }
        public string GhiChu { set; get; }
    }
}
