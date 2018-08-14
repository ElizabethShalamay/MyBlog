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

        // Method for additional operations (filtration, ordering) using IQueryable
        //public IEnumerable<T> Get(
        //    Expression<Func<T, bool>> filter = null,
        //    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        //{
        //    IQueryable<T> query = table;

        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }

        //    if (orderBy != null)
        //    {
        //        return orderBy(query).ToList();
        //    }
        //    else
        //    {
        //        return query.ToList();
        //    }
        //}

        public IEnumerable<T> Get()
        {
            return table.AsEnumerable();
        }

        public void Create(T entity)
        {
            table.Add(entity);
            blog.SaveChanges();
        }

        public void Update(T entity)
        {
            blog.Entry(entity).State = EntityState.Modified;
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
