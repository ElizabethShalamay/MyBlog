using AutoMapper;
using MyBlog.BLL.DTO;
using MyBlog.BLL.Interfaces;
using MyBlog.DAL.Entities;
using MyBlog.DAL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin;
using MyBlog.DAL;
using Microsoft.AspNet.Identity.Owin;
using MyBlog.DAL.Repository;
using System;
using Microsoft.Owin.Security.OAuth;

namespace MyBlog.BLL.Services
{
    public class UserService : IUserService, IDisposable
    {
        IUnitOfWork db;
        public UserService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        async Task<IdentityResult> IUserService.CreateUser(UserDTO userDTO, string password)
        {
            User user = Mapper.Map<User>(userDTO);
            return await db.AppUserManager.CreateAsync(user, password);
        }        

        UserDTO IUserService.GetUserById(string id)
        {
            User user = db.AppUserManager.FindByIdAsync(id).Result;
            return Mapper.Map<UserDTO>(user);
        }

        UserDTO IUserService.GetUserByName(string userName)
        {
            User user = db.AppUserManager.FindByNameAsync(userName).Result;
            return Mapper.Map<UserDTO>(user);
        }

        IEnumerable<UserDTO> IUserService.GetUsers(int page)
        {
            IEnumerable<User> users = db.AppUserManager.Users.OrderBy(user => user.UserName)
                .Skip((page - 1) * 5).Take(5).ToList();
            return Mapper.Map<IEnumerable<UserDTO>>(users);
        }

        private User GetUser(string name)
        {
            var user = db.AppUserManager.FindByNameAsync(name);
            return user.Result;
        }

        public static UserService Create()
        {
            return new UserService(new UnitOfWork());
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IOwinContext context, string login, string password)
        {
            var userManager = context.GetUserManager<ApplicationUserManager>();
            User user = await userManager.FindAsync(login, password);

            if (user == null)
            {
                throw new Exception("The user name or password is incorrect.");
            }

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
               OAuthDefaults.AuthenticationType);

            return oAuthIdentity;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~UserService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
