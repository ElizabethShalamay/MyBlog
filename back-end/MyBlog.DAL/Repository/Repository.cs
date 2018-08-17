using MyBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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

        public int Create(T entity)
        {
            table.Add(entity);
            return blog.SaveChanges();
        }

        public int Update(T entity, int id)
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
            return blog.SaveChanges();
        }

        public int Delete(int id)
        {
            T entity = Get(id);
            table.Remove(entity);
            return blog.SaveChanges();

        }
    }
}
