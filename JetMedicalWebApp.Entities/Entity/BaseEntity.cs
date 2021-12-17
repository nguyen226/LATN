using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JetMedicalWebApp.Entities.Entity
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = System.DateTime.Now;
            ModifiedDate = System.DateTime.Now;
            IsDisabled = false;
            No = 0;
        }

        [Key]
        public int Id { set; get; }
        public int? No { set; get; }
        [MaxLength()]
        public string Code { set; get; }
        [MaxLength()]
        public virtual string Name { set; get; }
        public System.DateTime CreatedDate { set; get; }
        public System.DateTime ModifiedDate { set; get; }
        public bool IsDisabled { set; get; }
    }
}
