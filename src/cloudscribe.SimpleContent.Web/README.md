# cloudscribe.SimpleContent.Web

[![NuGet](https://img.shields.io/nuget/v/cloudscribe.SimpleContent.Web.svg)](https://www.nuget.org/packages/cloudscribe.SimpleContent.Web)
[![License: Apache-2.0](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

A flexible ASP.NET Core web library providing the main UI, controllers, and integration points for the cloudscribe.SimpleContent CMS and blog engine. This package delivers the core web application features for managing, editing, and displaying content and blog posts, with support for themes, markdown, and extensibility.

## Features
- Full-featured content and blog engine for ASP.NET Core
- Admin UI for creating and managing posts and pages
- Markdown editing and rendering
- Theme support and customizable layouts
- Extensible with custom controllers and views
- Works with or without a database (supports NoDb and EFCore providers)

## Installation

```shell
Install-Package cloudscribe.SimpleContent.Web
```

## Usage
- Add this package to your ASP.NET Core project.
- Configure and register SimpleContent services in `Startup.cs` or `Program.cs`.
- Use the provided routes and controllers to expose content management and blog features.

## License

Licensed under the Apache 2.0 License. See the [LICENSE](https://github.com/cloudscribe/cloudscribe.SimpleContent/blob/main/LICENSE) file for details.
