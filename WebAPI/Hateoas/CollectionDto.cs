using System.Collections;

namespace WebAPI.Hateoas
{
    public class CollectionDto<T> : EntityDto
        where T : ILinksDto
    {
        private readonly List<T> _items;

        public CollectionDto(IEnumerable<T> items)
        {
            _items = new List<T>(items);
        }

        public IEnumerable<T> Embedded => _items;
    }
}
