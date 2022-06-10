using AutoFixture;
using FluentAssertions;
using MAS.Core.Dtos.Incoming.Subject;
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

namespace MAS.Core.Test.Services;

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
    public async Task GetAllSubjectsAsync_ShouldReturnDataInContent_WhenDataFound()
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
        result.Content.Should().NotBeEmpty();
        _subjectRepositoryMock.Verify(x => x.GetAllSubjectsAsync(subjectParamMock), Times.Once());
    }

    [Fact]
    public async Task GetAllSubjectsAsync_ShouldReturnEmptyContent_WhenDataNotFound()
    {
        // Arrange
        var subjectsMock = _fixture.Create<PagedResult<SubjectResponse>>();
        subjectsMock.Content.Clear();
        var subjectParamMock = _fixture.Create<SubjectParameters>();
        _subjectRepositoryMock.Setup(x => x.GetAllSubjectsAsync(subjectParamMock)).ReturnsAsync(subjectsMock);

        // Act
        var result = await _subjectService.GetAllSubjectsAsync(subjectParamMock).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PagedResult<SubjectResponse>>();
        result.Content.Should().BeEmpty();
        _subjectRepositoryMock.Verify(x => x.GetAllSubjectsAsync(subjectParamMock), Times.Once());
    }

    [Fact]
    public async Task GetSubjectByIdAsync_ShouldReturnDataInContent_WhenDataFound()
    {
        // Arrange
        var subjectId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<SubjectDetailResponse>>();
        resultResponse.Error = null;
        _subjectRepositoryMock.Setup(x => x.GetSubjectByIdAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _subjectService.GetSubjectByIdAsync(subjectId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<SubjectDetailResponse>>();
        result.Content.Should().NotBeNull();
        result.Content.Should().BeAssignableTo<SubjectDetailResponse>();
        _subjectRepositoryMock.Verify(x => x.GetSubjectByIdAsync(subjectId), Times.Once());
    }

    [Fact]
    public async Task GetSubjectByIdAsync_ShouldReturnNullInContent_WhenDataNotFound()
    {
        // Arrange
        var subjectId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<SubjectDetailResponse>>();
        resultResponse.Content = null;
        _subjectRepositoryMock.Setup(x => x.GetSubjectByIdAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _subjectService.GetSubjectByIdAsync(subjectId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<SubjectDetailResponse>>();
        result.Content.Should().BeNull();
        result.Error.Should().NotBeNull();
        _subjectRepositoryMock.Verify(x => x.GetSubjectByIdAsync(subjectId), Times.Once());
    }

    [Fact]
    public async Task GetSubjectByIdAsync_ShouldThrowArgumentNullException_WhenInputSubjectIdNull()
    {
        // Arrange
        string subjectId = null;
        Result<SubjectDetailResponse> resultResponse = null;
        _subjectRepositoryMock.Setup(x => x.GetSubjectByIdAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<SubjectDetailResponse>> result = () => _subjectService.GetSubjectByIdAsync(subjectId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GetSubjectByIdAsync_ShouldThrowArgumentNullException_WhenInputSubjectIdEmpty()
    {
        // Arrange
        string subjectId = string.Empty;
        Result<SubjectDetailResponse> resultResponse = null;
        _subjectRepositoryMock.Setup(x => x.GetSubjectByIdAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<SubjectDetailResponse>> result = () => _subjectService.GetSubjectByIdAsync(subjectId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GetSubjectByIdAsync_ShouldThrowArgumentNullException_WhenInputSubjectIdIsWhitespace()
    {
        // Arrange
        string subjectId = " ";
        Result<SubjectDetailResponse> resultResponse = null;
        _subjectRepositoryMock.Setup(x => x.GetSubjectByIdAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<SubjectDetailResponse>> result = () => _subjectService.GetSubjectByIdAsync(subjectId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateSubjectAsync_ShouldReturnData_WhenDataIsCreatedSucessfully()
    {
        // Arrange
        var subjectCreateRequest = _fixture.Create<SubjectCreateRequest>();
        var resultResponse = _fixture.Create<Result<SubjectResponse>>();
        resultResponse.Error = null;
        _subjectRepositoryMock.Setup(x => x.CreateSubjectAsync(subjectCreateRequest)).ReturnsAsync(resultResponse);

        // Act
        var result = await _subjectService.CreateSubjectAsync(subjectCreateRequest).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<SubjectResponse>>();
        result.Content.Should().NotBeNull();
        result.Content.Should().BeAssignableTo<SubjectResponse>();
        _subjectRepositoryMock.Verify(x => x.CreateSubjectAsync(subjectCreateRequest), Times.Once());
    }

    [Fact]
    public async Task CreateSubjectAsync_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
        // Arrange
        SubjectCreateRequest subjectCreateRequest = null;
        Result<SubjectResponse> resultResponse = null;
        _subjectRepositoryMock.Setup(x => x.CreateSubjectAsync(subjectCreateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<SubjectResponse>> result = () => _subjectService.CreateSubjectAsync(subjectCreateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteSubjectAsync_ShouldReturnContentTrue_WhenDataIsDeletedSucessfully()
    {
        // Arrange
        var subjectId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Error = null;
        resultResponse.Content = true;
        _subjectRepositoryMock.Setup(x => x.DeleteSubjectAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _subjectService.DeleteSubjectAsync(subjectId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeTrue();
        _subjectRepositoryMock.Verify(x => x.DeleteSubjectAsync(subjectId), Times.Once());
    }

    [Fact]
    public async Task DeleteSubjectAsync_ShouldReturnContentFalse_WhenDataNotFound()
    {
        // Arrange
        var subjectId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _subjectRepositoryMock.Setup(x => x.DeleteSubjectAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _subjectService.DeleteSubjectAsync(subjectId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeFalse();
        _subjectRepositoryMock.Verify(x => x.DeleteSubjectAsync(subjectId), Times.Once());
    }

    [Fact]
    public async Task DeleteSubjectAsync_ShouldThrowArgumentNullException_WhenInputSubjectIdIsNull()
    {
        // Arrange
        string subjectId = null;
        Result<bool> resultResponse = null;
        _subjectRepositoryMock.Setup(x => x.DeleteSubjectAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _subjectService.DeleteSubjectAsync(subjectId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteSubjectAsync_ShouldThrowArgumentNullException_WhenInputSubjectIdIsEmpty()
    {
        // Arrange
        string subjectId = string.Empty;
        Result<bool> resultResponse = null;
        _subjectRepositoryMock.Setup(x => x.DeleteSubjectAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _subjectService.DeleteSubjectAsync(subjectId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteSubjectAsync_ShouldThrowArgumentNullException_WhenInputSubjectIdIsWhitespace()
    {
        // Arrange
        string subjectId = " ";
        Result<bool> resultResponse = null;
        _subjectRepositoryMock.Setup(x => x.DeleteSubjectAsync(subjectId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _subjectService.DeleteSubjectAsync(subjectId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateSubjectAsync_ShoudReturnContentTrue_WhenDataUpdateSucessfully()
    {
        // Arrange
        var subjectId = _fixture.Create<string>();
        var subjectUpdateRequest = _fixture.Create<SubjectUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Error = null;
        resultResponse.Content = true;
        _subjectRepositoryMock.Setup(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        var result = await _subjectService.UpdateSubjectAsync(subjectId, subjectUpdateRequest).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeTrue();
        _subjectRepositoryMock.Verify(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest), Times.Once());
    }

    [Fact]
    public async Task UpdateSubjectAsync_ShoudReturnContentFalse_WhenDataIsNotFound()
    {
        // Arrange
        var subjectId = _fixture.Create<string>();
        var subjectUpdateRequest = _fixture.Create<SubjectUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Error = null;
        resultResponse.Content = false;
        _subjectRepositoryMock.Setup(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        var result = await _subjectService.UpdateSubjectAsync(subjectId, subjectUpdateRequest).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeFalse();
        _subjectRepositoryMock.Verify(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest), Times.Once());
    }

    [Fact]
    public async Task UpdateSubjectAsync_ShouldThrowArgumentNullException_WhenInputRequestIsNull()
    {
        // Arrange
        var subjectId = _fixture.Create<string>();
        SubjectUpdateRequest subjectUpdateRequest = null;
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _subjectRepositoryMock.Setup(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _subjectService.UpdateSubjectAsync(subjectId, subjectUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateSubjectAsync_ShouldThrowArgumentNullException_WhenSubjectIdIsNull()
    {
        // Arrange
        string subjectId = null;
        var subjectUpdateRequest = _fixture.Create<SubjectUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _subjectRepositoryMock.Setup(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _subjectService.UpdateSubjectAsync(subjectId, subjectUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateSubjectAsync_ShouldThrowArgumentNullException_WhenSubjectIdIsEmpty()
    {
        // Arrange
        string subjectId = string.Empty;
        var subjectUpdateRequest = _fixture.Create<SubjectUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _subjectRepositoryMock.Setup(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _subjectService.UpdateSubjectAsync(subjectId, subjectUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateSubjectAsync_ShouldThrowArgumentNullException_WhenSubjectIdIsWhitespace()
    {
        // Arrange
        string subjectId = " ";
        var subjectUpdateRequest = _fixture.Create<SubjectUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _subjectRepositoryMock.Setup(x => x.UpdateSubjectAsync(subjectId, subjectUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _subjectService.UpdateSubjectAsync(subjectId, subjectUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }
}    
