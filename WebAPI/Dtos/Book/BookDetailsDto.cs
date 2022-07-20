using WebAPI.Dtos.Author;

namespace WebAPI.Dtos.Book
{
    public class BookDetailsDto : BookDto
    {
        public List<AuthorDto> Authors { get; set; }
    }
}
