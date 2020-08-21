using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketManagement.Helpers
{
    public class SessionHandler
    {
        // ReSharper disable once InconsistentNaming
        private const string Portal_Token = "PortalToken";
        // ReSharper disable once InconsistentNaming
        private const string User_Id = "UserId";
        // ReSharper disable once InconsistentNaming
        private const string User_Name = "UserName";
        // ReSharper disable once InconsistentNaming
        private const string Mobile_No = "MobileNo";
        // ReSharper disable once InconsistentNaming
        private const string Role_Id = "RoleId";
        // ReSharper disable once InconsistentNaming
        private const string Role_Name = "RoleName";
        // ReSharper disable once InconsistentNaming
        private const string CacheProfile_Key = "CacheProfileKey";
        // ReSharper disable once InconsistentNaming
        private const string Email_Id = "EmailId";
        // ReSharper disable once InconsistentNaming
        private const string Is_FirstLogin = "IsFirstLogin";
        // ReSharper disable once InconsistentNaming
        private const string Admin_CategoryId = "AdminCategoryId";
        // ReSharper disable once InconsistentNaming
        private const string Hod_CategoryId = "HodCategoryId";

        public string PortalToken
        {
            get
            {
                return HttpContext.Current.Session[Portal_Token] == null ?
                    null : HttpContext.Current.Session[Portal_Token].ToString();
            }
            set
            {
                HttpContext.Current.Session[Portal_Token] = value;
            }
        }
        public string UserId
        {
            get
            {
                return HttpContext.Current.Session[User_Id] == null ?
                    null : HttpContext.Current.Session[User_Id].ToString();
            }
            set
            {
                HttpContext.Current.Session[User_Id] = value;
            }
        }
        public string UserName
        {
            get
            {
                return HttpContext.Current.Session[User_Name] == null ?
                    null : HttpContext.Current.Session[User_Name].ToString();
            }
            set
            {
                HttpContext.Current.Session[User_Name] = value;
            }
        }
        public string MobileNo
        {
            get
            {
                return HttpContext.Current.Session[Mobile_No] == null ?
                    null : HttpContext.Current.Session[Mobile_No].ToString();
            }
            set
            {
                HttpContext.Current.Session[Mobile_No] = value;
            }
        }
        public string RoleId
        {
            get
            {
                return HttpContext.Current.Session[Role_Id] == null ?
                    null : HttpContext.Current.Session[Role_Id].ToString();
            }
            set
            {
                HttpContext.Current.Session[Role_Id] = value;
            }
        }
        public string EmailId
        {
            get
            {
                return HttpContext.Current.Session[Email_Id] == null ?
                    null : HttpContext.Current.Session[Email_Id].ToString();
            }
            set
            {
                HttpContext.Current.Session[Email_Id] = value;
            }
        }
        public string IsFirstLogin
        {
            get
            {
                return HttpContext.Current.Session[Is_FirstLogin] == null ?
                    null : HttpContext.Current.Session[Is_FirstLogin].ToString();
            }
            set
            {
                HttpContext.Current.Session[Is_FirstLogin] = value;
            }
        }

        public string RoleName
        {
            get
            {
                return HttpContext.Current.Session[Role_Name] == null ?
                    null : HttpContext.Current.Session[Role_Name].ToString();
            }
            set
            {
                HttpContext.Current.Session[Role_Name] = value;
            }
        }

        public string CacheProfileKey
        {
            get
            {
                return HttpContext.Current.Session[CacheProfile_Key] == null ?
                    null : HttpContext.Current.Session[CacheProfile_Key].ToString();
            }
            set
            {
                HttpContext.Current.Session[CacheProfile_Key] = value;
            }
        }

        public string AgentAdminCategoryId
        {
            get
            {
                return HttpContext.Current.Session[Admin_CategoryId] == null ?
                    null : HttpContext.Current.Session[Admin_CategoryId].ToString();
            }
            set
            {
                HttpContext.Current.Session[Admin_CategoryId] = value;
            }
        }

        public string HodCategoryId
        {
            get
            {
                return HttpContext.Current.Session[Hod_CategoryId] == null ?
                    null : HttpContext.Current.Session[Hod_CategoryId].ToString();
            }
            set
            {
                HttpContext.Current.Session[Hod_CategoryId] = value;
            }
        }
    }
}