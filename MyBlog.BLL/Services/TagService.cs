using MyBlog.BLL.Interfaces;
using MyBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.BLL.DTO;
using AutoMapper;
using MyBlog.DAL.Entities;

namespace MyBlog.BLL.Services
{
    public class TagService : ITagService
    {
        IUnitOfWork Db { get; set; }
        public TagService(IUnitOfWork unitOfWork)
        {
            Db = unitOfWork;
        }

        IEnumerable<TagDTO> ITagService.GetAll()
        {
            var tags = Db.TagManager.Get()
                .OrderByDescending(t => t.Posts.Count)
                .Take(20);
            return Mapper.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(tags);
        }

        void ITagService.AddTag(TagDTO tagDTO)
        {
            if (tagDTO != null)
            {
                var tag = Mapper.Map<TagDTO, Tag>(tagDTO);
                Db.TagManager.Create(tag);
                Db.SaveAsync();
            }
        }

        void ITagService.UpdateTag(TagDTO tagDTO)
        {
            if (tagDTO != null)
            {
                var tag = Mapper.Map<TagDTO, Tag>(tagDTO);
                Db.TagManager.Update(tag);
                Db.SaveAsync();
            }
        }
    }
}
