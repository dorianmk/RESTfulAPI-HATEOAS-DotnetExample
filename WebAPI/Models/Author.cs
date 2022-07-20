using System.ComponentModel.DataAnnotations;
using WebAPI.Interfaces;

namespace WebAPI.Models
{
    public class Author : IIdentifiable<long>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 2)]
        public string LastName { get; set; }

        public ICollection<Book>? Books { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
    }
}