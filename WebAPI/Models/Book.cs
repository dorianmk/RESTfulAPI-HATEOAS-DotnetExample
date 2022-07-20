using System.ComponentModel.DataAnnotations;
using WebAPI.Interfaces;

namespace WebAPI.Models
{
    public class Book : IIdentifiable<long>
    {
        public long Id { get; set; }

        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        public ICollection<Author>? Authors { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }

    }
}
