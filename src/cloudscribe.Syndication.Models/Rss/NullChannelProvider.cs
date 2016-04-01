using System;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.Syndication.Models.Rss
{
    public class NullChannelProvider : IChannelProvider
    {
        public string Name { get { return "NullChannelProvider"; } }
        

        public Task<RssChannel> GetChannel(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            RssChannel channel = null;
            return Task.FromResult(channel);
        }

        
    }
}
