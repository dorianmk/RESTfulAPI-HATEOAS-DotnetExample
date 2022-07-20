using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class AuthorRepository : RepositoryBase<long, Author>, IAuthorRepository
    {

        public AuthorRepository(BookContext bookContext)
            : base(bookContext)
        {
        }

        public async Task<Author?> GetAuthorById(long id)
        {
            return await _dbContext.Set<Author>().Include(x => x.Books).AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<BookAuthorResultType> AddBookToAuthor(long authorId, long bookId)
        {
            return await DoAction(authorId, bookId, async (author, book) =>
              {
                  if (author.Books.Any(x => x.Id == bookId) == false)
                  {
                      author.Books.Add(book);
                      await _dbContext.SaveChangesAsync();
                  }
              });
        }

        public async Task<BookAuthorResultType> RemoveBookFromAuthor(long authorId, long bookId)
        {
            return await DoAction(authorId, bookId, async (author, book) =>
            {
                if (author.Books.Any(x => x.Id == bookId))
                {
                    author.Books.Remove(book);
                    await _dbContext.SaveChangesAsync();
                }
            });
        }

        private async Task<BookAuthorResultType> DoAction(long authorId, long bookId, Action<Author, Book> action)
        {
            var book = await _dbContext.Set<Book>().FirstOrDefaultAsync(x => x.Id.Equals(bookId));

            if (book == null)
                return BookAuthorResultType.BookNotFound;

            var author = await _dbContext.Set<Author>().Include(x => x.Books).FirstOrDefaultAsync(x => x.Id.Equals(authorId));

            if (author == null)
                return BookAuthorResultType.AuthorNotFound;

            action(author, book);

            return BookAuthorResultType.Ok;
        }


    }
}
