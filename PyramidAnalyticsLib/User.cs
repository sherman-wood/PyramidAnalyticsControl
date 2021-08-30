using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PyramidAnalytics
{
    public class User : PyramidAnalyticsElement
    {
        #region ENUMERATIONS
        public enum ADMINTYPE { none = 0, domainadmin = 1, enterpriseadmin = 2}
        public enum CLIENTLICENCETYPE { none = 0, viewer = 100, professional = 200 }
        public enum USERSTATUSID { disabled = 0, enabled = 1}
        #endregion ENUMERATIONS

        #region ATTRIBUTES
        protected string UserName;
        protected string FirstName;
        protected string LastName;
        protected string EMail;
        protected string Phone;
        protected string ProxyAccount;
        protected ADMINTYPE AdminType;
        protected List<string> RoleIds;
        protected CLIENTLICENCETYPE ClientLicenceType;
        protected USERSTATUSID StatusID;
        protected string TenantId;
        protected long CreatedDate;
        protected long LastLoginDate;
        protected string ADDomainName;
        protected string PrincipalName;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public User() : base()
        {
            this.UserName = null;
            this.FirstName = null;
            this.LastName = null;
            this.EMail = null;
            this.Phone = null;
            this.ProxyAccount = null;
            this.AdminType = ADMINTYPE.none;
            this.RoleIds = new List<string>();
            this.ClientLicenceType = CLIENTLICENCETYPE.none;
            this.StatusID = USERSTATUSID.disabled;
            this.TenantId = null;
            this.CreatedDate = 0;
            this.LastLoginDate = 0;
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string userName { set { this.UserName = value; } get { return this.UserName; } }
        public string firstName { set { this.FirstName = value; } get { return this.FirstName; } }
        public string lastName { set { this.LastName = value; } get { return this.LastName; } }
        public string email { set { this.EMail = value; } get { return this.EMail; } }
        public string phone { set { this.Phone = value; } get { return this.Phone; } }
        public string proxyAccount { set { this.ProxyAccount = value; } get { return this.ProxyAccount; } }
        public ADMINTYPE adminType { set { this.AdminType = value; } get { return this.AdminType; } }
        public List<string> roleIds { set { this.RoleIds = value; } get { return this.RoleIds; } }
        public CLIENTLICENCETYPE clientLicenceType { set { this.ClientLicenceType = value; } get { return this.ClientLicenceType; } }
        public USERSTATUSID statusID { set { this.StatusID = value; } get { return this.StatusID; } }
        public string tenantId { set { this.TenantId = value; } get { return this.TenantId; } }
        public long createdDate { set { this.CreatedDate = value; } get { return this.CreatedDate; } }
        public long lastLoginDate { set { this.LastLoginDate = value; } get { return this.LastLoginDate; } }
        #endregion PROPERTIES

        #region METHODES
        public static User parseDataReturnUser(string data)
        {
            if (data == null)
                return null;

            if (!data.Contains("\"data\":"))
                return null;

            data = data.Replace("\"inheritanceType\":\"PyramidViewUserObject\",", "");
            int dataStart = data.IndexOf("\"data\":") + "\"data\":".Length;
            int dataEnd = data.Length - 2;
            string dataString = data.Substring(dataStart, (dataEnd - dataStart + 1));

            return JsonSerializer.Deserialize<User>(dataString);
        }
        #endregion METHODES

        #region OVERRIDES
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion OVERRIDES
    }
}
