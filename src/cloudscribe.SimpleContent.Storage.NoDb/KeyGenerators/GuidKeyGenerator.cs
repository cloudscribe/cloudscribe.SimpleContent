using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class GuidKeyGenerator : IKeyGenerator
    {
        public virtual string GenerateKey(IContentItem item)
        {
            if(!string.IsNullOrEmpty(item.Id) && item.Id.Length == 36)
            {
                return item.Id;
            }

            return Guid.NewGuid().ToString();
        }
    }
}
