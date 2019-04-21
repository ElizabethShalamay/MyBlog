using AutoMapper;
using AutoMapper.Configuration;
using MyBlog.BLL.DTO;
using MyBlog.WEB.Models;

namespace MyBlog.WEB.Infrastructure
{
    public class MapperConfig : Profile
    {
        public static void InitializeMapper(MapperConfigurationExpression config)
        {
            config.CreateMap<UserViewModel, UserDTO>()
                .ForMember(user => user.PasswordHash, opt => opt.Ignore())
                .ReverseMap();

            Mapper.Initialize(config);
        }
    }
}