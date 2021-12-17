using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Models
{
    public class BaseViewModel
    {
        public int LanguageId { set; get; }
        public bool IsList { set; get; }
        public NewsContentDto NewsContent { set; get; }
        public List<DataThongKeDto> Tags { set; get; }
        public ContactDto Contact { set; get; }
        public ResourceDto Resource { set; get; }
        public IDictionary<string, string> IDicResource { set; get; }

        public List<BannerDto> Banners { set; get; }
        public List<feedbackDto> Feedbacks { set; get; }
        public List<StaffDto> Staffs { set; get; }
        public List<NewsCategoryDto> NewsCategories { set; get; }
        public List<NewsContentDto> Packages { set; get; }
        public List<NewsContentDto> News { set; get; }
        public List<NewsContentDto> NewsTrongNuoc { set; get; }
        public List<NewsContentDto> NewsQuocTe { set; get; }
        public List<NewsTypeDto> NewsTypes { set; get; }
        public List<NewsContentDto> NewsContents { set; get; }
        public List<VideoDto> Videos { set; get; }
        public int MemberId { set; get; }
        public string MemberName { set; get; }

        public int TongSoTrang { set; get; }
        public int PageIndex { set; get; }
        public string Url { set; get; }
        public string Text { set; get; }
        public string TextPhongBan { set; get; }
        public int PhongBanId { set; get; }
    }
}