# PyramidAnalyticsControl: a C# client for the Pyramid Analytics REST API
 
[Pyramid Analytics REST API documentation](https://help.pyramidanalytics.com/Content/Root/developer/reference/APIs/REST%20API/REST%20APIs.htm?tocpath=Tech%20Reference%7CAPIs%7CREST%20API%27s%7C_____0)


PyramidAnalyticsLib is a service that:
- Configures URL, user name and password to a Pyramid 2020 server.
- Authenticates against the server.
- Adds additional authentication methods for ByToken and Embed use cases [Pyramid REST Auth APIs](https://help.pyramidanalytics.com/Content/Root/developer/reference/APIs/REST%20API/API2/auth.htm?tocpath=Tech%20Reference%7CAPIs%7CREST%20API%27s%7CAPI%202.0%20Methods%7CAuth%20APIs%7C_____0)
- Maintains Tenants, User Profiles, Roles and Tasks/Schedules




PyramidAnalyticsUnitTest does the obvious and shows examples of using PyramidAnalyticsLib.

# Connect to and login to a Pyramid deployment

PyramidAnayticsServer p = new PyramidAnayticsServer(credentials.Url, credentials.User, credentials.Password, true);

# Get authentication tokens for use in front end apps

Tokens used for embedding need an Internet domain where they are embedding from.
When embedding in a web page, this is "document.domain" in JavaScript.

string userEmbeddingToken = p.authenticateEmbed(embedDomain);

User ids have to pre-exist in Pyramid in order to login as those users.

string userLoginByToken = p.authenticateUserByToken(loginAsUser);

string userEmbedLoginByToken = p.authenticateUserEmbedByToken(loginAsUser, embedDomain);

# Access Maintenance

[Tenants, security and User creation workflow via REST](https://help.pyramidanalytics.com/Content/Root/developer/reference/APIs/REST%20API/API2/access/createTenant.htm?tocpath=Tech%20Reference%7CAPIs%7CREST%20API%27s%7CAPI%202.0%20Methods%7CAccess%20APIs%7C_____9)

The script in the above page shows the creation sequence flow. Also look at PyramidAnalyticsUnitTest.testTenantsAndRoles


