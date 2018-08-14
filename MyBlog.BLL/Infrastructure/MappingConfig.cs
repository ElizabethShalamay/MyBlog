using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyBlog.DAL.Entities;
using MyBlog.BLL.DTO;
using AutoMapper.Configuration;

namespace MyBlog.BLL.Infrastructure
{
    public class MappingConfig
    {       
        public static void InitializeMapper(MapperConfigurationExpression config)
        {
            config.CreateMap<User, UserDTO>();
            config.CreateMap<Post, PostDTO>().ForMember("Tags", t => t.MapFrom(src => src.Tags.Select(str => str.Name)));
            config.CreateMap<Comment, CommentDTO>();
            config.CreateMap<Tag, TagDTO>();

            config.CreateMap<UserDTO, User>();
            config.CreateMap<PostDTO, Post>();
            config.CreateMap<CommentDTO, Comment>();
            config.CreateMap<TagDTO, Tag>();

            Mapper.Initialize(config);

        }
    }
}
