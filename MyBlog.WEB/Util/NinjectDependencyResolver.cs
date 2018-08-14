using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using Ninject;
using MyBlog.BLL.Interfaces;
using MyBlog.BLL.Services;
using Ninject.Syntax;
using System.Diagnostics.Contracts;
using System.Web.Http.Dependencies;
using Ninject.Activation;

namespace MyBlog.WEB.Util
{
    public class NinjectResolver : NinjectScope, IDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectResolver(IKernel kernel)
            : base(kernel)
        {
            _kernel = kernel;
        }
        public IDependencyScope BeginScope()
        {
            return new NinjectScope(_kernel.BeginBlock());
        }
    }

    public class NinjectScope : IDependencyScope
    {
        IKernel kernel;
        public NinjectScope(IKernel kernel)
        {
            this.kernel = kernel;
        }

        protected IResolutionRoot resolutionRoot;
        public NinjectScope(IResolutionRoot kernel)
        {
            resolutionRoot = kernel;
        }
        public object GetService(Type serviceType)
        {
            if (kernel == null)
                return resolutionRoot.TryGet(serviceType);
            else return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (kernel == null)
                return resolutionRoot.GetAll(serviceType);
            else return kernel.GetAll(serviceType);
        }
        public void Dispose()
        {
            IDisposable disposable = (IDisposable)resolutionRoot;
            if (disposable != null) disposable.Dispose();
            resolutionRoot = null;
        }
    }
}