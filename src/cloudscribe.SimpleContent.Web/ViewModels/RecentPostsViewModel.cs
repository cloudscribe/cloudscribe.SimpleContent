using cloudscribe.SimpleContent.Models;
using System.Collections.Generic;
using cloudscribe.Web.Common;
using System;
using cloudscribe.SimpleContent.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class RecentPostsViewModel
    {
        public RecentPostsViewModel(IContentProcessor contentProcessor)
        {
            _contentProcessor = contentProcessor;
            
            ProjectSettings = new ProjectSettings();
            Posts = new List<IPost>();
        }

        private IContentProcessor _contentProcessor;
        

        public IProjectSettings ProjectSettings { get; set; }

        public ITimeZoneHelper TimeZoneHelper { get; set; }
        public string TimeZoneId { get; set; } = "GMT";

        public List<IPost> Posts { get; set; }

        public string FormatDate(DateTime pubDate)
        {
            var localTime = TimeZoneHelper.ConvertToLocalTime(pubDate, TimeZoneId);
            return localTime.ToString(ProjectSettings.PubDateFormat);
        }

        //private string pslug = string.Empty;
        //private string firstImageUrl;
        public string ExtractFirstImageUrl(IPost post, IUrlHelper urlHelper, string fallbackImageUrl = null)
        {
            return _contentProcessor.ExtractFirstImageUrl(post, urlHelper, fallbackImageUrl);

            //if (urlHelper == null) return string.Empty;
            //if (post == null) return string.Empty;

            //var baseUrl = string.Concat(urlHelper.ActionContext.HttpContext.Request.Scheme,
            //            "://",
            //            urlHelper.ActionContext.HttpContext.Request.Host.ToUriComponent());

            //if (post.ContentType == "markdown")
            //{
            //    var mdImg = mdProcessor.ExtractFirstImageUrl(post.Content);
            //    if (!string.IsNullOrEmpty(mdImg))
            //    {
            //        if (mdImg.StartsWith("http")) return mdImg;

            //        return baseUrl + mdImg;
            //    }

            //    return string.Empty;
            //}

            //if (!string.IsNullOrWhiteSpace(firstImageUrl) && pslug == post.Slug)
            //{
            //    if (firstImageUrl.StartsWith("http")) return firstImageUrl;

            //    return baseUrl + firstImageUrl; //don't extract it more than once
            //}

            //if (post == null) return string.Empty;


            //firstImageUrl = filter.ExtractFirstImageUrl(post.Content);
            //pslug = post.Slug;

            //if (firstImageUrl == null) return fallbackImageUrl;

            //if (firstImageUrl.StartsWith("http")) return firstImageUrl;



            //return baseUrl + firstImageUrl;
        }

    }
}
