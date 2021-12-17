using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class SignedAppointment
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string date { get; set; }
        public int departmentId { get; set; }
        public int doctorId { get; set; }
        public string created_at { get; set; }
    }
}
