using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudscribe.SimpleContent.Storage.NoDb
{
    public class DefaultKeyGenerator : GuidKeyGenerator, IKeyGenerator
    {
        public override string GenerateKey(IContentItem item)
        {
            if(!string.IsNullOrWhiteSpace(item.Slug))
            {
                return item.Slug;
            }

            return base.GenerateKey(item);
        }
    }
}
