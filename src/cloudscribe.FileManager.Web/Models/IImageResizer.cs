using System.Drawing;

namespace cloudscribe.FileManager.Web.Models
{
    public interface IImageResizer
    {
        void ResizeImage(
            string sourceFilePath,
            string targetDirectoryPath,
            string newFileName,
            string mimeType,
            int maxWidth,
            int maxHeight,
            bool allowEnlargement = false,
            long quality = 90, 
            Color backgroundColor = default(Color)
            );
    }
}
