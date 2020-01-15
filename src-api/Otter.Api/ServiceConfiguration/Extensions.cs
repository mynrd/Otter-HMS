using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otter.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otter.Api.ServiceConfiguration
{
    public static class Extensions
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            typeof(BaseService).Assembly.DefinedTypes
                           .Where(x => typeof(IServiceConfiguration)
                           .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                           .Select(Activator.CreateInstance)
                           .Cast<IServiceConfiguration>().ToList()
                           .ForEach(svc => svc.AddServices(services, configuration));
        }

        public static void AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            typeof(Startup).Assembly.DefinedTypes
                           .Where(x => typeof(IServiceCoreConfiguration)
                           .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                           .Select(Activator.CreateInstance)
                           .Cast<IServiceCoreConfiguration>().ToList()
                           .ForEach(svc => svc.AddServices(services, configuration, webHostEnvironment));
        }
    }
}
