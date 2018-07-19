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
    class BlogRoleManager : RoleManager<Role>
    {
        public BlogRoleManager(RoleStore<Role> roleStore) : base(roleStore)
        { }
    }
}
