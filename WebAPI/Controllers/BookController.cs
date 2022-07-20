#nullable disable
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using WebAPI.Repositories;
using WebAPI.Hateoas;
using WebAPI.Interfaces;
using WebAPI.Dtos.Author;
using WebAPI.Dtos.Book;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _repository;
        private readonly IConverter<Book, BookDto> _bookConverter;
        private readonly IOneWayConverter<Book, BookDetailsDto> _bookDetailsConverter;
        private readonly IOneWayConverter<NewBookDto, Book> _newBookConverter;

        public BookController(
            IBookRepository repository,
            IConverter<Book, BookDto> bookConverter,
            IOneWayConverter<Book, BookDetailsDto> detailsConverter,
            IOneWayConverter<NewBookDto, Book> newBookConverter)
        {
            _repository = repository;
            _bookConverter = bookConverter;
            _bookDetailsConverter = detailsConverter;
            _newBookConverter = newBookConverter;
        }

        /// <summary>
        /// Returns all books
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CollectionDto<BookDto>>> GetBooks()
        {
            IEnumerable<BookDto> books = (await _repository.GetAll()).Select(x => _bookConverter.Convert(x));
            return new CollectionDto<BookDto>(books);
        }

        /// <summary>
        /// Returns a book details
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDetailsDto>> GetBookDetails(long id)
        {
            var book = await _repository.GetBookById(id);

            if (book == null)
                return NotFound();

            return _bookDetailsConverter.Convert(book);
        }

        /// <summary>
        /// Returns all book's authors
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns></returns>
        [HttpGet("{id}/authors")]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CollectionDto<AuthorDto>>> GetBookAuthors(long id)
        {
            var book = await _repository.GetBookById(id);

            if (book == null)
                return NotFound();

            return new CollectionDto<AuthorDto>(_bookDetailsConverter.Convert(book).Authors);
        }

        /// <summary>
        /// Updates existing book
        /// </summary>
        /// <param name="id">book id</param>
        /// <param name="book">actualized book</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBook(long id, BookDto book)
        {
            if (id != book.Id)
                return BadRequest("Book Id mismatch");

            ResultType updateResult = await _repository.Update(_bookConverter.Convert(book));

            if (updateResult == ResultType.NotFound)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Creates new book
        /// </summary>       
        /// <param name="book">new book</param>
        /// <returns></returns>
        [HttpPost]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookDto>> PostBook(NewBookDto book)
        {
            Book entity = _newBookConverter.Convert(book);
            ResultType createResult = await _repository.Create(entity);

            if (createResult == ResultType.AlreadyExists)
                return BadRequest("Book id already in use");

            return CreatedAtAction(nameof(GetBookDetails), new { id = entity.Id }, book);
        }

        /// <summary>
        /// Adds relation for existing book to existing author
        /// </summary>
        /// <param name="id">book id</param>
        /// <param name="authorId">author id</param>
        /// <returns></returns>
        [HttpPost("{id}/authors/{authorId}")]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookDto>> PostAuthor(long id, long authorId)
        {
            BookAuthorResultType addAuthorToBookResult = await _repository.AddAuthorToBook(id, authorId);

            if (addAuthorToBookResult != BookAuthorResultType.Ok)
                return BadRequest(addAuthorToBookResult.ToString());

            return CreatedAtAction(nameof(GetBookAuthors), new { id }, null);
        }

        /// <summary>
        /// Deletes existing book
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var result = await _repository.Delete(id);

            if (result == ResultType.NotFound)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Removes relation from existing book to existing author
        /// </summary>
        /// <param name="id">book id</param>
        /// <param name="authorId">author id</param>
        /// <returns></returns>
        [HttpDelete("{id}/authors/{authorId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(long id, long authorId)
        {
            BookAuthorResultType removeAuthorFromBookResult = await _repository.RemoveAuthorFromBook(id, authorId);

            if (removeAuthorFromBookResult != BookAuthorResultType.Ok)
                return NotFound(removeAuthorFromBookResult.ToString());

            return NoContent();
        }

    }
}
