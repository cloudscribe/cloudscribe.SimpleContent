using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoDb
{
    public interface IStoragePathOptionsResolver
    {
        Task<StoragePathOptions> Resolve(string projectId);
    }
}
