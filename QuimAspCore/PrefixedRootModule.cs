using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lib.AspNetCore
{
    public class PrefixedRootModule : IStartupModule
    {
        private readonly IStartupModule _wrapped;
        private readonly string _prefix;

        public PrefixedRootModule(IStartupModule wrapped, string prefix)
        {
            _wrapped = wrapped;
            _prefix = prefix;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddMvcCore(opt =>
            {
                var assembly = _wrapped
                    .GetType()
                    .Assembly;
                var prefixedRoute = new PrefixedRoute(_prefix, assembly);

                opt.Conventions.Insert(0, prefixedRoute);
            });
            _wrapped.ConfigureServices(services);
        }
    }
}
