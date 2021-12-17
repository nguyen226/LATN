using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace JetMedicalWebApp.DAL
{
    public class MyInitializer : DropCreateDatabaseIfModelChanges<ProjectDbContext>
    {
        protected override void Seed(ProjectDbContext context)
        {
            base.Seed(context);
        }
    }
}