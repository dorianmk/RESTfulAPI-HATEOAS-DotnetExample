using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos.Author
{
    public class AuthorDto : AuthorBaseDto
    {
        [Required]
        public long? Id { get; set; }
    }
}
