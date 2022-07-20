using AutoMapper;
using WebAPI.Dtos.Author;
using WebAPI.Dtos.Book;
using WebAPI.Models;

namespace WebAPI.Converters
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorDetailsDto>().ReverseMap();
            CreateMap<NewAuthorDto, Author>();
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookDetailsDto>();
            CreateMap<NewBookDto, Book>();
        }
    }
}
