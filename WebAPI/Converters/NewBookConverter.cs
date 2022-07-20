using AutoMapper;
using WebAPI.Dtos.Book;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class NewBookConverter : IOneWayConverter<NewBookDto, Book>
    {
        private readonly IMapper _mapper;

        public NewBookConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Book Convert(NewBookDto from)
        {
            return _mapper.Map<NewBookDto, Book>(from);
        }
    }
}
