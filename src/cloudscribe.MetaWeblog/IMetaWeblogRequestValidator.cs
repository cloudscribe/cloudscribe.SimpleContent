using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog
{
    public interface IMetaWeblogRequestValidator
    {
        Task<bool> IsValid(MetaWeblogRequest request, CancellationToken cancellationToken);
    }
}
