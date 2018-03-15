using cloudscribe.Core.Models;
using cloudscribe.Core.SimpleContent.ViewModels;
// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:                  Joe Audette
// Created:                 2018-03-15
// Last Modified:           2018-03-15
// 


using cloudscribe.Email;
using cloudscribe.SimpleContent.Models;
using cloudscribe.Web.Common.Razor;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace cloudscribe.Core.SimpleContent
{
    public class CoreProjectEmailService : IProjectEmailService
    {
        public CoreProjectEmailService(
            ViewRenderer viewRenderer,
            ISiteContextResolver siteResolver,
            IEmailSenderResolver emailSenderResolver,
            ILogger<CoreProjectEmailService> logger
            )
        {
            _viewRenderer = viewRenderer;
            _emailSenderResolver = emailSenderResolver;
            _siteResolver = siteResolver;
            _log = logger;
        }

        private ViewRenderer _viewRenderer;
        private ISiteContextResolver _siteResolver;
        private IEmailSenderResolver _emailSenderResolver;
        private ILogger _log;

        public async Task SendCommentNotificationEmailAsync(
            IProjectSettings project,
            IPost post,
            IComment comment,
            string postUrl,
            string approveUrl,
            string deleteUrl
            )
        {
            var sender = await _emailSenderResolver.GetEmailSender(project.Id);
            if (sender == null)
            {
                var logMessage = $"failed to send account confirmation email because email settings are not populated for site {project.Title}";
                _log.LogError(logMessage);
                return;
            }

            if (string.IsNullOrWhiteSpace(project.CommentNotificationEmail))
            {
                var logMessage = $"failed to send comment notification email because CommentNotificationEmail is not populated for project {project.Id}";
                _log.LogError(logMessage);
                return;
            }
            
            var site = await _siteResolver.GetById(new Guid(project.Id));

            var model = new CoreCommentNotificationModel(site, project, post, comment, postUrl);
            var subject = "Blog comment: " + post.Title;

            string plainTextMessage = null;
            string htmlMessage = null;

            try
            {
                try
                {
                    htmlMessage
                        = await _viewRenderer.RenderViewAsString<CoreCommentNotificationModel>("CommentNotificationEmail", model);
                }
                catch (Exception ex)
                {
                    _log.LogError("error generating html email from razor template", ex);
                    return;
                }

                await sender.SendEmailAsync(
                    project.CommentNotificationEmail, //to
                    site.DefaultEmailFromAddress, //from
                    subject,
                    plainTextMessage,
                    htmlMessage,
                    comment.Email, //replyto
                    configLookupKey: project.Id
                    ).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _log.LogError($"error sending comment notification email {ex.Message} : {ex.StackTrace}");
            }

        }

    }
}
