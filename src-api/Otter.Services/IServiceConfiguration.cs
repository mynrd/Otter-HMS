using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Services
{
    public interface IServiceConfiguration
    {
        void AddServices(IServiceCollection services, IConfiguration configuration);
    }
}
