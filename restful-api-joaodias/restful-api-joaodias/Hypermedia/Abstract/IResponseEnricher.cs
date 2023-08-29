using Microsoft.AspNetCore.Mvc.Filters;

namespace restful_api_joaodias.Hypermedia.Abstract
{
    public interface IResponseEnricher
    {
        bool CanEnrich(ResultExecutingContext context);
        Task Enrich(ResultExecutingContext context);
    }
}
