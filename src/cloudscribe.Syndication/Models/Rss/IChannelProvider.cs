using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cloudscribe.Syndication.Models.Rss;

namespace cloudscribe.Syndication.Models.Rss
{
    public interface IChannelProvider
    {
        string Name { get; }
        Task<RssChannel> GetChannel(CancellationToken cancellationToken = default(CancellationToken));
    }
}
