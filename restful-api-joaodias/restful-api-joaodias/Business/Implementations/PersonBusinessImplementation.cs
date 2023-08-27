using restful_api_joaodias.Data.Converter.Implementations;
using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Model;
using restful_api_joaodias.Repository.Generic;

namespace restful_api_joaodias.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IRepository<Person> _repository;
        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IRepository<Person> repository)
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

        public List<PersonVO> FindAll() { return _converter.Parse(_repository.FindAll()); }

        public PersonVO FindById(long id) { return _converter.Parse(_repository.FindById(id)); }

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
