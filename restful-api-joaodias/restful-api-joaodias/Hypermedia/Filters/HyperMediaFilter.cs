using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace restful_api_joaodias.Hypermedia.Filters
{
    public class HyperMediaFilter : ResultFilterAttribute
    {
        private readonly HyperMediaFilterOptions _hyperMediaFilterOptions;

        public HyperMediaFilter(HyperMediaFilterOptions hyperMediaFilterOptions)
        { _hyperMediaFilterOptions = hyperMediaFilterOptions; }

        private void TryEnrichResponse(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult objectResult)
            {
                var enricher = _hyperMediaFilterOptions.ContentResponseEnricherList
                    .FirstOrDefault(crel => crel.CanEnrich(context));
                if (enricher != null)
                {
                    Task.FromResult(enricher?.Enrich(context));
                }
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            TryEnrichResponse(context);
            base.OnResultExecuting(context);
        }
    }
}
