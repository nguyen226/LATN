using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JetMedicalWebApp.Controllers
{
    public class SignedAppointmentController : Controller
    {
        SignedAppointmentService signedAppointmentService = new SignedAppointmentService();

        // GET: SignedAppointment
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Signed()
        {
            Boolean Mailsuccess = false;
            string resultMessage = string.Empty;

            string hoten = Request.Params["hoten"].ToString();
            string sodienthoai = Request.Params["sodienthoai"].ToString();
            string checkin = Request.Params["checkin"].ToString();
            string time = Request.Params["time"].ToString();

            Dictionary<string, string> updatedValues = new Dictionary<string, string>();
            updatedValues.Add("fullname", hoten);
            updatedValues.Add("mobile", sodienthoai);
            updatedValues.Add("date", checkin);

            resultMessage = signedAppointmentService.AddOrUpdate(string.Empty, updatedValues);

            if (string.IsNullOrEmpty(resultMessage))
            {
                Mailsuccess = true;
                //Dangkylichhen(hoten, sodienthoai, checkin, time);
            }

            return Json(new { success = Mailsuccess, message = resultMessage }, JsonRequestBehavior.AllowGet);
        }
        public void Dangkylichhen(string hoten, string sodienthoai, string checkin, string time)
        {
            ViewBag.Error = "";
            ViewBag.Success = "";
            ViewBag.isPost = false;

            if (Request.HttpMethod == "POST")
            {
                string smtpAdress = "smtp.gmail.com";
                int portNumber = 587;

                string from = "benhvien199.bca@gmail.com";
                string password = "Bv199216nct";
                
                //getting useful configuration

                StringBuilder body = new StringBuilder();

                //building the body of our email
                body.Append("<html><head> </head><body>");
                body.Append("<div style=' font-family: Arial; font-size: 20px; color: black;'>Bạn có đăng ký lượt khám mới trên website:</br></br>"
                     + "</br>Tên khách hàng: " + hoten
                     + "</br> Số điện thoại: " + sodienthoai
                     + "</br> Ngày khám: " + checkin
                     + "</br> Giờ khám: " + time +
                    "</div>");
                body.Append("</body></html>");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(from);
                    //destination adress
                    mail.To.Add("cskh@benhvien199.vn");
                    mail.CC.Add("tranviethung@benhvien199.vn");
                    mail.Subject = "BV199 - Đăng ký khám bệnh ";
                    mail.Body = body.ToString();
                    //set to true, to specify that we are sending html text.
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient(smtpAdress, portNumber))
                    {
                        //passing the credentials for authentification
                        smtp.Credentials = new NetworkCredential(from, password);
                        //Authentification required
                        smtp.EnableSsl = true;
                        //sending email.
                        smtp.Send(mail);
                        ViewBag.Success = "Success";
                    }
                }
            }
        }
    }
}