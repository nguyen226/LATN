﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Khoa : BaseEntity
    {       
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
