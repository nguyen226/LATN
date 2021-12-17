using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class PackageServiceDto: BaseEntityDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string keywords { get; set; }
        public string descriptions { get; set; }
        public string avatar { get; set; }
        public string url { get; set; }
        public int categoriId { get; set; }
        public string categoriName { get; set; }
        public string name { get; set; }
        public string shorttitle { get; set; }
        public string description { get; set; }
        public int position { get; set; }
        public bool isactive { get; set; }
        public int languageId { get; set; }
        public string code { get; set; }
        public int priceSale { get; set; }
        public int priceOld { get; set; }
        public DateTime created_at { get; set; }
        public int nView { get; set; }
        public int ishot { get; set; }
    }
}
