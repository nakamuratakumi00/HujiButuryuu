using System;

namespace MacssWeb.Common
{
    public class Config
    {
        public const int UploadMaxFileSize = 2097152;

        public static string UploadMaxFileSizeText
        {
            get
            {
                var size = String.Format("{0:#,0}B", UploadMaxFileSize);
                if (UploadMaxFileSize > (1024 * 1024))
                {
                    size = String.Format("{0:#,0}MB", UploadMaxFileSize / (1024 * 1024));
                }
                else if (UploadMaxFileSize > 1024)
                {
                    size = String.Format("{0:#,0}KB", UploadMaxFileSize / 1024);
                }

                return size;
            }
        }
    }
}