using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.MetaWeblog.Models
{
    public class ApiOptions
    {
        /// <summary>
        /// only for debugging purposes would you ever set either of these true
        /// don't leave it as true
        /// </summary>
        public bool DumpRequestXmlToDisk { get; set; } = false;
        public bool DumpResponseXmlToDisk { get; set; } = false;
        public string AppRootDumpFolderVPath { get; set; } = "/cloudscribe_config/data_xml/metaweblogxmldumps/";
    }
}
