﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class KSK_CDHA
    {
        [Key]
        public int KSK_CDHA_ID { set; get; }
        public string MA_LK { set; get; } 
        public string TEN_CDHA { set; get; }
        public string KET_QUA { set; get; }
    }
}
