// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-23
// Last Modified:           2016-04-24
// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using NoDb;
using cloudscribe.SimpleContent.Models;

namespace NoDb.Test
{
    public class StringSerializerTests
    {

        [Fact]
        public void Can_RoundTrip_Project()
        {
            // arrange
            var project = new ProjectSettings();
            project.ProjectId = "mycustomProject";
            project.Title = "My Cool Project";
            project.TimeZoneId = "Eastern Standard Time";
            project.SmtpUser = "mysmtpuser";

            var serializaer = new StringSerializer<ProjectSettings>();

            //act
            string data = serializaer.Serialize(project);
            var projectCopy = serializaer.Deserialize(data);

            //assert

            Assert.True(project.ProjectId == projectCopy.ProjectId);
            Assert.True(project.Title == projectCopy.Title);
            Assert.True(project.TimeZoneId == projectCopy.TimeZoneId);
            Assert.True(project.SmtpUser == projectCopy.SmtpUser);


        }

    }
}
