using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.FileManager.Web.Models
{
    public class CkBrowseModel
    {
        /// <summary>
        /// the type of files to browse
        /// </summary>
        public string Type { get; set; } = "image";

        /// <summary>
        /// the client side id of the editor
        /// </summary>
        public string CKEditor { get; set; }

        public string CKEditorFuncNum { get; set; }

        public string LangCode { get; set; } = "en";

        public bool ResizeImages { get; set; } = true;

        public string InitialVirtualPath { get; set; }

        public string FileTreeServiceUrl { get; set; }

        public string UploadServiceUrl { get; set; }

        public string CreateFolderServiceUrl { get; set; }

        public string AllowedFileExtensionsRegex { get; set; }


    }
}
