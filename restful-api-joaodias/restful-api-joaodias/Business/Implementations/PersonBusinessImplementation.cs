using restful_api_joaodias.Business.Interfaces;
using restful_api_joaodias.Data.Converter.Implementations;
using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Hypermedia.Utils;
using restful_api_joaodias.Repository.PersonRepo;

namespace restful_api_joaodias.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly PersonConverter _converter;
        private readonly IPersonRepository _repository;

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public PersonVO Create(PersonVO person)
        {
            var entity = _converter.Parse(person);
            if (entity != null)
            {
                return _converter.Parse(_repository.Create(entity));
            }
            return null;
        }

        public void Delete(long id) { _repository.Delete(id); }

        public PersonVO Disable(long id)
        {
            var personEntity = _repository.Disable(id);
            return _converter.Parse(personEntity);
        }

        public List<PersonVO> FindAll() { return _converter.Parse(_repository.FindAll()); }

        public PersonVO FindById(long id) { return _converter.Parse(_repository.FindById(id)); }

        public List<PersonVO>? FindByName(string? firstName, string? lastName)
        { return _converter.Parse(_repository.FindByName(firstName, lastName)); }

        public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrEmpty(sortDirection)) && !(sortDirection.ToLower().Equals("desc")) ? "asc" : "desc";
            var size = (pageSize > 0) ? pageSize : 10;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"select * from person p where 1 = 1";
            if (!string.IsNullOrEmpty(name))
            {
                query += $"\n and p.first_name like '%{name.ToLower()}%' or p.last_name like '%{name.ToLower()}%'";
            }
            query += $"\n order by p.first_name {sort} limit {size} offset {offset}";

            string countQuery = @"select count(*) from person p where 1 = 1";
            if (!string.IsNullOrEmpty(name))
            {
                countQuery += $"\n and p.first_name like '%{name.ToLower()}%' or p.last_name like '%{name.ToLower()}%'";
            }
            var persons = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);
            return new PagedSearchVO<PersonVO>
            {
                CurrentPage = page,
                TotalResults = totalResults,
                SortDirections = sort,
                PageSize = size,
                List = _converter.Parse(persons)
            };
        }

        public PersonVO Update(PersonVO person)
        {
            var entity = _converter.Parse(person);
            if (entity != null)
            {
                return _converter.Parse(_repository.Update(entity));
            }
            return null;
        }
    }
}
