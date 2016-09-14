# cloudscribe SimpleContent Samples - simpleauthnodb

This sample uses [cloudscribe SimpleAuth](https://github.com/joeaudette/cloudscribe.Web.SimpleAuth) which is best suited for projects where only one or a few people need to be able to login. Users are pre-configured in the app-tenant-users.json file, and project settings are configured in the app-content-settings.json file. The sample uses NuGet packages for cloudscribe SimpleContent and cloudscribe SimpleAuth but has some integration code within the WebApp project to implement mutliple tenants. A good way to setup your own site is to use this sample. You can publish it from Visual Studio as it is or you can customize it by adding your own code and/or projects.

This sample uses [NoDb](https://github.com/joeaudette/NoDb) file system storage for content and data. NoDb is a "No Database" file system storage, it is also a "NoSql" storage system.

You can login to the sample data with admin as the username and admin as the password

Be sure to update the credentials before deployment.


If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave  message.

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Prerequisites

[Visual Studio 2015 Update 3](https://www.visualstudio.com/en-us/downloads) or newer and [ASP.NET Core 1.0 with Preview Tooling](https://dot.net/)

## Running the Sample simpleauthnodb.sln

If you launch it in Visual Studio 2015 with IIS Express it will have a single tenant running at localhost:52472. You can also run it with 2 tenants enabled. You can also run both configured tenants by opening a command window in the root of the WebApp project and execute the command:

    dotnet run
	
that will get you both localhost:50000 and localhost:50002 which you can open in your web browser. Both sites are pre-configured with some content and you can login with admin/admin in either or both tenants.

After you login a little pencil will appear in the upper right corner, it toggles the editor toolbar.

You should also be able to [create and edit blog posts and pages using the web browser or using Open Live Writer](https://github.com/joeaudette/cloudscribe.SimpleContent/wiki/Using-Open-Live-Writer)

If you decide to use this sample to build your site you should of course change the user name and password in the app-tenant-users.json file




