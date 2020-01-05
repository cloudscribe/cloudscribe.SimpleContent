using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cloudscribe.MetaWeblog
{
    public interface IMetaWeblogResultFormatter
    {
        XDocument Format(MetaWeblogResult metaWeblogResult);
    }
}
