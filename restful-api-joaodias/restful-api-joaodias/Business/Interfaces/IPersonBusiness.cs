using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Hypermedia.Utils;

namespace restful_api_joaodias.Business.Interfaces
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
        PersonVO FindById(long id);
        List<PersonVO>? FindByName(string? firstName, string? lastName);
        List<PersonVO> FindAll();
        PagedSearchVO<PersonVO> FindWithPagedSearch(string? name, string sortDirection, int pageSize, int page);
        PersonVO Update(PersonVO person);
        PersonVO Disable(long id);
        void Delete(long id);
    }
}
