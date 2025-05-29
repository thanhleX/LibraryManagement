namespace LibraryManagement.Domain.Entities
{
    public class InvalidToken
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiratedAt { get; set; }
    }
}
