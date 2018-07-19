using DAL.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity
{
    class BlogUserManager : UserManager<User>
    {
        public BlogUserManager(IUserStore<User> store) : base(store)
        {
        }
    }
}
