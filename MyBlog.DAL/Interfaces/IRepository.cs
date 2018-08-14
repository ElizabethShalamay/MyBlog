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
        IEnumerable<T> Get(Func<T, bool> predicate); // TODO: ???
        IEnumerable<T> Get();
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
