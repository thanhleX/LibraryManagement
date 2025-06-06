namespace LibraryManagement.Application.DTOs
{
    public class EbookInfoDto
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string DownloadUrl { get; set; }
        public string Format { get; set; }
        public long Size { get; set; }
        public string SizeFormatted => FormatFileSize(Size);

        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            double size = bytes;
            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }
            return $"{size:0.##} {sizes[order]}";
        }
    }
} 