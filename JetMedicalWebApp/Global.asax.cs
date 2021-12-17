using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Security;
using System.Data;

namespace JetMedicalWebApp
{
    public class MvcApplication : HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register); // NEW way
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["Totaluser"] = 0;

            DataSet tmpDs = new DataSet();
            tmpDs.ReadXml(Server.MapPath("~/counter.xml"));
            int hits = Int32.Parse(tmpDs.Tables[0].Rows[0]["hits"].ToString());
            hits += 1;
            tmpDs.Tables[0].Rows[0]["hits"] = hits.ToString();
            tmpDs.WriteXml(Server.MapPath("~/counter.xml"));
            Application["hits"] = hits;
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieUserID];
            if(authCookie != null)
            {
                FormsAuthentication.SetAuthCookie(authCookie.Value, true);
            }
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["Totaluser"] = (int)Application["Totaluser"] + 1;
            Application.UnLock();
        }
    }
}
