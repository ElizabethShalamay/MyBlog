using MyBlog.DAL.EF;
using MyBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Repository
{
    class Repository<T> : IRepository<T> where T : class
    {
        readonly BlogContext blog;
        DbSet<T> table;

        public Repository(BlogContext context)
        {
            blog = context;
            table = blog.Set<T>();
        }

        public T Get(int id)
        {
            return table.Find(id);
        }

        public IEnumerable<T> Get(Func<T, bool> predicate)
        {
            return table.Where(predicate).AsEnumerable();
        }

        public IEnumerable<T> Get()
        {
            return table.AsEnumerable();
        }

        public void Create(T entity)
        {
            table.Add(entity);
            blog.SaveChanges();
        }

        public void Update(T entity, int id)
        {
            T entry = Get(id);
            if(entry == null)
            {
                table.Add(entity);
            }
            else
            {
                blog.Entry(entry).CurrentValues.SetValues(entity);
            }
            blog.Entry(entry).State = EntityState.Modified;
            blog.SaveChanges();
        }

        public void Delete(int id)
        {
            T entity = Get(id);
            table.Remove(entity);
            blog.SaveChanges();

        }
    }
}
