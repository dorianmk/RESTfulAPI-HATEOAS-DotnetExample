namespace WebAPI.Hateoas
{
    public interface ILinksFactory
    {
        void CreateLinks(ILinksDto linksDto);
    }
}
