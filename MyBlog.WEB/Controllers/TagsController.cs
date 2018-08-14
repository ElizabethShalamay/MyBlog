using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyBlog.WEB.Controllers
{
    [Authorize(Roles = "User")]
    public class TagsController : ApiController
    {
        ITagService tagService;
        public TagsController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        [Route("api/tags")]
        public IEnumerable<TagViewModel> Get()
        {
            var tags = Mapper.Map<IEnumerable<TagDTO>, IEnumerable<TagViewModel>>(tagService.GetAll());
            return tags;
        }

        [Route("api/tags")]
        public void Post([FromBody] TagViewModel tag)
        {
            TagDTO tagDTO = Mapper.Map<TagViewModel, TagDTO>(tag);
            tagService.AddTag(tagDTO);
        }

        [Route("api/tags")]
        public void Put([FromBody] TagViewModel tag)
        {
            TagDTO tagDTO = Mapper.Map<TagViewModel, TagDTO>(tag);
            tagService.UpdateTag(tagDTO);
        }
    }
}
