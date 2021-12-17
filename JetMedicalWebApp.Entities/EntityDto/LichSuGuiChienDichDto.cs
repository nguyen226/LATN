using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class LichSuGuiChienDichDto : BaseEntityDto
    {
        public int ChienDichId { set; get; }
        public int UserId { set; get; }
        public int CompanyId { set; get; }
        public string HO_TEN { set; get; }
        public int TrangThai { set; get; }
        public string EmailId { set; get; }
        public string GhiChu { set; get; }
        public string TieuDe { set; get; }
        public string NoiDung { set; get; }
        public string TomTat { set; get; }
    }
}
