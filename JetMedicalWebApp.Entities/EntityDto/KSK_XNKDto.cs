using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.EntityDto
{
    public class KSK_XNKDto
    {
        public int KSK_XNK_ID { set; get; }
        public string MA_LK { set; get; } 
        public string TEN_XNK { set; get; }
        public string KQ_XNK { set; get; }
    }
}
