using AutoMapper;
using WebAPI.Dtos.Author;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class NewAuthorConverter : IOneWayConverter<NewAuthorDto, Author>
    {
        private readonly IMapper _mapper;

        public NewAuthorConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Author Convert(NewAuthorDto from)
        {
            return _mapper.Map<NewAuthorDto, Author>(from);
        }
    }
}
