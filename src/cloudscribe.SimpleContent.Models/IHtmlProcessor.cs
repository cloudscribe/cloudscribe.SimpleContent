//using System.Threading.Tasks;


//namespace cloudscribe.SimpleContent.Models
//{
//    public interface IHtmlProcessor
//    {
//        Task<string> ConvertMediaUrlsToRelative(string mediaVirtualPath, string absoluteBaseMediaUrl, string htmlInput);
//        string ConvertUrlsToAbsolute(string absoluteBaseMediaUrl, string htmlInput);
//        ImageSizeResult ExtractFirstImageDimensions(string htmlInput, string fallbackWidth = "550px", string fallbackHeight = "550px");
//        string ExtractFirstImageUrl(string htmlInput, string fallbackImageUrl = null);
//        string FilterCommentLinks(string rawComment);
//        string FilterHtml(string htmlInput, string cdnUrl = "", string rootPath = "");
//        string RemoveImageStyleAttribute(string htmlInput);
//        bool IsValidMarkup(string html);

//    }
//}