using AutoFixture;
using FluentAssertions;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Interfaces.Repositories.Subject;
using MAS.Core.Parameters.Subject;
using MAS.Core.Services.Subject;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MAS.Core.Test.Services
{
    public class SubjectServiceTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<ISubjectRepository> _subjectRepositoryMock;
        private readonly Mock<ILogger<SubjectService>> _loggerMock;
        private readonly SubjectService _subjectService;

        public SubjectServiceTest()
        {
            _fixture = new Fixture();
            _subjectRepositoryMock = _fixture.Freeze<Mock<ISubjectRepository>>();
            _loggerMock = _fixture.Freeze<Mock<ILogger<SubjectService>>>();
            _subjectService = new SubjectService(_subjectRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllSubjects_ShouldReturnData_WhenDataFound()
        {
            // Arrange
            var subjectsMock = _fixture.Create<PagedResult<SubjectResponse>>();
            var subjectParamMock = _fixture.Create<SubjectParameters>();
            _subjectRepositoryMock.Setup(x => x.GetAllSubjectsAsync(subjectParamMock)).ReturnsAsync(subjectsMock);

            // Act
            var result = await _subjectService.GetAllSubjectsAsync(subjectParamMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PagedResult<SubjectResponse>>();
            _subjectRepositoryMock.Verify(x => x.GetAllSubjectsAsync(subjectParamMock), Times.Once());
        }
    }
}
