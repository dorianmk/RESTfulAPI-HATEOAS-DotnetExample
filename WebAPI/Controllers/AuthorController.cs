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
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _repository;
        private readonly IConverter<Author, AuthorDto> _authorConverter;
        private readonly IOneWayConverter<Author, AuthorDetailsDto> _authorDetailsConverter;
        private readonly IOneWayConverter<NewAuthorDto, Author> _newAuthorConverter;

        public AuthorController(
            IAuthorRepository repository,
            IConverter<Author, AuthorDto> authorConverter,
            IOneWayConverter<Author, AuthorDetailsDto> authorDetailsConverter,
            IOneWayConverter<NewAuthorDto, Author> newAuthorConverter)
        {
            _repository = repository;
            _authorConverter = authorConverter;
            _authorDetailsConverter = authorDetailsConverter;
            _newAuthorConverter = newAuthorConverter;
        }

        /// <summary>
        /// Returns all authors
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CollectionDto<AuthorDto>>> GetAuthors()
        {
            IEnumerable<AuthorDto> authors = (await _repository.GetAll()).Select(x => _authorConverter.Convert(x));
            return new CollectionDto<AuthorDto>(authors);
        }

        /// <summary>
        /// Returns an author details
        /// </summary>
        /// <param name="id">author id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AuthorDetailsDto>> GetAuthorDetails(long id)
        {
            var author = await _repository.GetAuthorById(id);

            if (author == null)
                return NotFound();

            return _authorDetailsConverter.Convert(author);
        }

        /// <summary>
        /// Returns all author's books
        /// </summary>
        /// <param name="id">author id</param>
        /// <returns></returns>
        [HttpGet("{id}/books")]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CollectionDto<BookDto>>> GetAuthorBooks(long id)
        {
            var author = await _repository.GetAuthorById(id);

            if (author == null)
                return NotFound();

            return new CollectionDto<BookDto>(_authorDetailsConverter.Convert(author).Books);
        }

        /// <summary>
        /// Updates existing author
        /// </summary>
        /// <param name="id">author id</param>
        /// <param name="author">actualized author</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAuthor(long id, AuthorDto author)
        {
            if (id != author.Id)
                return BadRequest("Author Id mismatch");

            ResultType updateResult = await _repository.Update(_authorConverter.Convert(author));

            if (updateResult == ResultType.NotFound)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Creates new author
        /// </summary>
        /// <param name="author">new author</param>
        /// <returns></returns>
        [HttpPost]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthorDto>> PostAuthor(NewAuthorDto author)
        {
            Author entity = _newAuthorConverter.Convert(author);
            ResultType createResult = await _repository.Create(entity);

            if (createResult == ResultType.AlreadyExists)
                return BadRequest("Author id already in use");

            return CreatedAtAction(nameof(GetAuthorDetails), new { id = entity.Id }, author);
        }

        /// <summary>
        /// Adds relation for existing author to existing book
        /// </summary>
        /// <param name="id">author id</param>
        /// <param name="bookId">book id</param>
        /// <returns></returns>
        [HttpPost("{id}/books/{bookId}")]
        [Produces(ContentTypeNames.Application.HalJson)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookDto>> PostBook(long id, long bookId)
        {
            BookAuthorResultType addBookToAuthorResult = await _repository.AddBookToAuthor(id, bookId);

            if (addBookToAuthorResult != BookAuthorResultType.Ok)
                return BadRequest(addBookToAuthorResult.ToString());

            return CreatedAtAction(nameof(GetAuthorBooks), new { id }, null);
        }

        /// <summary>
        /// Deletes existing author
        /// </summary>
        /// <param name="id">author id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(long id)
        {
            var result = await _repository.Delete(id);

            if (result == ResultType.NotFound)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Removes relation from existing author to existing book
        /// </summary>
        /// <param name="id">author id</param>
        /// <param name="bookId">book id</param>
        /// <returns></returns>
        [HttpDelete("{id}/books/{bookId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(long id, long bookId)
        {
            BookAuthorResultType removeBookFromAuthorResult = await _repository.RemoveBookFromAuthor(id, bookId);

            if (removeBookFromAuthorResult != BookAuthorResultType.Ok)
                return NotFound(removeBookFromAuthorResult.ToString());

            return NoContent();
        }

    }
}
