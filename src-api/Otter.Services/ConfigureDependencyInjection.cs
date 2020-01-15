using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Otter.Core;
using Otter.Data;
using Otter.Services.Account;
using Otter.Services.Company;
using Repository;
using Repository.Providers.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Otter.Services
{
    public class ConfigureDependencyInjection : IServiceConfiguration
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IDataContext, HMSContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IUserAccessService, UserAccessService>();
            services.AddTransient<ICompanyService, CompanyService>();
        }
    }
}