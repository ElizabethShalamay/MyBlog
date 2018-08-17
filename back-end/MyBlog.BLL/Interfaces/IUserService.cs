using Microsoft.AspNet.Identity;
using MyBlog.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin;

namespace MyBlog.BLL.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(UserDTO userDTO, string password);
        Task<ClaimsIdentity> GenerateUserIdentityAsync(IOwinContext context, string login, string password);

        UserDTO GetUserById(string id);
        UserDTO GetUserByName(string userName);
        IEnumerable<UserDTO> GetUsers(int page);
    }
}
