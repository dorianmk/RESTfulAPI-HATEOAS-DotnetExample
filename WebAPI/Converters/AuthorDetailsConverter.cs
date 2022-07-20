using AutoMapper;
using WebAPI.Dtos.Author;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class AuthorDetailsConverter : IOneWayConverter<Author, AuthorDetailsDto>
    {
        private readonly IMapper _mapper;

        public AuthorDetailsConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public AuthorDetailsDto Convert(Author from)
        {
            return _mapper.Map<Author, AuthorDetailsDto>(from);
        }
    }
}
