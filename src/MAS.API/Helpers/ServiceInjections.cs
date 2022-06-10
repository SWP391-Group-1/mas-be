using MAS.Core.Interfaces.Repositories.Account;
using MAS.Core.Interfaces.Repositories.Appointment;
using MAS.Core.Interfaces.Repositories.Email;
using MAS.Core.Interfaces.Repositories.Major;
using MAS.Core.Interfaces.Repositories.MasUser;
using MAS.Core.Interfaces.Repositories.MentorSubject;
using MAS.Core.Interfaces.Repositories.Question;
using MAS.Core.Interfaces.Repositories.Slot;
using MAS.Core.Interfaces.Repositories.Subject;
using MAS.Core.Interfaces.Services.Account;
using MAS.Core.Interfaces.Services.Appointment;
using MAS.Core.Interfaces.Services.Email;
using MAS.Core.Interfaces.Services.Major;
using MAS.Core.Interfaces.Services.MasUser;
using MAS.Core.Interfaces.Services.MentorSubject;
using MAS.Core.Interfaces.Services.Question;
using MAS.Core.Interfaces.Services.Slot;
using MAS.Core.Interfaces.Services.Subject;
using MAS.Core.Services.Account;
using MAS.Core.Services.Appointment;
using MAS.Core.Services.Email;
using MAS.Core.Services.Major;
using MAS.Core.Services.MasUser;
using MAS.Core.Services.MentorSubject;
using MAS.Core.Services.Question;
using MAS.Core.Services.Slot;
using MAS.Core.Services.Subject;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Repositories.Account;
using MAS.Infrastructure.Repositories.Appointment;
using MAS.Infrastructure.Repositories.Email;
using MAS.Infrastructure.Repositories.Major;
using MAS.Infrastructure.Repositories.MasUser;
using MAS.Infrastructure.Repositories.MentorSubject;
using MAS.Infrastructure.Repositories.Question;
using MAS.Infrastructure.Repositories.Slot;
using MAS.Infrastructure.Repositories.Subject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MAS.API.Helpers;

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

        // Config for MentorSubject
        services.AddScoped<IMentorSubjectService, MentorSubjectService>();
        services.AddScoped<IMentorSubjectRepository, MentorSubjectRepository>();

        // Config for MasUser
        services.AddScoped<IMasUserService, MasUserService>();
        services.AddScoped<IMasUserRepository, MasUserRepository>();

        // Config for Question
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();

        // Config for Email
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailRepository, EmailRepository>();

        // Config for Slot
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<ISlotRepository, SlotRepository>();

        // Config for Appointment
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        // -----------------------------------------------------------------------------------------
        services.AddHttpClient();
        return services;
    }
}
