using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EF
{
    /// <summary>
    /// a class to represent Category string, since Post has List<string> rather than a true model for category
    /// named it tag in case later a Category class is introduced in models
    /// </summary>
    public class Tag
    {
        public string Id { get; set; }

        public string ProjectId { get; set; }

        public string ContentType { get; set; } = "Post";

        public string Value { get; set; }
    }
}
