# cloudscribe.SimpleContent

A simple, yet flexible content and blogging engine for ASP.NET Core that can work with or without a database. This project has borrowed significantly from [Mads Kristensen's MiniBlog](https://github.com/madskristensen/MiniBlog) both for ideas and code but re-implemented and extended in the newer ASP.NET Core framework. 

This project supports content pages in addition to blog posts, you can create and edit both pages and blog posts in the web browser or using [Open Live Writer](https://github.com/OpenLiveWriter/OpenLiveWriter)

If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave  message.

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

## Start simple with no database and migrate to a database later if you need one

Not all web site projects need a database, there can be many benefits to not using one including performance, scalability, portability, lower cost, and ease of making backup copies of the entire site. It should even be possible to make a site that runs from a thumb drive.

In fact, for blogs, there has been kind of a trend towards using [Static Site Generators](https://www.staticgen.com/). This project is not a static site generator, but by storing content as json files it can get some of the same benefits and be used in a similar way to using a static site genersator. For example you could host a localhost or intranet version of your site for producing and reviewing content, then when ready to publish you could committ the changes to a git repository and then do deployment from git to Azure for example, which would give you a highly scaleable site without the need or cost of a database and with a complete history of changes in git. Personal blogs and sites and small brochure sites are good candidates for not using a database.

Some sites do need a database though and we plan to support using both Entity Framework Core and MongoDb. If you need users to be able to register on your site or if you have more than a few editors, or for larger projects, you will typically want a database.

My plan is to usually build sites without a database, but implement a migration utility to be able to migrate any site from files to a database later if the needs of the project require it.

### Current Features
* Cross platform, works on Windows, OSX, and Linux
* __No database__ required uses json for pages and can use json or xml for blog posts
* For blog posts, supports the same XML format as MiniBlog and BlogEngine.NET
* Move your existing blog to MiniBlog using [MiniBlog Formatter](https://github.com/madskristensen/MiniBlogFormatter)
* __Inline editing__ of blog posts and pages
* Supports multiple tenants by host name even without a database
* Easy setting for serving static files from another domain. 
*  __Open Live Writer__ (OLW) and __Windows Live Writer__ (WLW) support
* You don't have to use OLW/WLW (but you should)
* Schedule posts to be published on a future date
* Supports blog urls with or without date segments
* Url date segments are hackable, ie /blog/2016/03/16 shows posts for the day, /blog/2016/03 shows posts for the month and /blog/2016 shows posts for the year
* Comments support
* [Recaptcha support](https://www.google.com/recaptcha/intro/index.html) to reduce comment spam
* __Gravatar__ support 
* Can easily be replaced by 3rd-party commenting systems such as Disqus
* __Drag 'n drop__ images to upload
* __OpenGraph__ enabled
* Responsive Theming support based on Bootstrap
* __SEO__ optimized
* Uses HTML 5 __microdata__ to add semantic meaning
* Works on any ASP.NET Core host including __Windows Azure__ Websites

### Planned Features
* Support for using Entity Framework Core
* Support for using cloudscribe.Core for user and site management
* Support for using MongoDb
* A Utility for importing the json or xml content into Entity Framework Core or MongoDb for easy migration
* RSS and ATOM __feeds__
* Support for __robots.txt__ and __sitemap.xml__
* Automatically __optimizes uploaded images__


###### below should go in the wiki

### Connecting with Open Live Writer (OLW) or Windows Live Writer (WLW)

To connect 

- Launch OLW/WLW

- If you have not used OLW/WLW to connect to a blog you will get a dialog window asking you to specify what blog service you use. If you have already connected OLW/WLW to a blog, you can go to _Blogs -> Add blog account..._ and get to the same dialog window.

- In the __What blog service do you use?__ dialog window you will tick the _Other services_ radio option and click next.

- The __Add a blog account__ dialog window will ask you for the web address of your blog, the username and password. The web address is the blog page address of your site. ie http://yourdomain/blog 

- The __Download Blog Theme__ dialog window will let you know OLW/WLW can download your blog theme if you allow it to publish a temporary post. Selecting yes will allow you to view how your posts will look directly from the OLW/WLW editor. 

- The __Select blog type__ dialog window will let you know OLW/WLW was not able to detect your blog type. It will ask you for the type of blog and the remote posting URL.  
Type of blog that you are using: _Metaweblog API_  
Remote posting URL for your blog: _http://yourdomain/api/metaweblog
Click next.

- The __Your blog has been set up__ dialog window will let you give your blog a nickname for the OLW/WLW instance. Change that if you want and click finish to get to posting!

Open Live Writer can be downloaded at:
[http://openlivewriter.org/](http://openlivewriter.org/)

Windows Live Writer can be downloaded at:  
[http://windows.microsoft.com/en-us/windows-live/essentials](http://windows.microsoft.com/en-us/windows-live/essentials)  

