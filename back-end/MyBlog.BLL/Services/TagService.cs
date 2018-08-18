using MyBlog.BLL.Interfaces;
using MyBlog.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using MyBlog.BLL.DTO;
using AutoMapper;
using MyBlog.DAL.Entities;

namespace MyBlog.BLL.Services
{
    /// <summary>
    /// Service for working with tags : reading, addind, editing
    /// </summary>
    public class TagService : ITagService
    {
        IUnitOfWork db;

        public TagService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        /// <summary>
        /// Return list of most used tags
        /// </summary>
        /// <returns>List of tag DTOs</returns>
        IEnumerable<TagDTO> ITagService.GetTop()
        {
            IEnumerable<Tag> tags = db.TagManager.Get()
                .OrderByDescending(t => t.Posts.Count)
                .Take(20);
            return Mapper.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(tags);
        }

        IEnumerable<TagDTO> ITagService.GetAll()
        {
            IEnumerable<Tag> tags = db.TagManager.Get();
            return Mapper.Map<IEnumerable<Tag>, IEnumerable<TagDTO>>(tags);
        }

        /// <summary>
        /// Add a new tag
        /// </summary>
        /// <param name="tagDTO">Tag</param>
        /// <returns>Boolean value of operatin success</returns>
        bool ITagService.AddTag(TagDTO tagDTO)
        {
            if (tagDTO == null || db.TagManager.Get(t => t.Name == tagDTO.Name).FirstOrDefault() != null)
                return false;

            Tag tag = Mapper.Map<Tag>(tagDTO);
            int result = db.TagManager.Create(tag);
            return result > 0;
        }

        /// <summary>
        /// Edit an existing tag
        /// </summary>
        /// <param name="tagDTO">Tag</param>
        /// <returns>Boolean value of operatin success</returns>
        bool ITagService.UpdateTag(TagDTO tagDTO)
        {
            if (tagDTO == null)
                return false;

            Tag tag = Mapper.Map<Tag>(tagDTO);
            int result = db.TagManager.Update(tag);

            return result > 0;
        }
    }
}
