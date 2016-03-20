using cloudscribe.SimpleContent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Models
{
    public interface IHandlePubDateAboutToChange
    {
        Task HandlePubDateAboutToChange(Post post, DateTime newPubDate);
    }
}
