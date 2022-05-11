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

            //services.AddDbContext<AppDbContext>
            //(
            //    // optionsAction: options => options.UseSqlServer(playTogetherDbConnectionString)
            //    options => options.UseSqlServer(configuration.GetConnectionString("PlayTogetherDbConnection"))
            //);

            // Config for automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Config DI

            // ---------------------------------------------------------
            services.AddHttpClient();
            return services;
        }
    }
}
