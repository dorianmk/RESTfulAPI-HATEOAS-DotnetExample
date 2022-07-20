using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Interfaces
{
    public interface IAuthorRepository : IRepository<long, Author, ResultType>
    {
        Task<Author?> GetAuthorById(long id);
        Task<BookAuthorResultType> AddBookToAuthor(long authorId, long bookId);
        Task<BookAuthorResultType> RemoveBookFromAuthor(long authorId, long bookId);
    }
}
