using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { areas = "Admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "JetMedicalWebApp.Areas.Admin.Controllers" }
            );
        }
    }
}