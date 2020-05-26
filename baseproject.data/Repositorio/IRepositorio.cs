using System.Collections.Generic;

namespace baseproject.data.Repositorio
{
    public interface IRepositorio<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(int id);
        void Save();
    }
}
