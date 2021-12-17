using System.Web.Mvc;

namespace JetMedicalWebApp.Areas.Member
{
    public class MemberAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Member";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Member_default",
                "Member/{controller}/{action}/{id}",
                new { areas = "Member", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "JetMedicalWebApp.Areas.Member.Controllers" }
            );
        }
    }
}