namespace WebAPI.Hateoas
{
    public interface ILinksDto
    {
        IEnumerable<LinkDto> Links { get; }

        void AddLink(LinkDto link);
    }
}
