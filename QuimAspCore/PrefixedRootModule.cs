using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace QuimAspCore
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

            _wrapped.Configure(app, env);
        }

        public void ConfigureMvcOptions(IHostingEnvironment env, MvcOptions options)
        {
            var assembly = _wrapped
                .GetType()
                .Assembly;
            var prefixedRoute = new PrefixedRoute(_prefix, assembly);

            options.Conventions.Insert(0, prefixedRoute);

            _wrapped.ConfigureMvcOptions(env, options);
        }

        public void ConfigureServices(IHostingEnvironment env, IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            _wrapped.ConfigureServices(env, services);
        }
    }
}
