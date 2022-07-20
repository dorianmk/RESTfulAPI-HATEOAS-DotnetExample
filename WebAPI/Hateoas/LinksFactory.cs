using Microsoft.AspNetCore.Http.Extensions;
using WebAPI.Controllers;
using WebAPI.Dtos.Author;
using WebAPI.Dtos.Book;

namespace WebAPI.Hateoas
{
    public class LinksFactory : ILinksFactory
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly LinkGenerator _linkGenerator;

        public LinksFactory(IHttpContextAccessor accessor, LinkGenerator linkGenerator)
        {
            _accessor = accessor;
            _linkGenerator = linkGenerator;
        }

        public void CreateLinks(ILinksDto linksDto)
        {
            HttpRequest request = _accessor.HttpContext.Request;
            linksDto.AddLink(new LinkDto(request.GetEncodedUrl(), "self", request.Method));

            if (linksDto is CollectionDto<AuthorDto> authors)
            {
                foreach (var author in authors.Embedded)
                    AddLinks(author);
            }
            if (linksDto is CollectionDto<BookDto> books)
            {
                foreach (var book in books.Embedded)
                    AddLinks(book);
            }
            if (linksDto is AuthorDetailsDto authorDetails)
            {
                foreach (var book in authorDetails.Books)
                    AddLinks(book);
            }
            if (linksDto is BookDetailsDto bookDetails)
            {
                foreach (var author in bookDetails.Authors)
                    AddLinks(author);
            }
            if (linksDto is AuthorDto authorDto)
            {
                AddBooksLink(authorDto);
            }
            if (linksDto is BookDto bookDto)
            {
                AddAuthorsLink(bookDto);
            }

        }

        private void AddLinks(AuthorDto author)
        {
            AddSelfLink(author);
            AddBooksLink(author);
        }

        private void AddLinks(BookDto book)
        {
            AddSelfLink(book);
            AddAuthorsLink(book);
        }

        private void AddSelfLink(AuthorDto author)
        {
            string uri = _linkGenerator.GetUriByAction(
                 _accessor.HttpContext,
                action: nameof(AuthorController.GetAuthorDetails),
                controller: "Author",
                values: new { author.Id });
            var link = new LinkDto(uri, "self", "GET");
            author.AddLink(link);
        }

        private void AddSelfLink(BookDto book)
        {
            string uri = _linkGenerator.GetUriByAction(
                _accessor.HttpContext,
                action: nameof(BookController.GetBookDetails),
                controller: "Book",
                values: new { book.Id });
            var link = new LinkDto(uri, "self", "GET");
            book.AddLink(link);
        }

        private void AddBooksLink(AuthorDto author)
        {
            string uri = _linkGenerator.GetUriByAction(
                _accessor.HttpContext,
               action: nameof(AuthorController.GetAuthorBooks),
               controller: "Author",
               values: new { author.Id });
            var link = new LinkDto(uri, "books", "GET");
            author.AddLink(link);
        }

        private void AddAuthorsLink(BookDto book)
        {
            string uri = _linkGenerator.GetUriByAction(
                _accessor.HttpContext,
               action: nameof(BookController.GetBookAuthors),
               controller: "Book",
               values: new { book.Id });
            var link = new LinkDto(uri, "authors", "GET");
            book.AddLink(link);
        }


    }
}
