using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.Syndication.Models.Rss
{
    public class DefaultChannelProviderResolver : IChannelProviderResolver
    {
        public IChannelProvider GetCurrentChannelProvider(
            IEnumerable<IChannelProvider> channelProviders)
        {
                return channelProviders.FirstOrDefault();
        }
    }
}
