#region IMPORTS
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
#endregion IMPORTS

namespace PyramidAnalytics
{
    public class PyramidAnayticsServer
    {
        #region ENUMERATIONS
        public enum SearchMatchType { contains = 0, notcontains = 1, equals = 2, startswith = 3, endswith = 4 }
        #endregion ENUMERATIONS

        #region CONSTANT ENDPOINTS
        private static string api2 = "/api2/";
        #endregion CONSTANT ENDPOINTS

        #region ATTRIBUTES
        protected string url;
        protected string UserName;
        protected string Password;
        protected string token;
        #endregion ATTRIBUTES

        #region CONSTRUCTORS
        public PyramidAnayticsServer()
        {
            this.UserName = null;
            this.Password = null;
            this.token = null;
        }

        /// <summary>
        /// Creates Pyramdi Analytics Server
        /// </summary>
        /// <param name="url">URL of the Pyramid Server</param>
        /// <param name="user">User name</param>
        /// <param name="password">Password</param>
        /// <param name="autoAuth">true will execute authenticate() in constructor and receive an authentication token on success</param>
        public PyramidAnayticsServer(string url, string user, string password, bool autoAuth = false)
        {
            this.url = url;
            this.UserName = user;
            this.Password = password;
            if (autoAuth) {
                this.token = authenticate();
                System.Console.WriteLine("auth token: " + this.token);
            }
        }
        #endregion CONSTRUCTORS

        #region PROPERTIES
        public string URL { set { this.url = value; } get { return this.url; } }
        
        public string userName { set { this.UserName = value; } get { return this.UserName; } }

        public string password { set { this.Password = value; } get { return this.Password; } }

        /// <summary>
        /// Returns authentication token from the Pyramid Server
        /// </summary>
        public string Token { get { return this.token; } }
        #endregion PROPERTIES

        #region METHODS
        /// <summary>
        /// takes no arguments. Utilizes the internal attributes url,user and password to try to connect.
        /// It will return the auth token on success. It will return null on failure
        /// </summary>
        /// <returns>Returns a secure authentication token</returns>
        private string authenticate()
        {
            if ((this.url == null)||(this.UserName == null)||(this.Password == null))
                return null;

            string data = "{'data':{'userName':'" + this.UserName + "','password':'" + this.Password + "'}}";
            return callAPI(APIEndpoint.authUser, data);
        }

        /// <summary>
        /// Login for embedding, returning token. Must pass domain of access.
        /// </summary>
        /// <returns>Returns a secure embedded authentication token</returns>
        public string authenticateEmbed(string domain)
        {
            if ((this.url == null)||(this.UserName == null)||(this.Password == null))
                return null;
            string data = "{'data':{" +
                "'userName':'" + this.UserName + "'," +
                "'password':'" + this.Password + "'," +
                "'domain':'" + domain + "'" +
                "}}";
            return callAPI(APIEndpoint.authUserEmbed, data);
        }

        /// <summary>
        /// Login as another user, returning token
        /// </summary>
        /// <returns>Returns a secure authentication token</returns>
        public string authenticateUserByToken( string userName )
        {
            string data = "{'data':{'userIdentity':'" + userName + "','token':'" + this.token + "'}}";
            return callAPI(APIEndpoint.authUserByToken, data);
        }

        /// <summary>
        /// Login as another user, returning token for embedding. Must pass domain of access.
        /// </summary>
        /// <returns>Returns a secure embedded authentication token</returns>
        public string authenticateUserEmbedByToken( string userName, string domain )
        {
            string embedToken = authenticateEmbed(domain);
            string data = "{'data':{'userIdentity':'" + userName + "','token':'" + embedToken + "'}}";
            return callAPI(APIEndpoint.authUserEmbedByToken, data);
        }

        /// <summary>
        /// creates a new Tenant. Calls API to create tenant. On success calls the API to retrieve all tenants and returns the tenant with the id returned by the "create" API call
        /// </summary>
        /// <param name="name">Name of the new Tenant</param>
        /// <param name="numOfViewer">Number of ViewerUser</param>
        /// <param name="numOfProUser">Number of ProUser</param>
        /// <returns>returns new Tenant on success. Returns null on failure e.g. Tenant name already exists.</returns>
        public Tenant createTenant(string name, int numOfViewer, int numOfProUser)
        {
            if (this.token == null)
                return null;

            string data = "{'tenant':{'name':'" + name + "','viewerSeats':" + numOfViewer + ",'proSeats':" + numOfProUser + "},'auth':'" + this.token + "'}";
            Result apiResult = new Result(callAPI(APIEndpoint.createTenant, data));
            if (!apiResult.Success)
                return null;

            foreach (Tenant t in getAllTenants())
            {
                if (t.id.Equals(apiResult.ModifiedList[0].id))
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// retrieves all tenants on the server
        /// </summary>
        /// <returns>returns a list of tenant objects</returns>
        public List<Tenant> getAllTenants()
        {
            string data = "{'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getAllTenants,data);

            return Tenant.parseDataReturnMultipleTenants(resultString);
        }

        /// <summary>
        /// retrieves a tenant object associated with a given name
        /// </summary>
        /// <param name="name">name of the tenant</param>
        /// <returns>tenant on success. Returns null on failure e.g. tenant does not exist</returns>
        public Tenant getTenantByName(string name)
        {
            string data = "{'tenantName':'" + name + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getTenantByName, data);
            
            return Tenant.parseDataReturnTenant(resultString);
        }

        /// <summary>
        /// retrieves the 'default' tenant
        /// </summary>
        /// <returns>retrieves the 'default' tenant on success. Returns null on failure e.g. invalid token</returns>
        public Tenant getDefaultTenant()
        {
            return getTenantByName("default");
        }

        /// <summary>
        /// Delete a tenant with a given tenant object
        /// </summary>
        /// <param name="propertyValue">Name or id of the tenant (id executes faster)</param>
        /// <param name="deleteUsers">drop users from system</param>
        /// <param name="deleteServers">drop tenants servers from system</param>
        /// <param name="identProp">switch for tenant name or tenant id (default id)</param>
        /// <returns>Returns a Result object</returns>
        public Result deleteTenant(string propertyValue, bool deleteUsers, bool deleteServers, Tenant.IdentifyingProperty identProp = Tenant.IdentifyingProperty.ID)
        {
            Tenant t;
            if(identProp == Tenant.IdentifyingProperty.Name)
            {
                t = getTenantByName(propertyValue);
                if (t == null)
                    return new Result(false,"No tenant with the name " + propertyValue);

                propertyValue = t.id;
            }
            string data = "{'data': {'tenantIds': ['" + propertyValue + "'],'deleteUsers':" + deleteUsers + ",'deleteServers':" + deleteServers + "},'auth':'" + this.token + "'}";
            
            return new Result(callAPI(APIEndpoint.deleteTenants,data));
        }

        /// <summary>
        /// Delete a tenant with a given tenant object
        /// </summary>
        /// <param name="tenant">Tenant object to identify the tenant</param>
        /// <param name="deleteUsers">drop users from system</param>
        /// <param name="deleteServers">drop tenants servers from system</param>
        /// <returns>Returns a Result object</returns>
        public Result deleteTenant(Tenant tenant, bool deleteUsers, bool deleteServers)
        {
            if (tenant == null)
                return new Result(false, "Tenant is null", new List<PyramidAnalyticsElement>());

            return deleteTenant(tenant.id, deleteUsers, deleteServers);
        }

        /// <summary>
        /// updates the number of seats for a certain tenant
        /// </summary>
        /// <param name="tenantProperty">Name or id of the tenant (id executes faster)</param>
        /// <param name="viewerSeats">Number of Viewer users</param>
        /// <param name="proSeats">Number of Pro Users</param>
        /// <param name="identifyingProperty">switch for tenant name or tenant id (default id)</param>
        /// <returns></returns>
        public string updateTenantSeats(string tenantProperty, int viewerSeats, int proSeats, Tenant.IdentifyingProperty identifyingProperty = Tenant.IdentifyingProperty.ID)
        {
            if (identifyingProperty == Tenant.IdentifyingProperty.Name)
                tenantProperty = getTenantByName(tenantProperty).id;
            string data = "{'data':{'id':'" + tenantProperty  + "','viewerSeats':" + viewerSeats + ",'proSeats':" + proSeats + "},'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.updateTenantSeats, data);
            return resultString;
        }

        /// <summary>
        /// A useful function to check if the current token at the server is still alive. *Can be executed by non-admin user
        /// </summary>
        /// <returns>The User that is currently logged on to the server associated with the token</returns>
        public User getMe()
        {
            string data = "{'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getMe, data);

            return User.parseDataReturnUser(resultString);
        }

        /// <summary>
        /// Adds a profile to the server
        /// </summary>
        /// <param name="name">Name of the new profile</param>
        /// <param name="description">Description of the profile</param>
        /// <param name="permissionIndexList">Hashset of permissions (doesn't check for inconsistency)</param>
        /// <param name="tenantProperty">Name or id of the tenant (id executes faster)</param>
        /// <param name="identifyingProperty">switch for tenant name or tenant id (default id)</param>
        /// <returns>On success returns the profile object it created. On failure returns null</returns>
        public Profile addProfile(string name, string description, HashSet<Permissions.PermissionSettings> permissionIndexList, string tenantProperty, Tenant.IdentifyingProperty identifyingProperty = Tenant.IdentifyingProperty.ID)
        {
            Tenant t;
            if (identifyingProperty == Tenant.IdentifyingProperty.Name)
            {
                t = getTenantByName(tenantProperty);
                if (t == null)
                    return null; // Log no tenant?

                tenantProperty = t.id;
            }
            string data = "{'profileApiData':{'name':'" + name + "','description':'" + description + "','permissions':{'permissionBitIndexList':" + JsonSerializer.Serialize(permissionIndexList) + "},'tenantId':'" + tenantProperty + "'},'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.addProfile, data);
            if(resultString.Contains("\"success\":true"))
            {
                Result result = new Result(resultString);
                return getProfile(result.ModifiedList[0].id);
            }
            return null;
        }

        /// <summary>
        /// Gets all Profiles by tenant. Is currently filtering all profiles by tenantId from the return of the /access/getProfilesByTenant REST API call. This is redundant once the API bug that returns all profiles is fixed.
        /// </summary>
        /// <param name="tenantProperty">Either tenantId or tenantName (id executes faster)</param>
        /// <param name="identifyingProperty">Switch if the property is an id or a name. The default is id</param>
        /// <returns>List of profiles attached to tenant</returns>
        public List<Profile> getProfilesByTenant(string tenantProperty, Tenant.IdentifyingProperty identifyingProperty = Tenant.IdentifyingProperty.ID)
        {
            if(identifyingProperty == Tenant.IdentifyingProperty.Name)
            {
                tenantProperty = getTenantByName(tenantProperty).id;
            }
            string data = "{'tenantId':'" + tenantProperty + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getAllProfilesByTenant, data);
            List<Profile> profiles = new List<Profile>();
            foreach (Profile p in Profile.parseProfiles(resultString))
                if (p.tenantId.Equals(tenantProperty))
                    profiles.Add(p);
            return profiles;
        }

        /// <summary>
        /// Gets all profiles configured at the server
        /// </summary>
        /// <returns>List of all profiles on the server</returns>
        public List<Profile> getAllProfiles()
        {
            string data = "{'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getAllProfiles, data);
            return Profile.parseProfiles(resultString);
        }
        
        /// <summary>
        /// Get a single profile by name or id. Returns null if a profile isn't found
        /// </summary>
        /// <param name="ProfileProperty">Either name or id to identify a profile (id executes faster)</param>
        /// <param name="identifyingProperty">Switch between name or id</param>
        /// <returns>Profile by name or id, if not found returns null</returns>
        public Profile getProfile(string ProfileProperty, Profile.IdentifyingProperty identifyingProperty = Profile.IdentifyingProperty.ID)
        {
            foreach(Profile p in getAllProfiles())
            {
                if (identifyingProperty == Profile.IdentifyingProperty.ID)
                    if (p.profileId.Equals(ProfileProperty))
                        return p;
                if (p.name.Equals(ProfileProperty))
                    return p;
            }
            return new Profile();
        }

        /// <summary>
        /// Deletes a profile from a server
        /// </summary>
        /// <param name="ProfileProperty">Either name or id to identify a profile (id executes faster)</param>
        /// <param name="identifyingProperty">Switch between name or id</param>
        /// <returns>Result object</returns>
        public Result deleteProfile(string ProfileProperty, Profile.IdentifyingProperty identifyingProperty = Profile.IdentifyingProperty.ID)
        {
            string id = ProfileProperty;
            if (identifyingProperty == Profile.IdentifyingProperty.Name)
                id = getProfile(ProfileProperty, Profile.IdentifyingProperty.Name).profileId;
            string data = "{'profileId':'" + id + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.deleteProfile, data);
            return new Result(resultString);
        }

        /// <summary>
        /// List all roles on the server. There is no tenant or profile information attached to the result.
        /// </summary>
        /// <returns>Returns list of role objects</returns>
        public List<Role> getAllRoles()
        {
            /***
             * The follwing code is standard API. Since the API returns more information using getRoleByName using wildcards to get all roles
             * 
             * 
            string data = "{'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getAllRoles, data);
            return Role.parseRoles(resultString);
            */
            return getRoleByName("", SearchMatchType.startswith);
        }

        /// <summary>
        /// Creates role. If role allready exists the API creates role anyway and appends '(1)' to the name
        /// </summary>
        /// <param name="name">Name of new role</param>
        /// <param name="isHidden">sets role hidden</param>
        /// <param name="isGroupRole">creates workgroup folder for role members</param>
        /// <param name="tenantProperty">Either name or id to identify a tenant (id executes faster)</param>
        /// <param name="identifyingProperty">Switch between name or id</param>
        /// <returns>Result object</returns>
        public Result createRole(string name, bool isHidden, bool isGroupRole, string tenantProperty, Tenant.IdentifyingProperty identifyingProperty = Tenant.IdentifyingProperty.ID)
        {
            if (identifyingProperty == Tenant.IdentifyingProperty.Name)
                tenantProperty = getTenantByName(tenantProperty).id;

            string data = "{'data':{'roleName':'" + name + "','isGroupRole':'" + isGroupRole + "','isHidden':" + isHidden + ",'tenantId':'" + tenantProperty + "'},'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.createRole, data);
            if (resultString != null)
                return new Result(resultString);

            return new Result(false,"API Call returned 'null'",new List<PyramidAnalyticsElement>());
        }
        
        /// <summary>
        /// Returns list of roles matching the search criteria. Duplicates in Attributes (e.g. name&roleName) due to inconsistent REST API responses
        /// </summary>
        /// <param name="name">string can be a partial match if it matches the searchtype</param>
        /// <param name="searchMatchType">Options are equals(default), contains, not contains, startswith, endswith</param>
        /// <returns>list of roles matching the search criteria</returns>
        public List<Role> getRoleByName(string name, SearchMatchType searchMatchType = SearchMatchType.equals)
        {
            string data = "{'data':{'searchValue':'" + name + "','searchMatchType':" + (int)searchMatchType + "},'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getRoleByName, data);
            return Role.parseRoles(resultString);
        }

        /// <summary>
        /// Returns the most complete information about a role
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>returns Role object</returns>
        public Role getRoleById(string roleId)
        {
            string data = "{'roleId':'" + roleId + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getRoleById, data);
            return Role.parseRole(resultString);
        }

        /// <summary>
        /// Deletes role associated with the given id
        /// </summary>
        /// <param name="roleId">Role id</param>
        /// <returns>returns Result object</returns>
        public Result deleteRole(string roleId)
        {
            string data = "{'roleId':'" + roleId + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.deleteRole, data);
            return new Result(resultString);
        }

        /// <summary>
        /// Find a schedule by name
        /// </summary>
        /// <param name="searchText">Searchtext for schedule name</param>
        /// <param name="searchMatchType">Search type e.g. startswidth default equals</param>
        /// <param name="scheduleType">Schedule type e.g. printing default etl</param>
        /// <returns>List of schedules mathcing the search criteria</returns>
        public List<Schedule> findSchedule(string searchText, SearchMatchType searchMatchType = SearchMatchType.equals, Schedule.ScheduleType scheduleType = Schedule.ScheduleType.etl)
        {
            string data = "{'searchCriteria':{'searchCriteria':{'searchValue':'" + searchText + "', 'searchMatchType':" + (int)searchMatchType + "},'scheduleType':" + (int)scheduleType + "},'auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.findSchedule, data);
            return Schedule.parseSchedules(resultString);
        }

        /// <summary>
        /// Get all tasks associated with an ExecutionId
        /// </summary>
        /// <param name="executionId">ExecutionId can be sourced from getExecutions(...)</param>
        /// <returns>Returns List of tasks</returns>
        public List<Task> getTasks(string executionId)
        {
            string data = "{'executionId':'" + executionId + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getTasks, data);
            return Task.parseTasks(resultString);
        }

        /// <summary>
        /// Get all executions by scheduleId
        /// </summary>
        /// <param name="scheduleId">ScheduleId can be sourced from findSchedule(...)</param>
        /// <returns>List of executions</returns>
        public List<Execution> getExecutions(string scheduleId)
        {
            string data = "{'scheduleId':'" + scheduleId + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.getExecutions, data);
            return Execution.parseExecutions(resultString);
        }

        /// <summary>
        /// Execute tasks e.g. model data flows
        /// </summary>
        /// <param name="taskId">taskId can get sourced from getTasks(...)</param>
        /// <returns>Returns result object with taskId in the modifiedList[]</returns>
        public Result reRunTask(string taskId)
        {
            string data = "{'taskId':'" + taskId + "','auth':'" + this.token + "'}";
            string resultString = callAPI(APIEndpoint.reRunTask, data);
            return new Result(resultString);
        }

        // TO BE DOCUMENTED BELOW


        // TO BE FULLY IMPLEMENTED BELOW
        public string runSchedule(string scheduleId, bool checkTriggers)
        {
            string ct = "false";
            if (checkTriggers)
                ct = "true";

            string data = "{'data':{'scheduleId':'" + scheduleId + "','checkTriggers':" + ct + "},'auth':'" + this.token + "'}";
            System.Console.WriteLine("+++++++++++++++++++");
            System.Console.WriteLine(data);
            System.Console.WriteLine("+++++++++++++++++++");
            string resultString = callAPI(APIEndpoint.runSchedule, data);
            return resultString;
        }
        public string getAllNonPrivateRoles() { return "get all non private roles is not implemented yet"; }
        public string getAllNonPrivateRolesByTenant() { return "get all non private roles all non private roles by tenant is not implemented yet"; }
        public string updateRolesByProfile() { return "update roles by profile is not implemented yet"; }
        public string getAllRolesByProfile() { return "get all roles by profile is not implemented yet"; }
        public string getAllRolesByUser() { return "get all roles by user is not implemented yet"; }
        public string getAllGroupsByRole() { return "get all groups by role is not implemented yet"; }
        public string updateUserRoles() { return "update user roles is not implemented yet"; }


        
        // DO NOT TOUCH CODE BELOW FULLY TESTED AND IMPLEMENTED ACROSS THE BOARD CHANGES WILL CAUSE HEAVY REFACTORING
        /// <summary>
        /// handles the REST POST call
        /// </summary>
        /// <param name="command">Please source from APIEndpoint static constants</param>
        /// <param name="data">provide a json object the REST API needs</param>
        /// <returns>Returns a string on a successfull API call</returns>
        private string callAPI(string command, string data)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(data);
            HttpResponseMessage result = client.PostAsync(this.url + PyramidAnayticsServer.api2 + command, content).Result;
            if (result.IsSuccessStatusCode)
            {
                Task<String> res = result.Content.ReadAsStringAsync();
                res.Wait();
                return res.Result;
            } else {
                throw new Exception("callAPI - status fail: " + result.StatusCode);
            }
            // return null;
        }
        #endregion METHODS
    }
}
