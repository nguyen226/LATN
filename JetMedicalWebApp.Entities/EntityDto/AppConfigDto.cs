using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class AppConfigDto : BaseEntityDto
    {
        public int ID { set; get; }
        public string CompanyName { set; get; }
        public string Logo { set; get; }
        public bool Active { set; get; }
        public bool PhoneAuthentication { set; get; }
        public string Hotline { set; get; }
        public string Facebook { set; get; }
        public string Zalo { set; get; }
        public string Website { set; get; }
        public string Email { set; get; }
        public string SmsAccount { set; get; }
        public string SmsPass { set; get; }
        public string SmsLink { set; get; }
        public string MailAccount { set; get; }
        public string MailPass { set; get; }
        public string MailPort { set; get; }
        public string MailHost { set; get; }
        public bool MailSSL { set; get; }
        public string Introduce { set; get; }
        public string Privacy { set; get; }
        public string TermsOfUse { set; get; }
        public string Support { set; get; }
        public string CreateDate { set; get; }

        //hungtv
        public string Title { set; get; }
        public string Description { set; get; }
        public string Keywords { set; get; }
    }
}
