using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Context
{
    class CommentRepository : IRepository<Comment>
    {
        List<Comment> list;
        public CommentRepository()
        {
            list = new List<Comment>();
        }
        public CommentRepository(IEnumerable<Comment> items)
        {
            list = items.ToList();
        }

        public int Create(Comment entity)
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
            Comment item = list.Where(t => t.Id == id).FirstOrDefault();
            if (item != null)
            {
                list.Remove(item);
                return 1;
            }
            return 0;
        }

        public Comment Get(int id)
        {
            return list.Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<Comment> Get(Func<Comment, bool> predicate)
        {
            return list.Where(predicate);
        }

        public IEnumerable<Comment> Get()
        {
            return list;
        }

        public int Update(Comment entity)
        {
            Comment item = list.Where(t => t.Id == entity.Id).FirstOrDefault();
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
