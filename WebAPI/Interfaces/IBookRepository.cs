using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Interfaces
{
    public interface IBookRepository : IRepository<long, Book, ResultType>
    {
        Task<Book?> GetBookById(long id);
        Task<BookAuthorResultType> AddAuthorToBook(long bookId, long authorId);
        Task<BookAuthorResultType> RemoveAuthorFromBook(long bookId, long authorId);
    }
}
