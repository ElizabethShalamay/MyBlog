using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.DAL.Interfaces

{
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        IEnumerable<T> Get(Func<T, bool> predicate);
        IEnumerable<T> Get();
        int Create(T entity);
        int Update(T entity, int id);
        int Delete(int id);
    }
}
