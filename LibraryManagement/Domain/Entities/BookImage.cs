namespace LibraryManagement.Domain.Entities
{
    public class BookImage
    {
        public int Id { get; set; }
        public string PublicId { get; set; }  // Cloudinary public ID
        public string Url { get; set; }       // Cloudinary URL
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
} 