# cloudscribe SimpleContent

A simple, yet flexible content and blog engine for ASP.NET Core that can work with or without a database. This project has borrowed significantly from [Mads Kristensen's MiniBlog](https://github.com/madskristensen/MiniBlog) both for ideas and code but re-implemented and extended in the newer ASP.NET Core framework. Get the big picture at [cloudscribe.com](https://www.cloudscribe.com/docs/introduction)

[Documentation](https://www.cloudscribe.com/docs/cloudscribe-simplecontent) - in progress so check back often. Also note that cloudscribe SimpleContent is being used to make the documentation on [cloudscribe.com](https://www.cloudscribe.com)

If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave  message.

### Build Status

| Windows  | Linux |
| ------------- | ------------- |
| [![Build status](https://ci.appveyor.com/api/projects/status/d0900wcf3y7l7jup/branch/master?svg=true)](https://ci.appveyor.com/project/joeaudette/cloudscribe-simplecontent/branch/master)  | [![Build Status](https://travis-ci.org/cloudscribe/cloudscribe.SimpleContent.svg?branch=master)](https://travis-ci.org/cloudscribe/cloudscribe.SimpleContent) |

[![Twitter URL](https://img.shields.io/twitter/url/http/shields.io.svg?style=social)](https://twitter.com/cloudscribeweb) [![Twitter Follow](https://img.shields.io/twitter/follow/cloudscribeweb.svg?style=social&label=Follow)](https://twitter.com/cloudscribeweb)
[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Getting Started

See the [Introduction](https://www.cloudscribe.com/docs/introduction) to get the big picture and learn how to start new projects using our project template for Visual Studio or the .NET Core CLI

### Current Features
* Supports markdown and/or html editing, the [documentation on using markdown](https://www.cloudscribe.com/docs/using-markdown)
* Create and edit pages and blog posts right from the web browser or using [Open Live Writer](https://www.cloudscribe.com/docs/using-open-live-writer)
* Built in image browser, uploader, cropper, with configurable automatic resizing, and even drag/drop images right into the editor
* Built in Page Manager - for easy drag/drop arrangement of the page hierarchy
* For technical articles includes built in syntax highlighter using the [CodeSnippet plugin in CKEditor](https://www.cloudscribe.com/docs/customizing-the-editor)
* Pages can be protected by roles for private or premium content
* Schedule posts and pages to be published on a future date
* Supports blog urls with or without date segments
* Optional internal comment system for the blog. Built in support for Disqus and not difficult to integrate some other comment system
* RSS feed built in at /api/rss
* [Google Site Map](https://www.cloudscribe.com/docs/easy-google-sitemaps) built in at /api/sitemap
* Responsive [theming support](https://www.cloudscribe.com/docs/themes-and-web-design) based on Bootstrap
* Uses HTML 5 microdata to add semantic meaning and improve SEO
* Cross platform, runs on ASP.NET Core which works on Windows, Mac, and Linux
* Comments support - can easily be replaced by 3rd-party commenting systems such as Disqus
* No database required - can use [markdown](https://www.cloudscribe.com/docs/using-markdown) or json for pages and can use markdown or xml for blog posts via [NoDb](https://github.com/joeaudette/NoDb). The XML format is the same as MiniBlog and BlogEngine.NET and you should be able to [migrate from other platforms](https://www.cloudscribe.com/docs/migrating-content-from-other-platforms)
* You can optionally use a database - it currently supports SQLite, MS SQL, PostgresSql, and MySql using Entity Framework Core
* Can use either [cloudscribe Core](https://www.cloudscribe.com/docs/cloudscribe-core) or [cloudscribe SimpleAuth](https://github.com/joeaudette/cloudscribe.Web.SimpleAuth) for user accounts. (I recommend use cloudscribe Core even for small sites)
* Can also be [integrated with other authentication systems](https://www.cloudscribe.com/docs/integrating-with-other-authentication-systems)
* Supports [multiple tenants](https://www.cloudscribe.com/docs/multi-tenant-support) via integration using cloudscribe Core
* [Supports Localization](https://www.cloudscribe.com/docs/localization)

### Planned Features
* Support for using MongoDb - hoping for a community member to do that
* A Utility for importing the NoDb content into Entity Framework Core or MongoDb for easy migration

### Screenshots

![Blog Screen shot](https://github.com/joeaudette/cloudscribe.SimpleContent/raw/master/screenshots/blog-index.png)

![page edit screen shot](https://github.com/joeaudette/cloudscribe.SimpleContent/raw/master/screenshots/page-edit.png)

![file browser Screen shot](https://github.com/joeaudette/cloudscribe.SimpleContent/raw/master/screenshots/file-browser.png)

![file selection Screen shot](https://github.com/joeaudette/cloudscribe.SimpleContent/raw/master/screenshots/image-selection.png)

![image cropper Screen shot](https://github.com/joeaudette/cloudscribe.SimpleContent/raw/master/screenshots/image-cropper.png)

![Blog Screen shot](https://github.com/joeaudette/cloudscribe.SimpleContent/raw/master/screenshots/drag-drop-page-manager.png)

## Start simple with no database and migrate to a database later if you need one

Not all web site projects need a database, there can be many benefits to not using one including performance, scalability, portability, lower cost, and ease of making backup copies of the entire site. It should even be possible to make a site that runs from a thumb drive.

In fact, for blogs, there has been kind of a trend towards using [Static Site Generators](https://www.staticgen.com/). This project is not a static site generator, but by storing content as json files it can get some of the same benefits and be used in a similar way to using a static site generator. For example you could host a localhost or intranet version of your site for producing and reviewing content, then when ready to publish you could commit the changes to a git repository and then do deployment from git to Azure for example, which would give you a highly scaleable site without the need or cost of a database and with a complete history of changes in git. Personal blogs and sites and small brochure sites are good candidates for not using a database.

Some sites do need a database though and we plan to support using both Entity Framework Core and MongoDb. If you need users to be able to register on your site or if you have more than a few editors, or for larger projects, you will typically want a database.

My plan is to usually build sites without a database (except for large projects), but implement a migration utility to be able to migrate any site from files to a database later if the needs of the project require it.

