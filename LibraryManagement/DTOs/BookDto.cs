namespace LibraryManagement.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
