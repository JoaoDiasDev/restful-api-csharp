using restful_api_joaodias.Model.Base;

namespace restful_api_joaodias.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T item);

        void Delete(long id);

        bool Exists(long id);

        List<T> FindAll();

        T FindById(long id);

        T Update(T item);
        List<T> FindWithPagedSearch(string query);
        int GetCount(string query);
    }
}
