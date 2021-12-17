using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetMedicalWebApp.Entities.Entity
{
    public class Menu_GroupPermission : BaseEntity
    {
        public bool SystemView { set; get; }
        public bool PersonalView { set; get; }
        public bool PersonalDelete { set; get; }
        public bool SystemDelete { set; get; }
        public bool SystemEdit { set; get; }
        public bool PersonalEdit { set; get; }
        public virtual Menu Menu { set; get; }
        public virtual GroupPermission GroupPermission { set; get; }
        public virtual Users ModifiedUsers { set; get; }
        public virtual Users CreatedUsers { set; get; }
    }
}
