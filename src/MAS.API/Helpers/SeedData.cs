using MAS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MAS.API.Helpers
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>())) {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
