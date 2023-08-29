using restful_api_joaodias.Hypermedia.Abstract;

namespace restful_api_joaodias.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
