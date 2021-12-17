using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetMedicalWebApp.Entities.Entity;

namespace JetMedicalWebApp.DAL
{
    public class UnitOfWork : IDisposable
    {
        private ProjectDbContext context = new ProjectDbContext();
        private GenericRepository<AppConfig> appConfigRepository;
        private GenericRepository<AppHistory> appHistoryRepository;
        private GenericRepository<SignedAppointment> signedAppointmentRepository;
        private GenericRepository<AppSlide> appSlideRepository;
        private GenericRepository<banner> bannerRepository;
        private GenericRepository<BloodType> bloodTypeRepository;
        private GenericRepository<CauHinhEmail> cauHinhEmailRepository;
        private GenericRepository<Company> companyRepository;
        private GenericRepository<Company_GroupPermission> company_GroupPermissionRepository;
        private GenericRepository<Contact> contactRepository;
        private GenericRepository<ChienDich> chienDichRepository;
        private GenericRepository<Department> departmentRepository;
        private GenericRepository<District> districtRepository;
        private GenericRepository<feedback> feedbackRepository;
        private GenericRepository<GroupPermission> groupPermissionRepository;
        private GenericRepository<Groups> groupsRepository;
        private GenericRepository<ICD10> iCD10Repository;
        private GenericRepository<Khoa> khoaRepository;
        private GenericRepository<LichSuGuiChienDich> lichSuGuiChienDichRepository;
        private GenericRepository<Menu> menuRepository;
        private GenericRepository<Menu_GroupPermission> menu_GroupPermissionRepository;
        private GenericRepository<Notification> notificationRepository;
        private GenericRepository<NewsCategory> newsCategoryRepository;
        private GenericRepository<NewsContent_Tag> newsContent_TagRepository;
        private GenericRepository<NewsContent> newsContentRepository;
        private GenericRepository<NhanTuVan> nhanTuVanRepository;
        private GenericRepository<NewsType> newsTypeRepository;
        private GenericRepository<NhomChienDich> nhomChienDichRepository;
        private GenericRepository<PackageService> packageServiceRepository;
        private GenericRepository<PatientGroup> patientGroupRepository;
        private GenericRepository<PatientGroup_Users> patientGroup_UsersRepository;
        private GenericRepository<Province> provinceRepository;
        private GenericRepository<RegisterService> registerServiceRepository;
        private GenericRepository<resource> resourceRepository;
        private GenericRepository<Service> serviceRepository;
        private GenericRepository<Service_Tag> service_TagRepository;
        private GenericRepository<SignedService> signedServiceRepository;
        private GenericRepository<Staff> staffRepository;
        private GenericRepository<Tag> tagRepository;
        private GenericRepository<UserInfo> userInfoRepository;
        private GenericRepository<Users> usersRepository;
        private GenericRepository<Users_GroupPermission> users_GroupPermissionRepository;
        private GenericRepository<Video> videoRepository;
        private GenericRepository<XML1> xML1Repository;
        private GenericRepository<XML2> xML2Repository;
        private GenericRepository<XML3> xML3Repository;
        private GenericRepository<XML4> xML4Repository;
        private GenericRepository<KSK_CDHA> kSK_CDHARepository;
        private GenericRepository<KSK_TTBENHNHAN> kSK_TTBENHNHANRepository;
        private GenericRepository<KSK_XNCTM> kSK_XNCTMRepository;
        private GenericRepository<KSK_XNNT> kSK_XNNTRepository;
        private GenericRepository<KSK_XNK> kSK_XNKRepository;

        public GenericRepository<KSK_XNK> KSK_XNKRepository
        {
            get
            {
                this.kSK_XNKRepository = new GenericRepository<KSK_XNK>(context);
                return kSK_XNKRepository;
            }
        }


        public GenericRepository<KSK_XNNT> KSK_XNNTRepository
        {
            get
            {
                this.kSK_XNNTRepository = new GenericRepository<KSK_XNNT>(context);
                return kSK_XNNTRepository;
            }
        }

        public GenericRepository<KSK_XNCTM> KSK_XNCTMRepository
        {
            get
            {
                this.kSK_XNCTMRepository = new GenericRepository<KSK_XNCTM>(context);
                return kSK_XNCTMRepository;
            }
        }

        public GenericRepository<KSK_TTBENHNHAN> KSK_TTBENHNHANRepository
        {
            get
            {
                this.kSK_TTBENHNHANRepository = new GenericRepository<KSK_TTBENHNHAN>(context);
                return kSK_TTBENHNHANRepository;
            }
        }

        public GenericRepository<KSK_CDHA> KSK_CDHARepository
        {
            get
            {
                this.kSK_CDHARepository = new GenericRepository<KSK_CDHA>(context);
                return kSK_CDHARepository;
            }
        }


        public GenericRepository<AppConfig> AppConfigRepository
        {
            get
            {
                this.appConfigRepository = new GenericRepository<AppConfig>(context);
                return appConfigRepository;
            }
        }
        public GenericRepository<AppHistory> AppHistoryRepository
        {
            get
            {
                this.appHistoryRepository = new GenericRepository<AppHistory>(context);
                return appHistoryRepository;
            }
        }
        public GenericRepository<SignedAppointment> SignedAppointmentRepository
        {
            get
            {
                this.signedAppointmentRepository = new GenericRepository<SignedAppointment>(context);
                return signedAppointmentRepository;
            }
        }
        public GenericRepository<AppSlide> AppSlideRepository
        {
            get
            {
                this.appSlideRepository = new GenericRepository<AppSlide>(context);
                return appSlideRepository;
            }
        }

        public GenericRepository<banner> BannerRepository
        {
            get
            {
                this.bannerRepository = new GenericRepository<banner>(context);
                return bannerRepository;
            }
        }

        public GenericRepository<BloodType> BloodTypeRepository
        {
            get
            {
                this.bloodTypeRepository = new GenericRepository<BloodType>(context);
                return bloodTypeRepository;
            }
        }
        public GenericRepository<CauHinhEmail> CauHinhEmailRepository
        {
            get
            {
                this.cauHinhEmailRepository = new GenericRepository<CauHinhEmail>(context);
                return cauHinhEmailRepository;
            }
        }

        public GenericRepository<Company> CompanyRepository
        {
            get
            {
                this.companyRepository = new GenericRepository<Company>(context);
                return companyRepository;
            }
        }

        public GenericRepository<Company_GroupPermission> Company_GroupPermissionRepository
        {
            get
            {
                this.company_GroupPermissionRepository = new GenericRepository<Company_GroupPermission>(context);
                return company_GroupPermissionRepository;
            }
        }

        public GenericRepository<Contact> ContactRepository
        {
            get
            {
                this.contactRepository = new GenericRepository<Contact>(context);
                return contactRepository;
            }
        }

        public GenericRepository<ChienDich> ChienDichRepository
        {
            get
            {
                this.chienDichRepository = new GenericRepository<ChienDich>(context);
                return chienDichRepository;
            }
        }
        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                this.departmentRepository = new GenericRepository<Department>(context);
                return departmentRepository;
            }
        }
        public GenericRepository<District> DistrictRepository
        {
            get
            {
                this.districtRepository = new GenericRepository<District>(context);
                return districtRepository;
            }
        }

        public GenericRepository<feedback> FeedbackRepository
        {
            get
            {
                this.feedbackRepository = new GenericRepository<feedback>(context);
                return feedbackRepository;
            }
        }

        public GenericRepository<GroupPermission> GroupPermissionRepository
        {
            get
            {
                this.groupPermissionRepository = new GenericRepository<GroupPermission>(context);
                return groupPermissionRepository;
            }
        }
        public GenericRepository<Groups> GroupsRepository
        {
            get
            {
                this.groupsRepository = new GenericRepository<Groups>(context);
                return groupsRepository;
            }
        }

        public GenericRepository<ICD10> ICD10Repository
        {
            get
            {
                this.iCD10Repository = new GenericRepository<ICD10>(context);
                return iCD10Repository;
            }
        }


        public GenericRepository<Khoa> KhoaRepository
        {
            get
            {
                this.khoaRepository = new GenericRepository<Khoa>(context);
                return khoaRepository;
            }
        }

        public GenericRepository<LichSuGuiChienDich> LichSuGuiChienDichRepository
        {
            get
            {
                this.lichSuGuiChienDichRepository = new GenericRepository<LichSuGuiChienDich>(context);
                return lichSuGuiChienDichRepository;
            }
        }

        public GenericRepository<Menu> MenuRepository
        {
            get
            {
                this.menuRepository = new GenericRepository<Menu>(context);
                return menuRepository;
            }
        }
        public GenericRepository<Menu_GroupPermission> Menu_GroupPermissionRepository
        {
            get
            {
                this.menu_GroupPermissionRepository = new GenericRepository<Menu_GroupPermission>(context);
                return menu_GroupPermissionRepository;
            }
        }
        public GenericRepository<NhomChienDich> NhomChienDichRepository
        {
            get
            {
                this.nhomChienDichRepository = new GenericRepository<NhomChienDich>(context);
                return nhomChienDichRepository;
            }
        }

        public GenericRepository<PatientGroup> PatientGroupRepository
        {
            get
            {
                this.patientGroupRepository = new GenericRepository<PatientGroup>(context);
                return patientGroupRepository;
            }
        }
        public GenericRepository<PackageService> PackageServiceRepository
        {
            get
            {
                this.packageServiceRepository = new GenericRepository<PackageService>(context);
                return packageServiceRepository;
            }
        }

        public GenericRepository<PatientGroup_Users> PatientGroup_UsersRepository
        {
            get
            {
                this.patientGroup_UsersRepository = new GenericRepository<PatientGroup_Users>(context);
                return patientGroup_UsersRepository;
            }
        }

        public GenericRepository<Notification> NotificationRepository
        {
            get
            {
                this.notificationRepository = new GenericRepository<Notification>(context);
                return notificationRepository;
            }
        }

        public GenericRepository<NewsCategory> NewsCategoryRepository
        {
            get
            {
                this.newsCategoryRepository = new GenericRepository<NewsCategory>(context);
                return newsCategoryRepository;
            }
        }

        public GenericRepository<NewsContent> NewsContentRepository
        {
            get
            {
                this.newsContentRepository = new GenericRepository<NewsContent>(context);
                return newsContentRepository;
            }
        }

        public GenericRepository<NewsContent_Tag> NewsContent_TagRepository
        {
            get
            {
                this.newsContent_TagRepository = new GenericRepository<NewsContent_Tag>(context);
                return newsContent_TagRepository;
            }
        }

        public GenericRepository<NhanTuVan> NhanTuVanRepository
        {
            get
            {
                this.nhanTuVanRepository = new GenericRepository<NhanTuVan>(context);
                return nhanTuVanRepository;
            }
        }

        public GenericRepository<NewsType> NewsTypeRepository
        {
            get
            {
                this.newsTypeRepository = new GenericRepository<NewsType>(context);
                return newsTypeRepository;
            }
        }

        public GenericRepository<Province> ProvinceRepository
        {
            get
            {
                this.provinceRepository = new GenericRepository<Province>(context);
                return provinceRepository;
            }
        }
        public GenericRepository<RegisterService> RegisterServiceRepository
        {
            get
            {
                this.registerServiceRepository = new GenericRepository<RegisterService>(context);
                return registerServiceRepository;
            }
        }

        public GenericRepository<resource> ResourceRepository
        {
            get
            {
                this.resourceRepository = new GenericRepository<resource>(context);
                return resourceRepository;
            }
        }

        public GenericRepository<Staff> StaffRepository
        {
            get
            {
                this.staffRepository = new GenericRepository<Staff>(context);
                return staffRepository;
            }
        }

        public GenericRepository<Tag> TagRepository
        {
            get
            {
                this.tagRepository = new GenericRepository<Tag>(context);
                return tagRepository;
            }
        }
        public GenericRepository<Service> ServiceRepository
        {
            get
            {
                this.serviceRepository = new GenericRepository<Service>(context);
                return serviceRepository;
            }
        }
        public GenericRepository<Service_Tag> Service_TagRepository
        {
            get
            {
                this.service_TagRepository = new GenericRepository<Service_Tag>(context);
                return service_TagRepository;
            }
        }
        public GenericRepository<SignedService> SignedServiceRepository
        {
            get
            {
                this.signedServiceRepository = new GenericRepository<SignedService>(context);
                return signedServiceRepository;
            }
        }
        public GenericRepository<UserInfo> UserInfoRepository
        {
            get
            {
                this.userInfoRepository = new GenericRepository<UserInfo>(context);
                return userInfoRepository;
            }
        }
        public GenericRepository<Users> UsersRepository
        {
            get
            {
                this.usersRepository = new GenericRepository<Users>(context);
                return usersRepository;
            }
        }
        public GenericRepository<Users_GroupPermission> Users_GroupPermissionRepository
        {
            get
            {
                this.users_GroupPermissionRepository = new GenericRepository<Users_GroupPermission>(context);
                return users_GroupPermissionRepository;
            }
        }
        public GenericRepository<Video> VideoRepository
        {
            get
            {
                this.videoRepository = new GenericRepository<Video>(context);
                return videoRepository;
            }
        }
        public GenericRepository<XML1> XML1Repository
        {
            get
            {
                this.xML1Repository = new GenericRepository<XML1>(context);
                return xML1Repository;
            }
        }
        public GenericRepository<XML2> XML2Repository
        {
            get
            {
                this.xML2Repository = new GenericRepository<XML2>(context);
                return xML2Repository;
            }
        }
        public GenericRepository<XML3> XML3Repository
        {
            get
            {
                this.xML3Repository = new GenericRepository<XML3>(context);
                return xML3Repository;
            }
        }
        public GenericRepository<XML4> XML4Repository
        {
            get
            {
                this.xML4Repository = new GenericRepository<XML4>(context);
                return xML4Repository;
            }
        }

        public ProjectDbContext DataContext
        {
            get
            {
                return context;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}