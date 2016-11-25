using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class PostCacheOptions
    {
        public int CacheDurationInSeconds { get; set; } = 3600; //default 1 hour
    }
}
