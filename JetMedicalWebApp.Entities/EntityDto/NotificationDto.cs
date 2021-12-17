using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class NotificationDto : BaseEntityDto
    {      
        public int NoID { set; get; }
        public int NoCategory { set; get; } //1,2,3,4,5,6
        public string NoTitle { set; get; }
        public string NoDes { set; get; }
        public int NoType { set; get; } // Kiểu thông báo, 1 app, 2 email, 3 tin nhắn
        public string NewID { set; get; } //ID của bài viết tin tức, dịch vụ
        public string NewTitle { set; get; } //Name của bài viết tin tức, dịch vụ
        public string NoHTML { set; get; }
        public string Image { set; get; }   
        public bool NoStatus { set; get; }
        public string CreateDate { set; get; }
    }
}
