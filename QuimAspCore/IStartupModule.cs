using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace QuimAspCore
{
    public interface IStartupModule
    {
        void ConfigureServices(IHostingEnvironment env, IServiceCollection services);

        void Configure(IApplicationBuilder app, IHostingEnvironment env);

        void ConfigureMvcOptions(IHostingEnvironment env, MvcOptions options);
    }
}
