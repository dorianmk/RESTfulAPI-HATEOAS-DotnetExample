using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class BookRepository : RepositoryBase<long, Book>, IBookRepository
    {

        public BookRepository(BookContext bookContext)
            : base(bookContext)
        {
        }

        public async Task<Book?> GetBookById(long id)
        {
            return await _dbContext.Set<Book>().Include(x => x.Authors).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<BookAuthorResultType> AddAuthorToBook(long bookId, long authorId)
        {
            return await DoAction(authorId, bookId, async (author, book) =>
            {
                if (book.Authors.Any(x => x.Id == authorId) == false)
                {
                    book.Authors.Add(author);
                    await _dbContext.SaveChangesAsync();
                }
            });
        }

        public async Task<BookAuthorResultType> RemoveAuthorFromBook(long bookId, long authorId)
        {
            return await DoAction(authorId, bookId, async (author, book) =>
            {
                if (book.Authors.Any(x => x.Id == authorId))
                {
                    book.Authors.Remove(author);
                    await _dbContext.SaveChangesAsync();
                }
            });
        }

        private async Task<BookAuthorResultType> DoAction(long authorId, long bookId, Action<Author, Book> action)
        {
            var author = await _dbContext.Set<Author>().FirstOrDefaultAsync(x => x.Id.Equals(authorId));

            if (author == null)
                return BookAuthorResultType.AuthorNotFound;

            var book = await _dbContext.Set<Book>().Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id.Equals(bookId));

            if (book == null)
                return BookAuthorResultType.BookNotFound;

            action(author, book);

            return BookAuthorResultType.Ok;
        }

    }
}
