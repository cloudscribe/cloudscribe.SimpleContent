// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-02-16
// Last Modified:           2016-02-16
// 

using cloudscribe.MetaWeblog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace cloudscribe.MetaWeblog.Tests
{
    public class MetaWeblogRequestParserTests
    {

        [Fact]
        public void Can_Parse_GetUserBlogs_Request()
        {

            var parser = new MetaWeblogRequestParser();
            var fileName = "testfiles/get-user-blogs-request-1.xml";
            XDocument doc = null;
            using (StreamReader reader = File.OpenText(fileName))
            {
                doc = XDocument.Load(reader.BaseStream);
            }

            Assert.NotNull(doc);

            var request = parser.ParseRequest(doc);

            Assert.NotNull(request);

            Assert.Equal("0123456789ABCDEF", request.AppKey);
            Assert.Equal("frodobaggins", request.UserName);
            Assert.Equal("thekeystupid", request.Password);

        }

        [Fact]
        public void Can_Parse_GetCategories_Request()
        {

            var parser = new MetaWeblogRequestParser();
            var fileName = "testfiles/get-categories-request-1.xml";
            XDocument doc = null;
            using (StreamReader reader = File.OpenText(fileName))
            {
                doc = XDocument.Load(reader.BaseStream);
            }

            Assert.NotNull(doc);

            var request = parser.ParseRequest(doc);

            Assert.NotNull(request);

            Assert.Equal("default", request.BlogId);
            Assert.Equal("frodobaggins", request.UserName);
            Assert.Equal("thekeystupid", request.Password);

        }

        [Fact]
        public void Can_Parse_GetRecentPosts_Request()
        {

            var parser = new MetaWeblogRequestParser();
            var fileName = "testfiles/get-recent-posts-request-1.xml";
            XDocument doc = null;
            using (StreamReader reader = File.OpenText(fileName))
            {
                doc = XDocument.Load(reader.BaseStream);
            }

            Assert.NotNull(doc);

            var request = parser.ParseRequest(doc);

            Assert.NotNull(request);

            Assert.Equal("default", request.BlogId);
            Assert.Equal("frodobaggins", request.UserName);
            Assert.Equal("thekeystupid", request.Password);

        }

        [Fact]
        public void Can_Parse_NewPost_Request()
        {

            var parser = new MetaWeblogRequestParser();
            var fileName = "testfiles/new-post-request-1.xml";
            XDocument doc = null;
            using (StreamReader reader = File.OpenText(fileName))
            {
                doc = XDocument.Load(reader.BaseStream);
            }

            Assert.NotNull(doc);

            var request = parser.ParseRequest(doc);

            Assert.NotNull(request);

            Assert.Equal("default", request.BlogId);
            Assert.Equal("frodobaggins", request.UserName);
            Assert.Equal("thekeystupid", request.Password);
            Assert.Equal("olw post 1", request.Post.title);
            Assert.True(request.Post.description.Contains("cooking"));

        }

        [Fact]
        public void Can_Parse_DeletePost_Request()
        {

            var parser = new MetaWeblogRequestParser();
            var fileName = "testfiles/delete-post-request.xml";
            XDocument doc = null;
            using (StreamReader reader = File.OpenText(fileName))
            {
                doc = XDocument.Load(reader.BaseStream);
            }

            Assert.NotNull(doc);

            var request = parser.ParseRequest(doc);

            Assert.NotNull(request);

            Assert.Equal("0123456789ABCDEF", request.AppKey);
            Assert.Equal("frodobaggins", request.UserName);
            Assert.Equal("thekeystupid", request.Password);
            Assert.Equal("c65ea975-516c-40aa-bbaa-d6192fbc30d8", request.PostId);
           

        }

        [Fact]
        public void Can_Parse_NewMedia_Request()
        {

            var parser = new MetaWeblogRequestParser();
            var fileName = "testfiles/new-media-request-1.xml";
            XDocument doc = null;
            using (StreamReader reader = File.OpenText(fileName))
            {
                doc = XDocument.Load(reader.BaseStream);
            }

            Assert.NotNull(doc);

            var request = parser.ParseRequest(doc);

            Assert.NotNull(request);

            Assert.Equal("default", request.BlogId);
            Assert.Equal("frodobaggins", request.UserName);
            Assert.Equal("thekeystupid", request.Password);
            Assert.Equal("Open-Live-Writer/One-with-a-picture-please_E33C/IMG_1311.jpg", request.MediaObject.name);
            Assert.Equal("image/jpeg", request.MediaObject.type);
            Assert.NotNull(request.MediaObject.bytes);
            Assert.True(request.MediaObject.bytes.Length > 0);

        }
    }
}
