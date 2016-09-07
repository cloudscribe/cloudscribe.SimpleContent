// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2016-04-21
// Last Modified:           2016-09-07
// 

using cloudscribe.Messaging.Email;
using cloudscribe.SimpleContent.Models;
using Microsoft.Extensions.Logging;
using cloudscribe.Web.Common.Razor;
using System;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Services
{
    public class ProjectEmailService : IProjectEmailService
    {

        public ProjectEmailService(
            ViewRenderer viewRenderer,
            ILogger<ProjectEmailService> logger)
        {
            this.viewRenderer = viewRenderer;
            log = logger;
        }

        private ViewRenderer viewRenderer;
        private ILogger log;

        public async Task SendCommentNotificationEmailAsync(
            ProjectSettings project,
            IPost post,
            Comment comment,
            string postUrl,
            string approveUrl,
            string deleteUrl
            )
        {
            var smtpOptions = GetSmptOptions(project);

            if (smtpOptions == null)
            {
                var logMessage = $"failed to send comment notification email because smtp settings are not populated for project {project.Id}";
                log.LogError(logMessage);
                return;
            }

            if (string.IsNullOrWhiteSpace(project.CommentNotificationEmail))
            {
                var logMessage = $"failed to send comment notification email because CommentNotificationEmail is not populated for project {project.Id}";
                log.LogError(logMessage);
                return;
            }

            if (string.IsNullOrWhiteSpace(project.EmailFromAddress))
            {
                var logMessage = $"failed to send comment notification email because EmailFromAddress is not populated for project {project.Id}";
                log.LogError(logMessage);
                return;
            }

            var model = new CommentNotificationModel(project, post, comment, postUrl);
            var subject = "Blog comment: " + post.Title;

            string plainTextMessage = null;
            string htmlMessage = null;
            var sender = new EmailSender();

            try
            {
                try
                {
                    htmlMessage 
                        = await viewRenderer.RenderViewAsString<CommentNotificationModel>("CommentEmail", model);
                }
                catch(Exception ex)
                {
                    log.LogError("error generating html email from razor template", ex);
                    return;
                }
                
                await sender.SendEmailAsync(
                    smtpOptions,
                    project.CommentNotificationEmail, //to
                    project.EmailFromAddress, //from
                    subject,
                    plainTextMessage,
                    htmlMessage,
                    comment.Email //replyto
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                log.LogError("error sending comment notification email", ex);
            }

        }

        private SmtpOptions GetSmptOptions(ProjectSettings project)
        {
            if (string.IsNullOrWhiteSpace(project.SmtpServer)) { return null; }

            SmtpOptions smtpOptions = new SmtpOptions();
            smtpOptions.Password = project.SmtpPassword;
            smtpOptions.Port = project.SmtpPort;
            smtpOptions.PreferredEncoding = project.SmtpPreferredEncoding;
            smtpOptions.RequiresAuthentication = project.SmtpRequiresAuth;
            smtpOptions.Server = project.SmtpServer;
            smtpOptions.User = project.SmtpUser;
            smtpOptions.UseSsl = project.SmtpUseSsl;

            return smtpOptions;
        }


    }
}
