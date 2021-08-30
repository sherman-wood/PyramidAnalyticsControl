using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace PyramidAnalytics
{
    
    public class Tenant : PyramidAnalyticsElement
    {
        #region ENUMERATIONS
        public enum IdentifyingProperty { ID, Name }
        #endregion ENUMERATIONS

        #region ATTRIBUTES
        protected string Name;
        protected int ViewerSeats;
        protected int UsedViewerSeats;
        protected int ProSeats;
        protected int UsedProSeats;

        protected TenantSettings TenantSettings;
        protected string PulseKey;
        protected string SelectedUserDefaultsId;
        protected string SelectedUserDefaultsName;
        protected string DefaultThemeId;
        protected string DefaultAiServer;
        protected bool UserDefaultsOverridable;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public Tenant() : base(null)
        {
            this.Name = null;
            this.ViewerSeats = 0;
            this.UsedViewerSeats = 0;
            this.ProSeats = 0;
            this.UsedProSeats = 0;
            this.TenantSettings = null;
            this.PulseKey = null;
            this.SelectedUserDefaultsId = null;
            this.SelectedUserDefaultsName = null;
            this.DefaultThemeId = null;
            this.DefaultAiServer = null;
            this.UserDefaultsOverridable = false;
    }

        public Tenant(string id, string name, int viewerSeats, int usedViewerSeats, int proSeats, int usedProSeats,
            TenantSettings tenantSettings, string pulseKey, string selectedUserDefaultsId, string selectedUserDefaultsName,
            string defaultThemeId, string defaultAiServer, bool userDefaultsOverridable) : base(id)
        {
            this.Name = name;
            this.ViewerSeats = viewerSeats;
            this.UsedViewerSeats = usedViewerSeats;
            this.ProSeats = proSeats;
            this.UsedProSeats = usedProSeats;
            this.TenantSettings = tenantSettings;
            this.PulseKey = pulseKey;
            this.SelectedUserDefaultsId = selectedUserDefaultsId;
            this.SelectedUserDefaultsName = selectedUserDefaultsName;
            this.DefaultThemeId = defaultThemeId;
            this.DefaultAiServer = defaultAiServer;
            this.UserDefaultsOverridable = userDefaultsOverridable;
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string name { set { this.Name = value; } get { return this.Name; } }
        public int viewerSeats { set { this.ViewerSeats = value; } get { return this.ViewerSeats; } }
        public int usedViewerSeats { set { this.UsedViewerSeats = value; } get { return this.UsedViewerSeats; } }
        public int proSeats { set { this.ProSeats = value; } get { return this.ProSeats; } }
        public int usedProSeats { set { this.UsedProSeats = value; }get { return this.UsedProSeats; } }
        public TenantSettings tenantSettings { set { this.TenantSettings = value; } get { return this.TenantSettings; } }
        public string pulseKey { set { this.PulseKey = value; } get { return this.PulseKey; } }
        public string selectedUserDefaultsId { set { this.SelectedUserDefaultsId = value; } get { return this.SelectedUserDefaultsId; } }
        public string selectedUserDefaultsName { set { this.SelectedUserDefaultsName = value; } get { return this.SelectedUserDefaultsName; } }
        public string defaultThemeId { set { this.DefaultThemeId = value; } get { return this.DefaultThemeId; } }
        public string defaultAiServer { set { this.DefaultAiServer = value; } get { return this.DefaultAiServer; } }
        public bool userDefaultsOverridable { set { this.UserDefaultsOverridable = value; } get { return this.UserDefaultsOverridable; } }
        #endregion PROPERTIES

        #region METHODES
        public static List<Tenant> parseDataReturnMultipleTenants(string dataList)
        {
            if (!dataList.Contains("\"data\":"))
                return null;

            int dataListStart = dataList.IndexOf("\"data\":") + "\"data\":".Length;
            int dataListEnd = dataList.Length - 2;
            string dataListString = dataList.Substring(dataListStart, (dataListEnd - dataListStart + 1));

            return JsonSerializer.Deserialize<List<Tenant>>(dataListString);
        }

        public static Tenant parseDataReturnTenant(string data)
        {
            if (!data.Contains("\"data\":"))
                return null;
            
            int dataStart = data.IndexOf("\"data\":") + "\"data\":".Length;
            int dataEnd = data.Length - 2;
            string dataString = data.Substring(dataStart, (dataEnd - dataStart + 1));

            return JsonSerializer.Deserialize<Tenant>(dataString);
        }
        #endregion METHODES

        #region OVERRIDES
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion OVERRIDES
    }

    public class TenantSettings
    {
        #region ATTRIBUTES
        protected bool ShowGroupFolder;
        protected bool AllowWebhookChannels;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public TenantSettings()
        {
            this.ShowGroupFolder = false;
            this.AllowWebhookChannels = false;
        }

        public TenantSettings(bool showGroupFolder, bool allowWebhookChannels)
        {
            this.ShowGroupFolder = showGroupFolder;
            this.AllowWebhookChannels = allowWebhookChannels;
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public bool showGroupFolder { set { this.ShowGroupFolder = value; } get { return this.ShowGroupFolder; } }
        public bool allowWebhookChannels { set { this.AllowWebhookChannels = value; } get { return this.AllowWebhookChannels; } }
        #endregion PROPERTIES
    }
}