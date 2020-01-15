using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otter.Api.ServiceConfiguration
{
    public interface IServiceCoreConfiguration
    {
        void AddServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment);
    }
}
