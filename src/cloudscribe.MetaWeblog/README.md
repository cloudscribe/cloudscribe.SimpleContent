# cloudscribe.Metaweblog is a re-useable implementation of the Metaweblog API for ASP.NET Core

It was implemented for use in [cloudscribe SimpleContent](https://github.com/joeaudette/cloudscribe.SimpleContent), but could be used by anyone who would like to add Metaweblog API support to their web application.

## What the heck is Metaweblog API

MetaWeblog API is something you can use to create, edit, and publish content using desktop tools such as [Open Live Writer](https://github.com/OpenLiveWriter/OpenLiveWrite). I specifically created this project in order to be able to use Open Live Writer with cloudscribe.SimpleContent, but I abstracted things so that others could use it too. Just implement and plugin your own implementation of [IMetaWeblogService](https://github.com/joeaudette/cloudscribe.SimpleContent/blob/master/src/cloudscribe.MetaWeblog/IMetaWeblogService.cs).

Note that this is not a generalized solution for XmlRpc, it is specifically an implementation of the MetaWeblog API

You can get it from nuget by adding this in your project.json dependencies

    "cloudscribe.MetaWeblog": "1.0.0-*"

If you have questions or just want to be social, say hello in our gitter chat room. I try to monitor that room on a regular basis while I'm working, but if I'm not around you can leave  message.

[![Join the chat at https://gitter.im/joeaudette/cloudscribe](https://badges.gitter.im/joeaudette/cloudscribe.svg)](https://gitter.im/joeaudette/cloudscribe?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)



