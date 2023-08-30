using restful_api_joaodias.Model;
using restful_api_joaodias.Repository.Generic;

namespace restful_api_joaodias.Repository.PersonRepo
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
    }
}
