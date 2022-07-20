namespace WebAPI.Hateoas
{
    public class LinkDto
    {
        ///<example>https://localhost:80/example</example>
        public string Href { get; }
        ///<example>self</example>
        public string Rel { get; }
        ///<example>GET</example>
        public string Method { get; }

        public LinkDto(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
