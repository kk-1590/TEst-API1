namespace AdvanceAPI.Classes
{
    public static class FileHelper
    {
        private static readonly byte[] JPGORJPEG = { 255, 216, 255 };
        private static readonly byte[] BMP = { 66, 77 };
        private static readonly byte[] PDF = { 37, 80, 68, 70, 45, 49, 46 };
        private static readonly byte[] DOC = { 208, 207, 17, 224, 161, 177, 26, 225 };
        private static readonly byte[] DOCXORXLSXORPPTX = { 80, 75, 3, 4, 20 };
        private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
        private static readonly byte[] XLS = { 208, 207, 17, 224, 161, 177, 26, 225, 0 };
        private static readonly byte[] MP3 = { 255, 251, 48 };
        private static readonly byte[] MP4 = { 0, 0, 0, 32 };

        private static readonly Dictionary<string, byte[]> AllowedExtensions = new Dictionary<string, byte[]>();

        static FileHelper()
        {
            SetupExtension();
        }

        private static void SetupExtension()
        {
            AllowedExtensions.Add("JPG", JPGORJPEG);
            AllowedExtensions.Add("JPEG", JPGORJPEG);
            AllowedExtensions.Add("BMP", BMP);
            AllowedExtensions.Add("PNG", PNG);
            AllowedExtensions.Add("PDF", PDF);
            AllowedExtensions.Add("DOC", DOC);
            AllowedExtensions.Add("XLS", XLS);
            AllowedExtensions.Add("DOCX", DOCXORXLSXORPPTX);
            AllowedExtensions.Add("XLSX", DOCXORXLSXORPPTX);
            AllowedExtensions.Add("PPTX", DOCXORXLSXORPPTX);
            AllowedExtensions.Add("MP4", MP4);
            AllowedExtensions.Add("MP3", MP3);
        }

        public static string PrepareExtensions(string extensions)
        {
            if (string.IsNullOrEmpty(extensions))
                return extensions;

            return extensions.Trim().Replace('*', ',').Replace(';', ',').Replace(' ', ',').Replace('.', ',');
        }

        public static bool IsValidFileStreamWithExtension(string fileName, byte[] content, bool validateContent = true)
        {
            bool isvalid = false;

            if (string.IsNullOrWhiteSpace(fileName))
                return isvalid;

            var ext = Path.GetExtension(fileName).Replace(".", "");

            if (string.IsNullOrWhiteSpace(ext))
                return isvalid;

            return IsExtensionValid(content, ext, validateContent);
        }

        public static bool IsValidFile(byte[] content, string extension, bool validateContent = true)
        {
            return IsExtensionValid(content, extension, validateContent);
        }

        public static bool IsValidFile(IFormFile file, string extension, bool validateContent = true)
        {
            if (file == null || file.Length == 0)
                return false;
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();

            return IsExtensionValid(fileBytes, extension, validateContent);

        }

        private static bool IsExtensionValid(byte[] content, string ext, bool validateContent)
        {
            var isValid = false;

            ext = ext.ToUpper();

            if (AllowedExtensions.ContainsKey(ext))
            {
                var item = AllowedExtensions[ext];

                if (validateContent)
                    isValid = content.Take(item.Length).SequenceEqual(item);
                else
                    isValid = true;
            }

            return isValid;
        }

    }
}
