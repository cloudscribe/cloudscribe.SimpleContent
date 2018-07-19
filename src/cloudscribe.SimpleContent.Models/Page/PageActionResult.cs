namespace cloudscribe.SimpleContent.Models
{
    public class PageActionResult
    {
        public PageActionResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        public bool Success { get; private set; }
        public string Message { get; private set; }

    }
}
