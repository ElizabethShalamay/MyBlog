﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using MyBlog.DAL.Interfaces;
using MyBlog.DAL.Repository;

namespace MyBlog.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        string connectionString;
        public ServiceModule(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}