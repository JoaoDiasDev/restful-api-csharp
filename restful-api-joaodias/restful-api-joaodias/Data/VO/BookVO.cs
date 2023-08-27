using restful_api_joaodias.Model.Base;

namespace restful_api_joaodias.Data.VO
{
    public class BookVO : BaseEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public decimal Price { get; set; }

        public DateTime LaunchDate { get; set; }
    }
}
