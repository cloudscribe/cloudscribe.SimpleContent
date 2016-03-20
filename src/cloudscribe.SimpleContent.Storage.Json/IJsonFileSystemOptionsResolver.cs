using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.Json
{
    public interface IJsonFileSystemOptionsResolver
    {
        Task<JsonFileSystemOptions> Resolve(string projectId);
    }
}
