using System;
using System.Collections.Generic;
using System.Text;

namespace PyramidAnalytics
{
    public class APIEndpoint
    {
        #region CONSTANT ENDPOINTS
        // A U T H E N T I C A T I O N
        public static string authUser = "auth/authenticateUser";
        public static string authUserByToken = "auth/authenticateUserByToken";
        public static string authUserEmbed = "auth/authenticateUserEmbed";
        public static string authUserEmbedByToken = "auth/authenticateUserEmbedByToken";
        public static string getMe = "access/getMe";
        
        // A C C E S S: Tenant
        public static string createTenant = "access/createTenant";
        public static string getAllTenants = "access/getAllTenants";
        public static string getTenantByName = "access/getTenantByName";
        public static string getDefaultTenant = "access/getDefaultTenant";
        public static string deleteTenants = "access/deleteTenants";
        public static string updateTenantSeats = "access/updateTenantSeats";


        // A C C E S S: Profile
        public static string addProfile = "access/addProfile";
        public static string getAllProfiles = "access/getAllProfiles";
        public static string getAllProfilesByTenant = "access/getAllProfilesByTenantId";
        public static string deleteProfile = "access/deleteProfile";

        // A C C E S S: Role
        public static string getAllRoles = "access/getAllRoles";
        public static string createRole = "access/createRole";
        public static string getRoleByName = "access/getRolesByName";
        public static string getRoleById = "access/getRoleById";
        public static string deleteRole = "access/deleteRole";

        // T A S K S
        public static string findSchedule = "tasks/findSchedule";
        public static string runSchedule = "tasks/runSchedule";
        public static string reRunTask = "tasks/reRunTask";
        public static string getExecutions = "tasks/getExecutions";
        public static string getTasks = "tasks/getTasks";
        #endregion CONSTANT ENDPOINTS
    }
}
