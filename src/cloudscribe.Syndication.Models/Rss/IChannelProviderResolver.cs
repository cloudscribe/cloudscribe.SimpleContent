using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Syndication.Models.Rss
{
    public interface IChannelProviderResolver
    {
        IChannelProvider GetCurrentChannelProvider(
            IEnumerable<IChannelProvider> channelProviders);
    }
}
