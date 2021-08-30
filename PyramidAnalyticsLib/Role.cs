using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PyramidAnalytics
{
    public class Role : PyramidAnalyticsElement
    {
        #region ENUMERATIONS        
        #endregion ENUMERATION

        #region ATTRIBUTES
        protected string Name;
        protected string TenantId;
        protected bool IsPrivate;
        protected bool IsHidden;
        protected string CreatedBy;
        protected bool IsGroupRole;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public Role() : base()
        {
            this.Name = null;
            this.TenantId = null;
            this.CreatedBy = null;
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string name { set { this.Name = value; } get { return this.Name; } }
        
        /// <summary>
        /// roleName is a duplicated Property as other API responses return "name"
        /// </summary>
        public string roleName { set { this.Name = value; } get { return this.Name; } } // Duplication due to inconsistant naming in REST API

        /// <summary>
        /// roleId is a duplicated Property as other API responses return "id"
        /// </summary>
        public string roleId { set { this.id = value; } get { return this.id; } } // Duplication due to inconsistant naming in REST API
        public string tenantId { set { this.TenantId = value; } get { return this.TenantId; } }
        public bool isPrivate { set { this.IsPrivate = value; } get { return this.IsPrivate; } }
        public bool isHidden { set { this.IsHidden = value; } get { return this.IsHidden; } }
        
        /// <summary>
        /// User Id of the account which created the role
        /// </summary>
        public string createdBy { set { this.CreatedBy = value; } get { return this.CreatedBy; } }
        
        /// <summary>
        /// There is or will be a Workgroup Folder for the role
        /// </summary>
        public bool isGroupRole { set { this.IsGroupRole = value; } get { return this.IsGroupRole; } }
        #endregion PROPERTIES

        #region METHODES
        #endregion METHODES

        #region OVERRIDES
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion OVERRIDES

        #region STATIC METHODES
        public static List<Role> parseRoles(string dataList)
        {
            if (!dataList.Contains("\"data\":"))
                return null;

            int dataListStart = dataList.IndexOf("\"data\":") + "\"data\":".Length;
            int dataListEnd = dataList.Length - 2;
            string dataListString = dataList.Substring(dataListStart, (dataListEnd - dataListStart + 1));

            return JsonSerializer.Deserialize<List<Role>>(dataListString);
        }

        public static Role parseRole(string data)
        {
            if (!data.Contains("\"data\":"))
                return null;

            int dataStart = data.IndexOf("\"data\":") + "\"data\":".Length;
            int dataEnd = data.Length - 2;
            string dataString = data.Substring(dataStart, (dataEnd - dataStart + 1));

            return JsonSerializer.Deserialize<Role>(dataString);
        }
        #endregion STATIC METHODES
    }
}
