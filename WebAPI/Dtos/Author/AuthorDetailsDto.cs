using WebAPI.Dtos.Book;

namespace WebAPI.Dtos.Author
{
    public class AuthorDetailsDto : AuthorDto
    {
        public List<BookDto> Books { get; set; }
    }
}
