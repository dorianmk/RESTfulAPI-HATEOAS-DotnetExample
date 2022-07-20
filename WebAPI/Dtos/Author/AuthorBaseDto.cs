using System.ComponentModel.DataAnnotations;
using WebAPI.Hateoas;

namespace WebAPI.Dtos.Author
{
    public class AuthorBaseDto : EntityDto
    {
        ///<example>Jan</example>
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 2)]
        public string FirstName { get; set; }

        ///<example>Kowalski</example>
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 2)]
        public string LastName { get; set; }
    }
}
