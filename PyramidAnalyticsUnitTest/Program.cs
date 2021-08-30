using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using PyramidAnalytics;

namespace PyramidAnalyticsUnitTest
{
    class Program
    {
        private static Credentials credentials;
        static void Main(string[] args)
        {
            credentials = loadCredentials(args[0]);
            System.Console.WriteLine("Accessing Pyramid at: " + credentials.Url);
            System.Console.WriteLine("Logged in as user: " + credentials.User);
            PyramidAnayticsServer p = new PyramidAnayticsServer(credentials.Url, credentials.User, credentials.Password, true);
            System.Console.WriteLine(p.getMe().ToString());
            testByTokenAndEmbed(p);
        }

        private static Credentials loadCredentials(string path)
        {
            // StreamReader sr = new StreamReader(path);
            // string line;
            // while((line = sr.ReadLine())!=null)
            // {
            try
            {
                string jsonString = File.ReadAllText(path);
                Credentials c = JsonSerializer.Deserialize<Credentials>(jsonString);
                if (c != null)
                    return c;
            }
            catch (Exception ex) {
                System.Console.WriteLine("Cannot parse credentials json.");
                System.Console.WriteLine(ex);
            }
            // }
            return null;
        }

        private static void testByTokenAndEmbed(PyramidAnayticsServer p)
        {
            string loginAsUser = "viewer";
            string embedDomain = "localhost";
            Console.WriteLine("PyramidAnalytics testByTokenAndEmbed!");
            // PyramidAnayticsServer p = new PyramidAnayticsServer(url, username, password, true);
            System.Console.WriteLine("----");
            System.Console.WriteLine("authenticateEmbed as: " + credentials.User);
            System.Console.WriteLine(p.authenticateEmbed(embedDomain));
            System.Console.WriteLine("authenticateByToken as: " + loginAsUser);
            System.Console.WriteLine(p.authenticateUserByToken(loginAsUser));
            System.Console.WriteLine("authenticateEmbedByToken as: " + loginAsUser);
            System.Console.WriteLine(p.authenticateUserEmbedByToken(loginAsUser, embedDomain));
        }

        private static void testTenantAndRoles(PyramidAnayticsServer p)
        {
            Console.WriteLine("PyramidAnalytics testTenantAndRoles!");
            // PyramidAnayticsServer p = new PyramidAnayticsServer(url, username, password, true);
            System.Console.WriteLine("----");
            
            Tenant t = p.createTenant("AAAPI_Test_Tenant", 1, 1);
            if (t != null)
                System.Console.WriteLine(t.ToString());
            t = p.getTenantByName("FAF");
            if (t != null)
                System.Console.WriteLine(t.ToString());
            
            t = p.getDefaultTenant();
            if (t != null)
                System.Console.WriteLine(t.ToString());
            t = p.getTenantByName("AAAPI_Test_Tenant");
            System.Console.WriteLine(p.deleteTenant(t, true, true).ToString());
            
            System.Console.WriteLine(p.getMe().ToString());
            
            t = p.createTenant("AAAPI_Test_Tenant", 1, 1);
            System.Console.WriteLine("----");
            
            HashSet<Permissions.PermissionSettings> permissions = new HashSet<Permissions.PermissionSettings>();
            permissions.Add(Permissions.PermissionSettings.Advanced_Analytics);
            permissions.Add(Permissions.PermissionSettings.Discover_On1);
            permissions.Add(Permissions.PermissionSettings.Discover_On2);
            System.Console.WriteLine(p.addProfile("APITestProfile", "Whatever should be described", permissions, "AAAPI_Test_Tenant", Tenant.IdentifyingProperty.Name));
            
            List<Profile> profiles = p.getAllProfiles();
            if (profiles != null)
                foreach (Profile prof in profiles)
                    Console.WriteLine(prof.name + "|" + prof.permissions.permissionBitmap.Length + "|" + prof.permissions.ToString());
            System.Console.WriteLine();
            System.Console.WriteLine("----");
            System.Console.WriteLine(p.deleteTenant("AAAPI_Test_Tenant", true, true, Tenant.IdentifyingProperty.Name));
            System.Console.WriteLine("----");
            System.Console.WriteLine("Done!");
            
            permissions = new HashSet<Permissions.PermissionSettings>();
            permissions.Add(Permissions.PermissionSettings.Scheduling);
            permissions.Add(Permissions.PermissionSettings.Publish_On);
            permissions.Add(Permissions.PermissionSettings.Present_On);
            Profile pf = p.addProfile("APITestProfile", "Whatever should be described", permissions, "default", Tenant.IdentifyingProperty.Name);
            if (pf != null)
                System.Console.WriteLine(pf.ToString());
            
            profiles = p.getProfilesByTenant("WILLIAM REED", Tenant.IdentifyingProperty.Name);
            foreach (Profile prof in profiles)
                System.Console.WriteLine(prof.name + "\r\n" + prof.ToString());

            permissions = new HashSet<Permissions.PermissionSettings>();
            permissions.Add(Permissions.PermissionSettings.Scheduling);
            permissions.Add(Permissions.PermissionSettings.Publish_On);
            permissions.Add(Permissions.PermissionSettings.Present_On);

            pf = p.addProfile("APITestProfile", "Whatever should be described", permissions, "default", Tenant.IdentifyingProperty.Name);
            System.Console.WriteLine(p.deleteProfile("APITestProfile", Profile.IdentifyingProperty.Name).ToString());

            t = p.createTenant("AAAPI_Test_Tenant", 1, 1);
            System.Console.WriteLine(p.updateTenantSeats("AAAPI_Test_Tenant", 25, 25));
            p.deleteTenant(t, true, true);

            // p = new PyramidAnayticsServer(url, username, password, true);
            List<Role> roles = p.getAllRoles();
            foreach (Role r in roles)
                System.Console.WriteLine(r.name + "\r\n" + r.ToString());

            roles = p.getRoleByName("A", PyramidAnayticsServer.SearchMatchType.contains);
            foreach (Role r in roles)
                System.Console.WriteLine(r.name + "\r\n" + r.ToString());

            //741dae43-ec8e-42c7-baba-5dc8922ee5fd
            System.Console.WriteLine(p.getRoleById("741dae43-ec8e-42c7-baba-5dc8922ee5fd").ToString());

            t = p.createTenant("AAAPI_Test_Tenant", 1, 1);

            Result res = p.createRole("AAATestRole", true, false, "AAAPI_Test_Tenant", Tenant.IdentifyingProperty.Name);
            System.Console.WriteLine("Create Role:\r\n" + res.ToString());
            if (res.Success)
                System.Console.WriteLine(p.deleteRole(res.ModifiedList[0].id));
            System.Console.WriteLine(p.deleteRole("dsfsfdsd"));
        }
    }

    public class Credentials
    {
        protected string url;
        protected string user;
        protected string password;

        public Credentials()
        {
            this.url = null;
            this.user = null;
            this.password = null;
        }

        public string Url { set { this.url = value; } get { return this.url; } }
        public string User { set { this.user = value; } get { return this.user; } }
        public string Password { set { this.password = value; } get { return this.password; } }
    }
}
