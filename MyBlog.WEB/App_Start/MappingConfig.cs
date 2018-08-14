using AutoMapper;
using AutoMapper.Configuration;
using MyBlog.BLL.DTO;
using MyBlog.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.WEB.App_Start
{
    public class MappingConfig
    {
        public static MapperConfigurationExpression InitializeMapper()
        {
            var config = new MapperConfigurationExpression();

            config.CreateMap<PostViewModel, PostDTO>();
            config.CreateMap<CommentViewModel, CommentDTO>();
            config.CreateMap<TagViewModel, TagDTO>();
            config.CreateMap<PostDTO, PostViewModel>();
            config.CreateMap<CommentDTO, CommentViewModel>();
            config.CreateMap<TagDTO, TagViewModel>();

            return config;
        }
    }
}