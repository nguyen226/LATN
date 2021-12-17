using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class SignedService
    {
        public int id { get; set; }
        public string code { get; set; }
        public string fullname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string date { get; set; }
        public int packageId { get; set; }
        public string packageName { get; set; }
        public string packageCode { get; set; }
        public int packagePrice { get; set; }
        public string created_at { get; set; }
    }
}
