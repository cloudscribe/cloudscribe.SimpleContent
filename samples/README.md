# cloudscribe SimpleContent Samples 

[SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent) is a simple, yet flexible content and blog engine for ASP.NET Core.

All of these samples are intended as something you could copy and use to start your own ASP.NET Core project in Visual Studio 2015. The WebApp in each solution is yours to customize and add your own custom functionality. There is no "cloudscribe" source code in the samples they only have NuGet dependencies on cloudscribe libraries and the have been configured with example Startup.cs code to get things working.

## SimpleContent with SimpleAuth and NoDb

[available now](https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/samples/simpleauthnodb)

This sample uses [cloudscribe SimpleAuth](https://github.com/joeaudette/cloudscribe.Web.SimpleAuth) for user authentication and uses [NoDb](https://github.com/joeaudette/NoDb) file system storage for content and data. 

[Simple Auth](https://github.com/joeaudette/cloudscribe.Web.SimpleAuth) is a bare bones authentication system for when only a few pre-configured users will need to login. It does not require a database, the users are configured form a json file in the root of the application. It has a smaller attack surface than more complex identity solutions because of the reduced complexity.

[NoDb](https://github.com/joeaudette/NoDb) is a "No Database" file system storage, it is also a "NoSql" storage system.

This sample is pre-populated with sample data, when you run it you can login with admin as the username and admin as the password.

Be sure to update the credentials before deployment.


## SimpleContent with cloudscribe Core and NoDb

[available now](https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/samples/cloudscribecorenodb)

This sample uses [cloudscribe Core](https://github.com/joeaudette/cloudscribe) for user authentication and [NoDb](https://github.com/joeaudette/NoDb) file system storage for content and data. 

[cloudscribe Core]https://github.com/joeaudette/cloudscribe is a multi-tenant web application foundation. It provides multi-tenant identity management for sites, users, and roles.

[NoDb](https://github.com/joeaudette/NoDb) is a "No Database" file system storage, it is also a "NoSql" storage system.

This sample is pre-populated with sample data. There are 2 pre-configured sites, the root level site and another folder site at /two

When you run it you can login to either site with admin@admin.com as the username and admin as the password.

Be sure to update the credentials before deployment.


## SimpleContent with cloudscribe Core and Entity Framework Core

[available now](https://github.com/joeaudette/cloudscribe.SimpleContent/tree/master/samples/cloudscribecore-ef)

This sample uses [cloudscribe Core](https://github.com/joeaudette/cloudscribe) for user authentication and Entity Framework/SqlServer storage for content and data.

[cloudscribe Core]https://github.com/joeaudette/cloudscribe is a multi-tenant web application foundation. It provides multi-tenant identity management for sites, users, and roles.

When you run it from Visual Studio it should initilaize a new localdb on the first run. 

You can login with admin@admin.com as the username and admin as the password

Be sure to update the credentials before deployment.


## Questions?

If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave  message.

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

