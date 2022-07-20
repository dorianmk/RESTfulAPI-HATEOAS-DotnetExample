using AutoMapper;
using WebAPI.Dtos.Book;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class BookDetailsConverter : IOneWayConverter<Book, BookDetailsDto>
    {
        private readonly IMapper _mapper;

        public BookDetailsConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public BookDetailsDto Convert(Book from)
        {
            return _mapper.Map<Book, BookDetailsDto>(from);
        }
    }
}
