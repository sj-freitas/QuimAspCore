using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Lib.AspNetCore
{
    public abstract class ModuledStartup
    {
        private readonly IEnumerable<IStartupModule> _modules;

        protected ModuledStartup(params IStartupModule[] modules)
            : this((IEnumerable<IStartupModule>)modules)
        {
        }

        protected ModuledStartup(IEnumerable<IStartupModule> modules)
        {
            _modules = modules;
        }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            foreach (var curr in _modules)
            {
                curr.ConfigureServices(services);
            }
        }

        // This method gets called by the runtime.
        // Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            foreach (var curr in _modules)
            {
                curr.Configure(app, env);
            }
        }
    }
}
