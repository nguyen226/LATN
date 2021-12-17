using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetMedicalWebApp.Entities.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace JetMedicalWebApp.DAL
{
    public class ProjectDbContext : System.Data.Entity.DbContext
    {
        public ProjectDbContext()
            : base("ProjectConnection")
        {
            // Database.SetInitializer(new MyInitializer());
            Database.SetInitializer<ProjectDbContext>(null);
        }

        public ProjectDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public DbSet<AppConfig> AppConfigs { set; get; }
        public DbSet<AppHistory> AppHistorys { set; get; }
        public DbSet<SignedAppointment> SignedAppointments { set; get; }
        public DbSet<AppSlide> AppSlides { set; get; }
        public DbSet<banner> banners { set; get; }
        public DbSet<BloodType> BloodTypes { set; get; }
        public DbSet<CauHinhEmail> CauHinhEmails { set; get; }
        public DbSet<Company> Companys { set; get; }
        public DbSet<Company_GroupPermission> Company_GroupPermissions { set; get; }
        public DbSet<Contact> Contacts { set; get; }
        public DbSet<ChienDich> ChienDichs { set; get; }
        public DbSet<Department> Departments { set; get; }
        public DbSet<District> Districts { set; get; }
        public DbSet<feedback> feedbacks { set; get; }
        public DbSet<GroupPermission> GroupPermissions { set; get; }
        public DbSet<ICD10> ICD10s { set; get; }
        public DbSet<Khoa> Khoas { set; get; }
        public DbSet<LichSuGuiChienDich> LichSuGuiChienDichs { set; get; }
        public DbSet<NhomChienDich> NhomChienDichs { set; get; }
        public DbSet<Groups> Groupss { set; get; }
        public DbSet<Menu> Menus { set; get; }
        public DbSet<Menu_GroupPermission> Menu_GroupPermissions { set; get; }
        public DbSet<NewsCategory> NewsCategorys { set; get; }
        public DbSet<NewsContent> NewsContents { set; get; }
        public DbSet<NewsContent_Tag> NewsContent_Tags { set; get; }
        public DbSet<NewsType> NewsTypes { set; get; }
        public DbSet<NhanTuVan> NhanTuVans { set; get; }
        public DbSet<Notification> Notifications { set; get; }
        public DbSet<PackageService> PackageServices { set; get; }
        public DbSet<PatientGroup> PatientGroups { set; get; }
        public DbSet<PatientGroup_Users> PatientGroup_Userss { set; get; }
        public DbSet<Province> Provinces { set; get; }
        public DbSet<UserInfo> UserInfos { set; get; }
        public DbSet<RegisterService> RegisterServices { set; get; }
        public DbSet<resource> Resources { set; get; }
        public DbSet<Service> Services { set; get; }
        public DbSet<Service_Tag> Service_Tags { set; get; }
        public DbSet<SignedService> SignedServices { set; get; }
        public DbSet<Staff> Staffs { set; get; }
        public DbSet<Tag> Tags { set; get; }
        public DbSet<Users> Userss { set; get; }
        public DbSet<Users_GroupPermission> Users_GroupPermissions { set; get; }
        public DbSet<Video> Videos { set; get; }
        public DbSet<XML1> XML1s { set; get; }
        public DbSet<XML2> XML2s { set; get; }
        public DbSet<XML3> XML3s { set; get; }
        public DbSet<XML4> XML4s { set; get; }
        public DbSet<KSK_CDHA> KSK_CDHAs { set; get; }
        public DbSet<KSK_TTBENHNHAN> KSK_TTBENHNHANs { set; get; }
        public DbSet<KSK_XNCTM> KSK_XNCTMs { set; get; }
        public DbSet<KSK_XNNT> KSK_XNNTs { set; get; }
        public DbSet<KSK_XNK> KSK_XNKs { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
