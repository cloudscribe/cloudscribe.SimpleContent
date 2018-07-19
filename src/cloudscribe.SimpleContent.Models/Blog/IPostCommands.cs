using System;
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
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken));

        Task Update(
            string projectId,
            IPost post,
            CancellationToken cancellationToken = default(CancellationToken));

        //Task HandlePubDateAboutToChange(
        //    string projectId,
        //    IPost post, 
        //    DateTime newPubDate, 
        //    CancellationToken cancellationToken = default(CancellationToken));

    }

}
