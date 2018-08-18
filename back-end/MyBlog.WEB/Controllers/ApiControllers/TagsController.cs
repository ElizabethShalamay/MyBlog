using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.WEB.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace MyBlog.WEB.Controllers
{
    [Authorize]
    [RoutePrefix("api/tags")]
    public class TagsController : ApiController
    {
        ITagService tagService;
        public TagsController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public IHttpActionResult Get()
        {
            IEnumerable<TagViewModel> tags = Mapper.Map<IEnumerable<TagViewModel>>(tagService.GetTop());
            if (tags != null)
                return Ok(tags);
            return NotFound();
        }

        public IHttpActionResult Post([FromBody] TagViewModel tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TagDTO tagDTO = Mapper.Map<TagDTO>(tag);
            bool success = tagService.AddTag(tagDTO);

            if (success)
                return Ok();
            return BadRequest();
        }

        [Route("{id}")]
        public IHttpActionResult Put(int id, [FromBody] TagViewModel tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TagDTO tagDTO = Mapper.Map<TagDTO>(tag);
            bool success = tagService.UpdateTag(tagDTO);

            if (success)
                return Ok();
            return BadRequest();
        }
    }
}
