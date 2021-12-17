using JetMedicalWebApp.Entities.EntityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JetMedicalWebApp.Models
{
    public class PartialViewModel
    {
        public int LanguageId { set; get; }
        public string HomeText { set; get; }
        public List<NewsCategoryDto> NewsCategories { set; get; }
        public List<NewsTypeDto> NewsTypes { set; get; }
        public List<NewsContentDto> NewsContents { set; get; }
        public List<NewsContentDto> Packages { set; get; }
        public List<VideoDto> Videos { set; get; }
        public List<DataThongKeDto> Tags { set; get; }
        public int MemberId { set; get; }
        public string MemberName { set; get; }
    }
}