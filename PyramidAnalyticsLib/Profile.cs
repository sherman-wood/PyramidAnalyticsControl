using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PyramidAnalytics
{
    public class Profile
    {
        #region ENUMERATIONS
        public enum IdentifyingProperty { ID, Name }
        #endregion ENUMERATIONS

        #region ATTRIBUTES
        protected string ProfileID;
        protected string Name;
        protected string Description;
        protected Permissions Permissions;
        protected long CreatedDate;
        protected long ModifiedDate;
        protected string CreatedBy;
        protected string LastModifiedBy;
        protected string TenantID;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public Profile()
        {
            this.ProfileID = null;
            this.Name = null;
            this.Description = null;
            this.Permissions = new Permissions();
            this.CreatedDate = 0;
            this.ModifiedDate = 0;
            this.CreatedBy = null;
            this.LastModifiedBy = null;
            this.TenantID = null;
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string profileId { set { this.ProfileID = value; } get { return this.ProfileID; } }
        public string name { set { this.Name = value; } get { return this.Name; } }
        public string description { set { this.Description = value; } get { return this.Description; } }
        public Permissions permissions { set { this.Permissions = value; } get { return this.Permissions; } }
        public long createdDate { set { this.CreatedDate = value; } get { return this.CreatedDate; } }
        public long modifiedDate { set { this.ModifiedDate = value; } get { return this.ModifiedDate; } }
        public string createdBy { set { this.CreatedBy = value; } get { return this.CreatedBy; } }
        public string lastModifiedBy { set { this.LastModifiedBy = value; } get { return this.LastModifiedBy; } }
        public string tenantId { set { this.TenantID = value; } get { return this.TenantID; } }
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
        public static List<Profile> parseProfiles(string dataList)
        {
            if (!dataList.Contains("\"data\":"))
                return null;

            int dataListStart = dataList.IndexOf("\"data\":") + "\"data\":".Length;
            int dataListEnd = dataList.Length - 2;
            string dataListString = dataList.Substring(dataListStart, (dataListEnd - dataListStart + 1));
            return JsonSerializer.Deserialize<List<Profile>>(dataListString);
        }
        #endregion STATIC METHODES
    }

    public class Permissions
    {
        #region ENUMERATIONS
        public enum PermissionSettings
        {
            // Discover_On1 does set the checkbox in the App, but doesn't set the additional toggle for Discover_On2 might lead to bugs
            // Formulate_On1 does set the checkbox in the App, but doesn't set the additional toggle for Formulate_On2 might lead to bugs
            Illustrate_On = 0, Present_On = 1, Model_On = 2, Discover_On1 = 3 /* 14 is On too?*/, Formulate_On1 = 4 /* 12 is On too?*/, 
            Publish_On = 5, Add_Data_Server = 6 /* needs 2 On as well*/, Use_Machine_Learning = 7 /* needs 2 On as well*/, Create_Script_R_Python_Model = 8 /* needs 2 On as well*/, Advanced_Analytics = 9 /* needs 3+14 On as well; Exclusive to 22*/,
            Can_Create_KPI = 10 /* needs 4+12 On as well*/, Create_Script_R_Python_Formulate = 11 /* needs 4+12 On as well*/, Formulate_On2 = 12 /* 4 is On too?*/, Select_Multiple_Items = 13 /* needs 5 On as well*/, Discover_On2 = 14 /* 3 is On too?*/,
            Custom_Visual = 15 /* needs 4+12 On as well*/, Create_Custom_SQL_Scripts = 16 /* needs 2 On as well*/, Advanced_Options = 17, Save_Shareable_Content = 18 /* needs 3+14 On as well; Exclusive to 22*/, Smart_Publish = 19 /* needs 5 On as well*/,
            Scheduling = 20 /* needs 5 On as well*/, Smart_Present = 21 /* needs 1 On as well*/, Discover_Lite = 22 /* needs 3+14 On as well; Exclusive to 9 and 18*/, Present_Lite = 23 /* needs 1 On as well*/, Execute_Process = 24 /* needs 2 On as well*/
        }
        #endregion ENUMERATIONS

        #region ATTRIBUTES
        protected long Numeric;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public Permissions()
        {
            this.numeric = 0;
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public long numeric { set { this.Numeric = value; } get { return this.Numeric; } }
        public string permissionBitmap
        {
            get
            {
                char[] pbm = Convert.ToString(this.Numeric, 2).ToCharArray();
                Array.Reverse(pbm);
                return new string(pbm);
            }
        }

        public bool Illustrate_On { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Illustrate_On) { return this.permissionBitmap[(int)PermissionSettings.Illustrate_On]=='1'; } else { return false; } } }
        public bool Present_On { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Present_On) { return this.permissionBitmap[(int)PermissionSettings.Present_On] == '1'; } else { return false; } } }
        public bool Model_On { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Model_On) { return this.permissionBitmap[(int)PermissionSettings.Model_On] == '1'; } else { return false; } } }
        public bool Discover_On1 { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Discover_On1) { return this.permissionBitmap[(int)PermissionSettings.Discover_On1] == '1'; } else { return false; } } }
        public bool Formulate_On1 { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Formulate_On1) { return this.permissionBitmap[(int)PermissionSettings.Formulate_On1] == '1'; } else { return false; } } }
        public bool Publish_On { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Publish_On) { return this.permissionBitmap[(int)PermissionSettings.Publish_On] == '1'; } else { return false; } } }
        public bool Add_Data_Server { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Add_Data_Server) { return this.permissionBitmap[(int)PermissionSettings.Add_Data_Server] == '1'; } else { return false; } } }
        public bool Use_Machine_Learning { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Use_Machine_Learning) { return this.permissionBitmap[(int)PermissionSettings.Use_Machine_Learning] == '1'; } else { return false; } } }
        public bool Create_Script_R_Python_Model { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Create_Script_R_Python_Model) { return this.permissionBitmap[(int)PermissionSettings.Create_Script_R_Python_Model] == '1'; } else { return false; } } }
        public bool Advanced_Analytics { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Advanced_Analytics) { return this.permissionBitmap[(int)PermissionSettings.Advanced_Analytics] == '1'; } else { return false; } } }
        public bool Can_Create_KPI { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Can_Create_KPI) { return this.permissionBitmap[(int)PermissionSettings.Can_Create_KPI] == '1'; } else { return false; } } }
        public bool Create_Script_R_Python_Formulate { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Create_Script_R_Python_Formulate) { return this.permissionBitmap[(int)PermissionSettings.Create_Script_R_Python_Formulate] == '1'; } else { return false; } } }
        public bool Formulate_On2 { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Formulate_On2) { return this.permissionBitmap[(int)PermissionSettings.Formulate_On2] == '1'; } else { return false; } } }
        public bool Select_Multiple_Items { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Select_Multiple_Items) { return this.permissionBitmap[(int)PermissionSettings.Select_Multiple_Items] == '1'; } else { return false; } } }
        public bool Discover_On2 { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Discover_On2) { return this.permissionBitmap[(int)PermissionSettings.Discover_On2] == '1'; } else { return false; } } }
        public bool Custom_Visual { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Custom_Visual) { return this.permissionBitmap[(int)PermissionSettings.Custom_Visual] == '1'; } else { return false; } } }
        public bool Create_Custom_SQL_Scripts { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Create_Custom_SQL_Scripts) { return this.permissionBitmap[(int)PermissionSettings.Create_Custom_SQL_Scripts] == '1'; } else { return false; } } }
        public bool Advanced_Options { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Advanced_Options) { return this.permissionBitmap[(int)PermissionSettings.Advanced_Options] == '1'; } else { return false; } } }
        public bool Save_Shareable_Content { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Save_Shareable_Content) { return this.permissionBitmap[(int)PermissionSettings.Save_Shareable_Content] == '1'; } else { return false; } } }
        public bool Smart_Publish { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Smart_Publish) { return this.permissionBitmap[(int)PermissionSettings.Smart_Publish] == '1'; } else { return false; } } }
        public bool Scheduling { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Scheduling) { return this.permissionBitmap[(int)PermissionSettings.Scheduling] == '1'; } else { return false; } } }
        public bool Smart_Present { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Smart_Present) { return this.permissionBitmap[(int)PermissionSettings.Smart_Present] == '1'; } else { return false; } } }
        public bool Discover_Lite { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Discover_Lite) { return this.permissionBitmap[(int)PermissionSettings.Discover_Lite] == '1'; } else { return false; } } }
        public bool Present_Lite { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Present_Lite) { return this.permissionBitmap[(int)PermissionSettings.Present_Lite] == '1'; } else { return false; } } }
        public bool Execute_Process { get { if (this.permissionBitmap.Length > (int)PermissionSettings.Execute_Process) { return this.permissionBitmap[(int)PermissionSettings.Execute_Process] == '1'; } else { return false; } } }
        //PROPERTIES GET FOR ALL CHECK BOXES
        #endregion PROPERTIES

        #region METHODES
        #endregion METHODES

        #region OVERRIDES
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion OVERRIDES
    }
}