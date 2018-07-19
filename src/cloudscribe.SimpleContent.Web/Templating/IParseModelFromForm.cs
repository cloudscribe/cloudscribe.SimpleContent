using Microsoft.AspNetCore.Http;

namespace cloudscribe.SimpleContent.Web.Templating
{
    public interface IParseModelFromForm
    {
        string ParserName { get; }
        object ParseModel(string modelType, IFormCollection form);
    }
}
