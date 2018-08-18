using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Context
{
    class PostRepository : IRepository<Post>
    {
        List<Post> list;
        public PostRepository()
        {
            list = new List<Post>();
        }
        public PostRepository(IEnumerable<Post> items)
        {
            list = items.ToList();
        }

        public int Create(Post entity)
        {
            if (list.Where(item => item.Id == entity.Id).Count() == 0)
            {
                list.Add(entity);
                return 1;
            }
            return 0;
        }

        public int Delete(int id)
        {
            Post item = list.Where(t => t.Id == id).FirstOrDefault();
            if (item != null)
            {
                list.Remove(item);
                return 1;
            }
            return 0;
        }

        public Post Get(int id)
        {
            return list.Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<Post> Get(Func<Post, bool> predicate)
        {
            return list.Where(predicate);
        }

        public IEnumerable<Post> Get()
        {
            return list;
        }

        public int Update(Post entity)
        {
            Post item = list.Where(t => t.Id == entity.Id).FirstOrDefault();
            if (item != null)
            {
                list.Remove(item);
                list.Add(entity);
                return 1;
            }
            return 0;
        }
    }
}
