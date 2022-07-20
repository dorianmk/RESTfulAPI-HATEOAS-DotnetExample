using AutoMapper;
using WebAPI.Dtos.Book;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class BookConverter : IConverter<Book, BookDto>
    {
        private readonly IMapper _mapper;

        public BookConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public BookDto Convert(Book item)
        {
            return _mapper.Map<Book, BookDto>(item);
        }

        public Book Convert(BookDto item)
        {
            return _mapper.Map<BookDto, Book>(item);
        }
    }
}
