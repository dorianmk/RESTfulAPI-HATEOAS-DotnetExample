using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Book
{
    public class BookDto : BookBaseDto
    {
        [Required]
        public long? Id { get; set; }
    }
}
