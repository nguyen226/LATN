using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JetMedicalWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            //about
            routes.MapRoute(
                name: "about",
                url: "{metatitle}_a_{id}",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "about2",
                url: "{cate}/{metatitle}_a_{id}",
                defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            //contact
            routes.MapRoute(
                name: "contact",
                url: "{metatitle}_c_{id}",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "contact2",
                url: "{cate}/{metatitle}_c_{id}",
                defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            //signedservice
            routes.MapRoute(
                name: "signedservice",
                url: "{metatitle}_s_{id}",
                defaults: new { controller = "SignedService", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "signedservice2",
                url: "{cate}/{metatitle}_s_{id}",
                defaults: new { controller = "SignedService", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            routes.MapRoute(
                name: "signedservice3",
                url: "{metatitle}_p_{id}",
                defaults: new { controller = "SignedService", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "signedservice4",
                url: "{cate}/{metatitle}_p_{id}",
                defaults: new { controller = "SignedService", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            //news
            routes.MapRoute(
                name: "news",
                url: "{metatitle}_n_{id}",
                defaults: new { controller = "ListNews", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "news1",
                url: "{metatitle}_pdt_{id}",
                defaults: new { controller = "SignedService", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            //routes.MapRoute(
            //    name: "news2",
            //    url: "{cate}/{metatitle}_n_{id}",
            //    defaults: new { controller = "ListNews", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "JetMedicalWebApp.Controllers" }
            //);

            routes.MapRoute(
                name: "news3",
                url: "{metatitle}_dt_{id}",
                defaults: new { controller = "NewsDetail", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "news4",
                url: "{metatitle}_dt_{id}",
                defaults: new { controller = "NewsDetail", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "news5",
                url: "{cate}/{metatitle}_dt_{id}",
                defaults: new { controller = "NewsDetail", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "news6",
                url: "{type}/{cate}/{metatitle}_dt_{id}",
                defaults: new { controller = "NewsDetail", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
                name: "news7",
                url: "{cate}/{metatitle}_hd_{id}",
                defaults: new { controller = "NewsDetail", action = "Cate", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            // ket qua online
            routes.MapRoute(
                name: "ResultOnline",
                url: "result-online",
                defaults: new { controller = "ResultOnline", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            // Dang ky lich kham
            routes.MapRoute(
                name: "SignedAppointment",
                url: "signed-appointment",
                defaults: new { controller = "SignedAppointment", action = "Signed", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            // List Staff
            routes.MapRoute(
                name: "staff2",
                url: "{cate}/{metatitle}_st_{id}",
                defaults: new { controller = "ListStaffs", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            //// Staff
            //routes.MapRoute(
            //    name: "StaffDetail",
            //    url: "{cate}/{metatitle}_sf_{id}",
            //    defaults: new { controller = "StaffDetail", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "JetMedicalWebApp.Controllers" }
            //);
            // Staff
            routes.MapRoute(
                name: "Search",
                url: "{metatitle}_rp",
                defaults: new { controller = "Search", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            // Dang ky thanh vien
            routes.MapRoute(
                name: "RegisterMember",
                url: "member-register",
                defaults: new { controller = "Home", action = "DangKy", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            // Gui lai code active
            routes.MapRoute(
                name: "ReSendActiveCode",
                url: "resend-activate-code",
                defaults: new { controller = "Home", action = "ResendCode", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            // Active code
            routes.MapRoute(
                name: "ActiveCode",
                url: "member-active",
                defaults: new { controller = "Home", action = "ActivateCode", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );


            // Active code
            routes.MapRoute(
                name: "ActiveCodeSDTEmail",
                url: "member-active-dang-nhap",
                defaults: new { controller = "Home", action = "SDTEmailActivateCode", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            // Member Login
            routes.MapRoute(
                name: "MemberLogin",
                url: "member-login",
                defaults: new { controller = "Home", action = "DangNhap", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
            // Member Logout
            routes.MapRoute(
                name: "MemberLogout",
                url: "dang-xuat",
                defaults: new { controller = "Home", action = "DangXuat", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            // Tag
            routes.MapRoute(
                name: "tag",
                url: "tag/{id}",
                defaults: new { controller = "ListNews", action = "Tag", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            // Chuyen khoa
            routes.MapRoute(
                name: "ChuyenKhoa",
                url: "chuyen-khoa",
                defaults: new { controller = "ChuyenKhoa", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
             name: "ChuyenKhoaList",
             url: "{cate}/{metatitle}_ck_{id}/{slug}",
             defaults: new { controller = "ChuyenKhoa", action = "List", id = UrlParameter.Optional },
             namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
             name: "ChuyenKhoaChiTietBacSi",
             url: "{cate}/{metatitle}_sf_{idct}",
             defaults: new { controller = "ChuyenKhoa", action = "ChiTietBacSi", id = UrlParameter.Optional },
             namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );

            routes.MapRoute(
            name: "ChuyenKhoaChiTietTinTuc",
            url: "{cate}/{urlChiTiet}_ct_{idct}",
            defaults: new { controller = "ChuyenKhoa", action = "ChiTietTinTuc", id = UrlParameter.Optional },
            namespaces: new[] { "JetMedicalWebApp.Controllers" }
           );


            // Default
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JetMedicalWebApp.Controllers" }
            );
        }
    }
}
