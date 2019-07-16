using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace QuimAspCore
{
    public abstract class ModuledStartup
    {
        private readonly IHostingEnvironment _env;
        private readonly IEnumerable<IStartupModule> _modules;

        protected ModuledStartup(IHostingEnvironment env, params IStartupModule[] modules)
            : this(env, (IEnumerable<IStartupModule>)modules)
        {
        }

        protected ModuledStartup(IHostingEnvironment env, IEnumerable<IStartupModule> modules)
        {
            _env = env;
            _modules = modules;
        }

        // This method gets called by the runtime.
        // Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore(options =>
                {
                    options.ReturnHttpNotAcceptable = true;

                    foreach (var curr in _modules)
                    {
                        curr.ConfigureMvcOptions(_env, options);
                    }
                })
                .AddJsonFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            foreach (var curr in _modules)
            {
                curr.ConfigureServices(_env, services);
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
