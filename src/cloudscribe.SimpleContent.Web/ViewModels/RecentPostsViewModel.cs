using cloudscribe.SimpleContent.Models;
using System.Collections.Generic;
using cloudscribe.Web.Common;
using System;

namespace cloudscribe.SimpleContent.Web.ViewModels
{
    public class RecentPostsViewModel
    {
        public RecentPostsViewModel()
        {
            ProjectSettings = new ProjectSettings();
            Posts = new List<IPost>();
        }

        public IProjectSettings ProjectSettings { get; set; }

        public ITimeZoneHelper TimeZoneHelper { get; set; }
        public string TimeZoneId { get; set; } = "GMT";

        public List<IPost> Posts { get; set; }

        public string FormatDate(DateTime pubDate)
        {
            var localTime = TimeZoneHelper.ConvertToLocalTime(pubDate, TimeZoneId);
            return localTime.ToString(ProjectSettings.PubDateFormat);
        }
    }
}
