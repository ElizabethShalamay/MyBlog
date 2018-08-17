using MyBlog.BLL.Interfaces;
using MyBlog.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using MyBlog.BLL.DTO;
using AutoMapper;
using MyBlog.DAL.Entities;

namespace MyBlog.BLL.Services
{
    public class TagService : ITagService
    {
        IUnitOfWork db;

        public TagService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        IEnumerable<TagDTO> ITagService.GetAll()
        {
            var tags = db.TagManager.Get()
                .OrderByDescending(t => t.Posts.Count)
                .Take(20);
            return Mapper.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(tags);
        }

        bool ITagService.AddTag(TagDTO tagDTO)
        {
            if (tagDTO != null)
            {
                var tag = Mapper.Map<TagDTO, Tag>(tagDTO);
                int result = db.TagManager.Create(tag);
                return result > 0;
            }

            return false;
        }

        bool ITagService.UpdateTag(TagDTO tagDTO)
        {
            if (tagDTO != null)
            {
                var tag = Mapper.Map<TagDTO, Tag>(tagDTO);
                int result = db.TagManager.Update(tag, tag.Id);

                return result > 0;
            }

            return false;
        }
    }
}
