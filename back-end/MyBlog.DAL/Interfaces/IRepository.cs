using DAL.Entities;
using System;
using System.Collections.Generic;

namespace MyBlog.DAL.Interfaces
{
    public interface IRepository<T> where T : class,  IIdentical
    {
        T Get(int id);
        IEnumerable<T> Get(Func<T, bool> predicate);
        IEnumerable<T> Get();
        int Create(T entity);
        int Update(T entity);
        int Delete(int id);
    }
}
