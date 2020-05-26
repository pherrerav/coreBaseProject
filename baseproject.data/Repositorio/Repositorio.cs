using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace baseproject.data.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly MyDbContext Context;
        public DbSet<T> table;

        public Repositorio(MyDbContext context)
        {
            this.Context = context;
            table = Context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            Context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Save()
        {
            Context.SaveChanges();
        }
    }
}
