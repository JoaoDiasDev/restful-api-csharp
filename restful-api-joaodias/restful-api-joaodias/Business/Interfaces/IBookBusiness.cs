using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Hypermedia.Utils;

namespace restful_api_joaodias.Business.Interfaces
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);
        BookVO FindByID(long id);
        PagedSearchVO<BookVO> FindWithPagedSearch(string? title, string sortDirection, int pageSize, int page);
        BookVO Update(BookVO book);
        void Delete(long id);
    }
}
