using restful_api_joaodias.Business.Interfaces;
using restful_api_joaodias.Data.Converter.Implementations;
using restful_api_joaodias.Data.VO;
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
