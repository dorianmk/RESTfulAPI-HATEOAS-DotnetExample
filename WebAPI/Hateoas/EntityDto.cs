namespace WebAPI.Hateoas
{
    public abstract class EntityDto : ILinksDto
    {
        private readonly List<LinkDto> _links;

        protected EntityDto()
        {
            _links = new List<LinkDto>();
        }

        public IEnumerable<LinkDto> Links => _links;

        public void AddLink(LinkDto link)
        {
            _links.Add(link);
        }
    }
}
