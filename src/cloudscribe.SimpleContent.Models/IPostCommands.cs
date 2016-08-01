using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IPostCommands
    {
        Task Delete(
            string projectId, 
            string postId, 
            CancellationToken cancellationToken = default(CancellationToken));

        Task Create(
            string projectId,
            Post post,
            CancellationToken cancellationToken = default(CancellationToken));

        Task Update(
            string projectId,
            Post post,
            CancellationToken cancellationToken = default(CancellationToken));

        Task HandlePubDateAboutToChange(
            Post post, 
            DateTime newPubDate, 
            CancellationToken cancellationToken = default(CancellationToken));

    }

}
