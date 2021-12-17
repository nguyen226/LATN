using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Net.Mail;
using JetMedicalWebApp.Entities.EntityDto;
using System.IO;

namespace JetMedicalWebApp.Common
{
    public class GmailService
    {
        public GmailService()
        {
           
        }

        public string GuiEmailToiUser(SmtpConfigurationDto _config, EmailMessageDto message, string duongDanHoaDonDienTu)
        {
            string resultMessage = string.Empty;
            try
            {
                var smtp = new SmtpClient
                {
                    Host = _config.Host,
                    Port = _config.Port,
                    EnableSsl = _config.Ssl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 5000,
                    Credentials = new NetworkCredential(_config.Username, _config.Password),
                };

                MailAddress to = new MailAddress(message.ToEmail);
                MailAddress from = new MailAddress(_config.Username,string.Empty);

                using (var smtpMessage = new MailMessage(from, to))
                {

                    if (!string.IsNullOrEmpty(duongDanHoaDonDienTu))
                    {
                        WebClient myClient = new WebClient();
                        byte[] bytes = myClient.DownloadData(duongDanHoaDonDienTu);
                        var duoiFile = duongDanHoaDonDienTu.Split('.');

                        System.IO.MemoryStream file = new MemoryStream(bytes);
                        smtpMessage.Attachments.Add(new Attachment(file, "image." + duoiFile[duoiFile.Length - 1]));
                    }

                    smtpMessage.Subject = message.Subject;
                    smtpMessage.Body = message.Body;
                    smtpMessage.IsBodyHtml = message.IsHtml;
                    smtp.Send(smtpMessage);
                }
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
            }

            return resultMessage;
        }
    }
}
