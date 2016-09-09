using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Storage.EFCore.Models
{
    public class PostCategory
    {
        //public string Id { get; set; }
        public string Value { get; set; }

        public string PostEntityId { get; set; }

        public string ProjectId { get; set; } // so we can retrieve by project and delete if the project is deleted
    }
}
