using AutoFixture;
using FluentAssertions;
using MAS.API.Controllers.V1;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Subject;
using MAS.Core.Interfaces.Services.Subject;
using MAS.Core.Parameters.Subject;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MAS.API.Test.Controllers.V1
{
    public class SubjectsControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<ISubjectService> _subjectServiceMock;
        private readonly SubjectsController _subjectController;

        public SubjectsControllerTest()
        {
            _fixture = new Fixture();
            _subjectServiceMock = _fixture.Freeze<Mock<ISubjectService>>();
            _subjectController = new SubjectsController(_subjectServiceMock.Object);
        }

        [Fact]
        public async Task GetAllSubjects_SholdReturnOkResponse_WhenDataFound()
        {
            // Arrange
            var subjectsMock = _fixture.Create<PagedResult<SubjectResponse>>();
            var subjectParamMock = _fixture.Create<SubjectParameters>();
            _subjectServiceMock.Setup(x => x.GetAllSubjectsAsync(subjectParamMock)).ReturnsAsync(subjectsMock);

            // Act
            var result = await _subjectController.GetAllSubjects(subjectParamMock).ConfigureAwait(false);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<ActionResult<PagedResult<SubjectResponse>>>();
            result.Result.Should().BeAssignableTo<OkObjectResult>();
            result.Result.As<OkObjectResult>().Value
                .Should().BeOfType(subjectsMock.GetType());
            _subjectServiceMock.Verify(x => x.GetAllSubjectsAsync(subjectParamMock), Times.Once());
        }
    }
}
