using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces

{
    interface IRepository<T> where T : class
    {
        T Get(object id);
        IEnumerable<T> Get(Func<T, bool> predicate); // TODO: ???
        IEnumerable<T> Get();
        void Create(T entity);
        void Update(T entity);
        void Delete(object id);
    }
}
