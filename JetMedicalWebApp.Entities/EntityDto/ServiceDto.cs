using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class ServiceDto
    {
        public int id { set; get; }
        public string title { set; get; }
        public string keywords { set; get; }
        public string descriptions { set; get; }
        public string avatar { set; get; }
        public string url { set; get; }
        public int categoriId { set; get; }
        public string categoriName { set; get; }
        public string name { set; get; }
        public string shorttitle { set; get; }
        public string description { set; get; }
        public string created_by { set; get; }
        public bool approved { set; get; }
        public int position { set; get; }
        public int nView { set; get; }
        public bool isactive { set; get; }
        public bool ishot { set; get; }
        public int languageId { set; get; }
        public string code { set; get; }
        public int isGroup { set; get; }
        public string updated_by { set; get; }
        public int priceSale { set; get; }
        public int priceOld { set; get; }
        public bool ActivePhone { set; get; }
        public string Noted { set; get; }
        public string fullname { set; get; }
        public string fullshorttitle { set; get; }
       
        // thêm vào db mới
        public string created_at { set; get; }
        public string updated_at { set; get; }
        public int ModifiedUserID { set; get; }
        public int CreatedUserID { set; get; }

        public int TypeId { set; get; }
        public string TypeName { set; get; }
        public string TypeCode { set; get; }
        public string Tags { set; get; }

        public int PackageId { set; get; }
        public string GoiKham { set; get; }
        public int ChuyenMuc { set; get; }
        public string slug { set; get; }

    }
}
