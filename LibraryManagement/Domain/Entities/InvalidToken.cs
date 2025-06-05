using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Domain.Entities
{
    public class InvalidToken
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Token { get; set; }
        public DateTime ExpiratedAt { get; set; }
    }
}
