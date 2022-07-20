using System.ComponentModel.DataAnnotations;
using WebAPI.Hateoas;

namespace WebAPI.Dtos.Book
{
    public abstract class BookBaseDto : EntityDto
    {
        ///<example>Tytuł</example>
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        public DateTime? ReleaseDate { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal? Price { get; set; }
    }
}
