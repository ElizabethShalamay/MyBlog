using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Test.Context
{
    class TagRepository : IRepository<Tag>
    {
        List<Tag> list;
        public TagRepository()
        {
            list = new List<Tag>();
        }
        public TagRepository(IEnumerable<Tag> items)
        {
            list = items.ToList();
        }

        public int Create(Tag entity)
        {
            if (list.Where(item => item.Name == entity.Name).Count() == 0
                && list.Where(item => item.Id == entity.Id).Count() == 0)
            {
                list.Add(entity);
                return 1;
            }
            return 0;
        }

        public int Delete(int id)
        {
            Tag item = list.Where(t => t.Id == id).FirstOrDefault();
            if (item == null)
                return 0;
            list.Remove(item);
            return 1;

        }

        public Tag Get(int id)
        {
            return list.Where(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<Tag> Get(Func<Tag, bool> predicate)
        {
            return list.Where(predicate);
        }

        public IEnumerable<Tag> Get()
        {
            return list;
        }

        public int Update(Tag entity)
        {
            Tag item = list.Where(t => t.Id == entity.Id).FirstOrDefault();
            if (item == null)
                return 0;
            list.Remove(item);
            list.Add(entity);
            return 1;

        }
    }
}
