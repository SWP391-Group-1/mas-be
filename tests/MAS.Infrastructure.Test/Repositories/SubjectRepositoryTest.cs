using AutoFixture;
using AutoMapper;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Parameters.Subject;
using MAS.Infrastructure.Data;
using MAS.Infrastructure.Repositories.Subject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace MAS.Infrastructure.Test.Repositories;

public class SubjectRepositoryTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IMapper> _mapper;
    private readonly SubjectRepository _subjectRepository;

    public SubjectRepositoryTest()
    {
        _fixture = new Fixture();
        _mapper = _fixture.Freeze<Mock<IMapper>>();
        // Create DB Context
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                                      .AddEnvironmentVariables()
                                                      .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("MasDbConnection"));

        var context = new AppDbContext(optionsBuilder.Options);

        // Delete all existing Data in Database, and create new
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        _subjectRepository = new SubjectRepository(_mapper.Object, context);
    }

    [Fact]
    public async Task GetAllSubjectsAsync_ShouldReturnOneRowData_WhenCreateOneObjectSubject()
    {
        //await _subjectRepository.CreateSubjectAsync()


    }
}
