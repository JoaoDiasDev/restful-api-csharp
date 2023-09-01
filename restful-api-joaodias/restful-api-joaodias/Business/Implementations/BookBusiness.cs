using restful_api_joaodias.Business.Interfaces;
using restful_api_joaodias.Data.Converter.Implementations;
using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Model;
using restful_api_joaodias.Repository.Generic;

namespace restful_api_joaodias.Business.Implementations
{
    public class BookBusiness : IBookBusiness
    {
        private readonly BookConverter _converter;
        private readonly IRepository<Book> _repository;


        public BookBusiness(IRepository<Book> repository)
        {
            _repository = repository;
            _converter = new BookConverter();
        }

        public BookVO Create(BookVO book)
        {
            var entity = _converter.Parse(book);
            if (entity != null)
            {
                return _converter.Parse(_repository.Create(entity));
            }
            return null;
        }

        public void Delete(long id) { _repository.Delete(id); }

        public List<BookVO> FindAll() { return _converter.Parse(_repository.FindAll()); }

        public BookVO FindByID(long id) { return _converter.Parse(_repository.FindById(id)); }

        public BookVO Update(BookVO book)
        {
            var entity = _converter.Parse(book);
            if (entity != null)
            {
                return _converter.Parse(_repository.Update(entity));
            }
            return null;
        }
    }
}
