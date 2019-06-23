using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.PwaKit.Models
{
    public class PwaPreCacheItems
    {
        public PwaPreCacheItems()
        {
            Assets = new List<PreCacheItem>();
        }

        public List<PreCacheItem> Assets { get; set; }

    }
}
