using restful_api_joaodias.Business.Interfaces;
using restful_api_joaodias.Data.Converter.Implementations;
using restful_api_joaodias.Data.VO;
using restful_api_joaodias.Hypermedia.Utils;
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

        public PagedSearchVO<BookVO> FindWithPagedSearch(
            string? title, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"select * from books p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(title)) query = query + $" and p.title like '%{title}%' ";
            query += $" order by p.title {sort} limit {size} offset {offset}";

            string countQuery = @"select count(*) from books p where 1 = 1 ";
            if (!string.IsNullOrWhiteSpace(title)) countQuery = countQuery + $" and p.title like '%{title}%' ";

            var books = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<BookVO>
            {
                CurrentPage = page,
                List = _converter.Parse(books),
                PageSize = size,
                SortDirections = sort,
                TotalResults = totalResults
            };
        }

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
