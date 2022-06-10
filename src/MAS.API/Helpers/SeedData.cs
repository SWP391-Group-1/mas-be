using MAS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace MAS.API.Helpers;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>())) {
            dbContext.Database.EnsureCreated();

            if (!dbContext.Majors.Any()) {
                dbContext.Majors.AddRange(
                    new Core.Entities.Major {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        Code = "GD",
                        Title = "Graphics Design",
                        Description = "Learn to become a graphic designer."
                    },
                    new Core.Entities.Major {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        Code = "IA",
                        Title = "Information Assurance",
                        Description = "Learn more in how to security system."
                    },
                    new Core.Entities.Major {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        Code = "SE",
                        Title = "Software Engineering",
                        Description = "Learn more in how to design and develop software."
                    }
                );
            }
            dbContext.SaveChanges();

            if (!dbContext.Subjects.Any()) {
                // var gd = dbContext.Majors.FirstOrDefault(x => x.Title == "[GD] Graphics Design");
                // var ia = dbContext.Majors.FirstOrDefault(x => x.Title == "[IA] Information Assurance");
                var se = dbContext.Majors.FirstOrDefault(x => x.Code == "SE");
                dbContext.AddRange(
                    new Core.Entities.Subject {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        MajorId = se.Id,
                        Code = "SWP301",
                        Title = "Mini Capstone",
                        Description = ""
                    },
                    new Core.Entities.Subject {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        MajorId = se.Id,
                        Code = "PRX301",
                        Title = "Advanced XML for Java and Javascript",
                        Description = ""
                    },
                    new Core.Entities.Subject {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        MajorId = se.Id,
                        Code = "SWD302",
                        Title = "Software Design",
                        Description = ""
                    },
                    new Core.Entities.Subject {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        MajorId = se.Id,
                        Code = "SWR302",
                        Title = "Software Requirements",
                        Description = ""
                    },
                    new Core.Entities.Subject {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        MajorId = se.Id,
                        Code = "SWT301",
                        Title = "Software Testing",
                        Description = ""
                    },
                    new Core.Entities.Subject {
                        Id = Guid.NewGuid().ToString(),
                        CreateDate = DateTime.UtcNow.AddHours(7),
                        UpdateDate = null,
                        MajorId = se.Id,
                        Code = "PRO192",
                        Title = "Object-Oriented Programming",
                        Description = ""
                    }
                );
            }
            dbContext.SaveChanges();
        }
    }
}

