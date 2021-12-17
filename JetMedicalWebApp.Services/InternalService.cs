using System;
using System.Collections.Generic;
using System.Linq;
using JetMedicalWebApp.DAL;
using JetMedicalWebApp.Entities.Entity;
using JetMedicalWebApp.Entities.EntityDto;
using System.Linq.Dynamic;
using JetMedicalWebApp.Common;
using System.Web;

namespace JetMedicalWebApp.Services
{
    public class InternalService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public string GetUrlHost()
        {
            string host = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            return host;
        }
        public Users GetNguoiSuDungCapNhat(UnitOfWork unitOfWork)
        {
            int userID = -1;
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieUserID];
            if (authCookie != null)
            {
                userID = Int32.Parse(authCookie.Value);
            }


            return unitOfWork.UsersRepository.Get(m => m.UserID.Equals(userID)).FirstOrDefault();
        }

        public Dictionary<string, Dictionary<string, string>> SetThamSoChoFilter(Dictionary<string, string> parameters)
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            string stringFilter = string.Empty;

            if (parameters.ContainsKey(CommonConstants.StrStringFilter))
            {
                stringFilter += !string.IsNullOrEmpty(parameters[CommonConstants.StrStringFilter]) ? "(" + parameters[CommonConstants.StrStringFilter].Replace("\\", "\"") + ") " : string.Empty;
            }

            if (!string.IsNullOrEmpty(stringFilter))
            {
                filters.Add(CommonConstants.StrStringFilter, stringFilter);
            }

            if (parameters.ContainsKey(CommonConstants.StrTop))
            {
                inData.Add(CommonConstants.StrTake, parameters[CommonConstants.StrTop]);
            }

            if (parameters.ContainsKey(CommonConstants.StrSelectedFields))
            {
                inData.Add(CommonConstants.StrSelectedFields, parameters[CommonConstants.StrSelectedFields]);
            }

            if (parameters.ContainsKey(CommonConstants.StrSelectedFields))
            {
                inData.Add(CommonConstants.StrSortedColumnNames, parameters[CommonConstants.StrSortedColumnNames]);
            }

            result.Add(CommonConstants.StrFilters, filters);
            result.Add(CommonConstants.StrInData, inData);

            return result;
        }
        public Dictionary<string, Dictionary<string, string>> SetThamSoChoDatatable_GetList(int start, int length, Dictionary<string, string> parameters)
        {
            Dictionary<string, Dictionary<string, string>> result = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> filters = new Dictionary<string, string>();
            Dictionary<string, string> inData = new Dictionary<string, string>();
            Dictionary<string, string> requestParamters = new Dictionary<string, string>();

            int pageSize = length;

            //var sortColumn = System.Web.HttpContext.Current.Request.Params.GetValues("sort[field]") != null ? System.Web.HttpContext.Current.Request.Params.GetValues("sort[field]").FirstOrDefault() : string.Empty;
            //var sortColumnDir = System.Web.HttpContext.Current.Request.Params.GetValues("sort[sort]") != null ? System.Web.HttpContext.Current.Request.Params.GetValues("sort[sort]").FirstOrDefault() : string.Empty;

            string stringFilter = parameters.ContainsKey(CommonConstants.StrStringFilter) && !string.IsNullOrEmpty(parameters[CommonConstants.StrStringFilter]) ? "(" + parameters[CommonConstants.StrStringFilter].Replace("\\", "\"") + ") " : string.Empty;
            string sortCondition = parameters.ContainsKey(CommonConstants.StrSortedColumnNames) && !string.IsNullOrEmpty(parameters[CommonConstants.StrSortedColumnNames]) ? parameters[CommonConstants.StrSortedColumnNames] : "Id asc";

            requestParamters.Add(CommonConstants.StrPage, start.ToString());
            requestParamters.Add(CommonConstants.StrPageSize, pageSize.ToString());
            requestParamters.Add(CommonConstants.StrSortColumn, string.Empty);
            requestParamters.Add(CommonConstants.StrSortColumnDir, string.Empty);
            requestParamters.Add(CommonConstants.StrLength, length.ToString());

            inData.Add(CommonConstants.StrSelectedFields, parameters[CommonConstants.StrSelectedFields]);

            if (!string.IsNullOrEmpty(stringFilter))
            {
                filters.Add(CommonConstants.StrStringFilter, stringFilter);
            }

            if (!string.IsNullOrEmpty(pageSize.ToString()))
            {
                inData.Add(CommonConstants.StrSkip, start.ToString());
                inData.Add(CommonConstants.StrTake, pageSize.ToString());
            }

            inData.Add(CommonConstants.StrTotalRecords, "0");

            inData.Add(CommonConstants.StrSortedColumnNames, sortCondition);

            result.Add(CommonConstants.StrFilters, filters);
            result.Add(CommonConstants.StrInData, inData);
            result.Add(CommonConstants.StrRequestParamters, requestParamters);

            return result;
        }

        public object KhoiTaoModel(object model)
        {
            Menu_GroupPermissionDto menu_GroupPermission = null;
            string isAdminString = string.Empty;

            var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieUserID];
            if (authCookie != null)
            {
                model.GetType().GetProperty("UsersID").SetValue(model, Int32.Parse(authCookie.Value), null);
            }

            if (System.Web.HttpContext.Current.Request.Cookies[CommonConstants.CookieUserAdmin] != null)
            {
                isAdminString = System.Web.HttpContext.Current.Request.Cookies.Get(CommonConstants.CookieUserAdmin).Value;
            }


            if (isAdminString == "true")
            {
                menu_GroupPermission = new Menu_GroupPermissionDto()
                {
                    AllowUpdate = true,
                    AllowDelete = true,
                    SystemView = true,
                    PersonalView = true,
                    SystemDelete = true,
                    PersonalDelete = true,
                    SystemEdit = true,
                    PersonalEdit = true,
                    IsSystemUpdate_User = true,
                    IsSystemUpdate_XML1 = true
                };
                System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Menu_GroupPermission] = menu_GroupPermission;
            }
            else
            {
                menu_GroupPermission = (Menu_GroupPermissionDto)System.Web.HttpContext.Current.Session[CommonConstants.Session_CurrentUser_Menu_GroupPermission];
            }

            model.GetType().GetProperty("Menu_GroupPermission").SetValue(model, menu_GroupPermission, null);
         
            return model;
        }


        public object KhoiTaoMemberModel(object model)
        {
            var authCookie = System.Web.HttpContext.Current.Request.Cookies[Common.CommonConstants.CookieMemberID];
            if (authCookie != null)
            {
                model.GetType().GetProperty("UsersID").SetValue(model, Int32.Parse(authCookie.Value), null);
            }
            return model;
        }


        public string GenerateStringFilterTheoGroupPermission(Menu_GroupPermissionDto menu_GroupPermission)
        {
            string result = string.Empty;

            if (!HttpContext.Current.User.IsInRole(CommonConstants.AdminRole))
            {
                if (menu_GroupPermission.PersonalView == true)
                {
                    result = "UsernameNguoiCapNhat=\"" + HttpContext.Current.User.Identity.Name + "\"";
                }
            }

            return result;
        }

        public BaoCaoDto GetDuLieuBaoCaoTheoNam(int nam)
        {
            BaoCaoDto duLieu = new BaoCaoDto();
            UnitOfWork unitOfWork = new UnitOfWork();
            string sqlCommand = "exec usp_DuLieuBaoCao @nam";

            var param1 = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = "nam",
                Value = nam
            };

            duLieu = unitOfWork.DataContext.Database.SqlQuery<BaoCaoDto>(sqlCommand, param1).FirstOrDefault();

            return duLieu;
        }

        public List<DataThongKeDto> GetListDataThongKeDichVuFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                               out Dictionary<string, string> outData)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            List<DataThongKeDto> result = new List<DataThongKeDto>();
            string tableName = "uv_ThongKeDichVu";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData[CommonConstants.StrSelectedFields];
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "Id ASC";

            if (filters != null)
            {
                int intValue;
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "StringFilter":
                            if (!string.IsNullOrEmpty(stringFilter))
                            {
                                stringFilter += " AND ";
                            }

                            stringFilter += item.Value;

                            break;

                        default:
                            DataThongKeDto obj = new DataThongKeDto();

                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                if (!string.IsNullOrEmpty(stringFilter))
                                {
                                    stringFilter += " AND ";
                                }
                                stringFilter += item.Key + " like N'%" + item.Value + "%'";
                            }
                            else
                            {
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    if (!string.IsNullOrEmpty(stringFilter))
                                    {
                                        stringFilter += " AND ";
                                    }
                                    stringFilter += item.Key + " = " + item.Value;
                                }
                            }

                            break;
                    }
                }
            }

            var sqlCommand = "SELECT " + selectedFields + " FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);

            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<DataThongKeDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                DataThongKeDto obj = new DataThongKeDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                        case "CreatedDate":
                        case "ModifiedDate":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy") : string.Empty, null);
                            break;

                        default:
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null), null);
                            break;
                    }

                }
                result.Add(obj);
            }

            return result;
        }

        public List<DataThongKeDto> GetListDataThongKeNewsContentFromViewExposeDto(Dictionary<string, string> filters, Dictionary<string, string> inData,
                                                              out Dictionary<string, string> outData)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            List<DataThongKeDto> result = new List<DataThongKeDto>();
            string tableName = "uv_ThongKeNewsContent";
            string stringFilter = string.Empty;
            outData = new Dictionary<string, string>();

            string selectedFields = inData[CommonConstants.StrSelectedFields];
            string paging = "OFFSET " + (inData.ContainsKey("Skip") ? inData["Skip"] : "0") + " ROWS FETCH NEXT " +
                (inData.ContainsKey("Take") ? (inData["Take"] != "-1" ? inData["Take"] : "99999") : "99999999") + " ROWS ONLY";
            string sortedColumnNames = !string.IsNullOrEmpty(inData[CommonConstants.StrSortedColumnNames]) ? inData[CommonConstants.StrSortedColumnNames] : "Id ASC";

            if (filters != null)
            {
                int intValue;
                foreach (KeyValuePair<string, string> item in filters)
                {
                    switch (item.Key)
                    {
                        case "StringFilter":
                            if (!string.IsNullOrEmpty(stringFilter))
                            {
                                stringFilter += " AND ";
                            }

                            stringFilter += item.Value;

                            break;

                        default:
                            DataThongKeDto obj = new DataThongKeDto();

                            if (obj.GetType().GetProperty(item.Key).PropertyType == typeof(string))
                            {
                                if (!string.IsNullOrEmpty(stringFilter))
                                {
                                    stringFilter += " AND ";
                                }
                                stringFilter += item.Key + " like N'%" + item.Value + "%'";
                            }
                            else
                            {
                                if (Int32.TryParse(item.Value, out intValue))
                                {
                                    if (!string.IsNullOrEmpty(stringFilter))
                                    {
                                        stringFilter += " AND ";
                                    }
                                    stringFilter += item.Key + " = " + item.Value;
                                }
                            }

                            break;
                    }
                }
            }

            var sqlCommand = "SELECT " + selectedFields + " FROM " + tableName + " " + (!string.IsNullOrEmpty(stringFilter) ? " WHERE " + stringFilter : string.Empty);

            var dataCount = unitOfWork.DataContext.Database.SqlQuery<int>("SELECT COUNT(*) FROM (" + sqlCommand + ") AS tb").AsEnumerable();
            var displayedData = unitOfWork.DataContext.Database.SqlQuery<DataThongKeDto>(sqlCommand + (!string.IsNullOrEmpty(sortedColumnNames) ? " ORDER BY " + sortedColumnNames : "") + " " + paging).AsEnumerable();
            outData.Add("TotalRecords", dataCount.ToList()[0].ToString());

            foreach (dynamic item in displayedData)
            {
                DataThongKeDto obj = new DataThongKeDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        case "StrNgayTao":
                        case "StrNgayCapNhat":
                        case "CreatedDate":
                        case "ModifiedDate":
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null) != null ? itemPropertyInfo.GetValue(item, null).ToString("dd/MM/yyyy") : string.Empty, null);
                            break;

                        default:
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null), null);
                            break;
                    }

                }
                result.Add(obj);
            }

            return result;
        }

        public string kiemTraUnitMetaTitleTag(string metatitle, int id)
        {
            bool check = false;
            int tongMetatitle = 0;

            check = unitOfWork.TagRepository.Get(m => m.TagId.Trim() == metatitle.Trim() && m.Id != id).FirstOrDefault() != null;

            if (check)
            {
                tongMetatitle += unitOfWork.TagRepository.Get(m => m.TagId.Trim() == metatitle.Trim() && m.Id != id).Count();
            }

            return metatitle + (tongMetatitle > 0 ? ("-" + tongMetatitle.ToString()) : "");
        }

        public List<DataThongKeDto> GetListDataThongKeTagsFromViewExposeDto()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            List<DataThongKeDto> result = new List<DataThongKeDto>();
            string tableName = "uv_ThongKe20Tag";
   
            var sqlCommand = "SELECT * FROM " + tableName ;

            var displayedData = unitOfWork.DataContext.Database.SqlQuery<DataThongKeDto>(sqlCommand).AsEnumerable();
            foreach (dynamic item in displayedData)
            {
                DataThongKeDto obj = new DataThongKeDto();

                foreach (System.Reflection.PropertyInfo itemPropertyInfo in item.GetType().GetProperties())
                {
                    switch (itemPropertyInfo.Name)
                    {
                        default:
                            obj.GetType().GetProperty(itemPropertyInfo.Name).SetValue(obj, itemPropertyInfo.GetValue(item, null), null);
                            break;
                    }

                }
                result.Add(obj);
            }

            return result;
        }

    }
}
