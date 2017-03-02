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
            long quality = 90,
            bool allowEnlargement = true,
            Color backgroundColor = default(Color)
            );
    }
}
