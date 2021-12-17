using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class KSK_XNNT
    {
        [Key]
        public int KSK_XNNT_ID { set; get; }
        public string MA_LK { set; get; } 
        public string TEN_XNNT { set; get; }
        public string KQ_XNTT { set; get; }
    }
}
