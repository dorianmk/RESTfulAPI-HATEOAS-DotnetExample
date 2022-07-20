using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPI.Hateoas;

namespace WebAPI.ActionFilters
{
    public class HateoasResultFilter : IActionFilter
    {
        private readonly ILinksFactory _linksFactory;

        public HateoasResultFilter(ILinksFactory linksFactory)
        {
            _linksFactory = linksFactory;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is not ObjectResult objectResult)
                return;

            if (objectResult.Value is not ILinksDto linksDto)
                return;

            _linksFactory.CreateLinks(linksDto);
        }
    }
}
