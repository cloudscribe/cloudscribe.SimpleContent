
using System.Threading;
using System.Threading.Tasks;

namespace NoDb
{
    public interface IStoragePathResolver<TObject> where TObject : class
    {
        Task<string> ResolvePath(
            string projectId, 
            string key = "", 
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            );

        /// <summary>
        /// the purpose of this overload is so implementors of the interface can do more complex storage patterns
        /// ie I will try to implement for the blog to store posts in year/month folders according to pubdate
        /// so my custom resolver for Posts will be able to interogate the object that needs to be saved and determine where to put it
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="ensureFoldersExist"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> ResolvePath(
            string projectId, 
            string key, 
            TObject obj, 
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken)
            );
    }
}
