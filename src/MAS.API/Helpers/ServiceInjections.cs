using MAS.Core.Interfaces.Repositories.Account;
using MAS.Core.Interfaces.Repositories.Major;
using MAS.Core.Interfaces.Repositories.Subject;
using MAS.Core.Interfaces.Services.Account;
using MAS.Core.Interfaces.Services.Major;
using MAS.Core.Interfaces.Services.Subject;
using MAS.Core.Services.Account;
using MAS.Core.Services.Major;
using MAS.Core.Services.Subject;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Repositories.Account;
using MAS.Infrastructure.Repositories.Major;
using MAS.Infrastructure.Repositories.Subject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MAS.API.Helpers
{
    public static class ServiceInjections
    {
        public static IServiceCollection ConfigureServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null) {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null) {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.AddDbContext<AppDbContext>
            (
                options => options.UseSqlServer(configuration.GetConnectionString("MasDbConnection"))
            );

            // Config for automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Config DI--------------------------------------------------------------------------------

            // Config for Authentication
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // Config for Major
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IMajorRepository, MajorRepository>();

            // Config for Subject
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            // -----------------------------------------------------------------------------------------
            services.AddHttpClient();
            return services;
        }
    }
}
