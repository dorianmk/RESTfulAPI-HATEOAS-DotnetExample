using AutoMapper;
using WebAPI.Dtos.Author;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class AuthorConverter : IConverter<Author, AuthorDto>
    {
        private readonly IMapper _mapper;

        public AuthorConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public AuthorDto Convert(Author item)
        {
            return _mapper.Map<Author, AuthorDto>(item);
        }

        public Author Convert(AuthorDto item)
        {
            return _mapper.Map<AuthorDto, Author>(item);
        }
    }
}
