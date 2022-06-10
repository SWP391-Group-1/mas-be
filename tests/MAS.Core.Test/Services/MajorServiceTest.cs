using AutoFixture;
using FluentAssertions;
using MAS.Core.Dtos.Incoming.Major;
using MAS.Core.Dtos.Outcoming.Generic;
using MAS.Core.Dtos.Outcoming.Major;
using MAS.Core.Interfaces.Repositories.Major;
using MAS.Core.Parameters.Major;
using MAS.Core.Services.Major;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MAS.Core.Test.Services;

public class MajorServiceTest
{
    private readonly IFixture _fixture;
    private readonly Mock<IMajorRepository> _majorRepositoryMock;
    private readonly Mock<ILogger<MajorService>> _loggerMock;
    private readonly MajorService _majorService;

    public MajorServiceTest()
    {
        _fixture = new Fixture();
        _majorRepositoryMock = _fixture.Freeze<Mock<IMajorRepository>>();
        _loggerMock = _fixture.Freeze<Mock<ILogger<MajorService>>>();
        _majorService = new MajorService(_majorRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllMajorsAsync_ShouldReturnDataInContent_WhenDataFound()
    {
        // Arrange
        var majorsMock = _fixture.Create<PagedResult<MajorResponse>>();
        var majorParamMock = _fixture.Create<MajorParameters>();
        _majorRepositoryMock.Setup(x => x.GetAllMajorsAsync(majorParamMock)).ReturnsAsync(majorsMock);

        // Act
        var result = await _majorService.GetAllMajorsAsync(majorParamMock).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PagedResult<MajorResponse>>();
        result.Content.Should().NotBeEmpty();
        _majorRepositoryMock.Verify(x => x.GetAllMajorsAsync(majorParamMock), Times.Once());
    }

    [Fact]
    public async Task GetAllMajorsAsync_ShouldReturnEmptyContent_WhenDataNotFound()
    {
        // Arrange
        var majorsMock = _fixture.Create<PagedResult<MajorResponse>>();
        majorsMock.Content.Clear();
        var majorParamMock = _fixture.Create<MajorParameters>();
        _majorRepositoryMock.Setup(x => x.GetAllMajorsAsync(majorParamMock)).ReturnsAsync(majorsMock);

        // Act
        var result = await _majorService.GetAllMajorsAsync(majorParamMock).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<PagedResult<MajorResponse>>();
        result.Content.Should().BeEmpty();
        _majorRepositoryMock.Verify(x => x.GetAllMajorsAsync(majorParamMock), Times.Once());
    }

    [Fact]
    public async Task GetMajorByIdAsync_ShouldReturnDataInContent_WhenDataFound()
    {
        // Arrange
        var majorId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<MajorResponse>>();
        resultResponse.Error = null;
        _majorRepositoryMock.Setup(x => x.GetMajorByIdAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _majorService.GetMajorByIdAsync(majorId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<MajorResponse>>();
        result.Content.Should().NotBeNull();
        result.Content.Should().BeAssignableTo<MajorResponse>();
        _majorRepositoryMock.Verify(x => x.GetMajorByIdAsync(majorId), Times.Once());
    }

    [Fact]
    public async Task GetMajorByIdAsync_ShouldReturnNullInContent_WhenDataNotFound()
    {
        // Arrange
        var majorId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<MajorResponse>>();
        resultResponse.Content = null;
        _majorRepositoryMock.Setup(x => x.GetMajorByIdAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _majorService.GetMajorByIdAsync(majorId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<MajorResponse>>();
        result.Content.Should().BeNull();
        result.Error.Should().NotBeNull();
        _majorRepositoryMock.Verify(x => x.GetMajorByIdAsync(majorId), Times.Once());
    }

    [Fact]
    public async Task GetMajorByIdAsync_ShouldThrowArgumentNullException_WhenInputMajorIdNull()
    {
        // Arrange
        string majorId = null;
        Result<MajorResponse> resultResponse = null;
        _majorRepositoryMock.Setup(x => x.GetMajorByIdAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<MajorResponse>> result = () => _majorService.GetMajorByIdAsync(majorId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GetMajorByIdAsync_ShouldThrowArgumentNullException_WhenInputMajorIdEmpty()
    {
        // Arrange
        string majorId = string.Empty;
        Result<MajorResponse> resultResponse = null;
        _majorRepositoryMock.Setup(x => x.GetMajorByIdAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<MajorResponse>> result = () => _majorService.GetMajorByIdAsync(majorId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task GetMajorByIdAsync_ShouldThrowArgumentNullException_WhenInputMajorIdIsWhitespace()
    {
        // Arrange
        string majorId = " ";
        Result<MajorResponse> resultResponse = null;
        _majorRepositoryMock.Setup(x => x.GetMajorByIdAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<MajorResponse>> result = () => _majorService.GetMajorByIdAsync(majorId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateMajorAsync_ShouldReturnData_WhenDataIsCreatedSucessfully()
    {
        // Arrange
        var majorCreateRequest = _fixture.Create<MajorCreateRequest>();
        var resultResponse = _fixture.Create<Result<MajorResponse>>();
        resultResponse.Error = null;
        _majorRepositoryMock.Setup(x => x.CreateMajorAsync(majorCreateRequest)).ReturnsAsync(resultResponse);

        // Act
        var result = await _majorService.CreateMajorAsync(majorCreateRequest).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<MajorResponse>>();
        result.Content.Should().NotBeNull();
        result.Content.Should().BeAssignableTo<MajorResponse>();
        _majorRepositoryMock.Verify(x => x.CreateMajorAsync(majorCreateRequest), Times.Once());
    }

    [Fact]
    public async Task CreateMajorAsync_ShouldThrowArgumentNullException_WhenInputIsNull()
    {
        // Arrange
        MajorCreateRequest majorCreateRequest = null;
        Result<MajorResponse> resultResponse = null;
        _majorRepositoryMock.Setup(x => x.CreateMajorAsync(majorCreateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<MajorResponse>> result = () => _majorService.CreateMajorAsync(majorCreateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteMajorAsync_ShouldReturnContentTrue_WhenDataIsDeletedSucessfully()
    {
        // Arrange
        var majorId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Error = null;
        resultResponse.Content = true;
        _majorRepositoryMock.Setup(x => x.DeleteMajorAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _majorService.DeleteMajorAsync(majorId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeTrue();
        _majorRepositoryMock.Verify(x => x.DeleteMajorAsync(majorId), Times.Once());
    }

    [Fact]
    public async Task DeleteMajorAsync_ShouldReturnContentFalse_WhenDataNotFound()
    {
        // Arrange
        var majorId = _fixture.Create<string>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _majorRepositoryMock.Setup(x => x.DeleteMajorAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        var result = await _majorService.DeleteMajorAsync(majorId).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeFalse();
        _majorRepositoryMock.Verify(x => x.DeleteMajorAsync(majorId), Times.Once());
    }

    [Fact]
    public async Task DeleteMajorAsync_ShouldThrowArgumentNullException_WhenInputMajorIdIsNull()
    {
        // Arrange
        string majorId = null;
        Result<bool> resultResponse = null;
        _majorRepositoryMock.Setup(x => x.DeleteMajorAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _majorService.DeleteMajorAsync(majorId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteMajorAsync_ShouldThrowArgumentNullException_WhenInputMajorIdIsEmpty()
    {
        // Arrange
        string majorId = string.Empty;
        Result<bool> resultResponse = null;
        _majorRepositoryMock.Setup(x => x.DeleteMajorAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _majorService.DeleteMajorAsync(majorId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task DeleteMajorAsync_ShouldThrowArgumentNullException_WhenInputMajorIdIsWhitespace()
    {
        // Arrange
        string majorId = " ";
        Result<bool> resultResponse = null;
        _majorRepositoryMock.Setup(x => x.DeleteMajorAsync(majorId)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _majorService.DeleteMajorAsync(majorId).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateMajorAsync_ShoudReturnContentTrue_WhenDataUpdateSucessfully()
    {
        // Arrange
        var majorId = _fixture.Create<string>();
        var majorUpdateRequest = _fixture.Create<MajorUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Error = null;
        resultResponse.Content = true;
        _majorRepositoryMock.Setup(x => x.UpdateMajorAsync(majorId, majorUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        var result = await _majorService.UpdateMajorAsync(majorId, majorUpdateRequest).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeTrue();
        _majorRepositoryMock.Verify(x => x.UpdateMajorAsync(majorId, majorUpdateRequest), Times.Once());
    }

    [Fact]
    public async Task UpdateMajorAsync_ShoudReturnContentFalse_WhenDataIsNotFound()
    {
        // Arrange
        var majorId = _fixture.Create<string>();
        var majorUpdateRequest = _fixture.Create<MajorUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Error = null;
        resultResponse.Content = false;
        _majorRepositoryMock.Setup(x => x.UpdateMajorAsync(majorId, majorUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        var result = await _majorService.UpdateMajorAsync(majorId, majorUpdateRequest).ConfigureAwait(false);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<Result<bool>>();
        result.Content.Should().BeFalse();
        _majorRepositoryMock.Verify(x => x.UpdateMajorAsync(majorId, majorUpdateRequest), Times.Once());
    }

    [Fact]
    public async Task UpdateMajorAsync_ShouldThrowArgumentNullException_WhenInputRequestIsNull()
    {
        // Arrange
        var majorId = _fixture.Create<string>();
        MajorUpdateRequest majorUpdateRequest = null;
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _majorRepositoryMock.Setup(x => x.UpdateMajorAsync(majorId, majorUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _majorService.UpdateMajorAsync(majorId, majorUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateMajorAsync_ShouldThrowArgumentNullException_WhenMajorIdIsNull()
    {
        // Arrange
        string majorId = null;
        var majorUpdateRequest = _fixture.Create<MajorUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _majorRepositoryMock.Setup(x => x.UpdateMajorAsync(majorId, majorUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _majorService.UpdateMajorAsync(majorId, majorUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateMajorAsync_ShouldThrowArgumentNullException_WhenMajorIdIsEmpty()
    {
        // Arrange
        string majorId = string.Empty;
        var majorUpdateRequest = _fixture.Create<MajorUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _majorRepositoryMock.Setup(x => x.UpdateMajorAsync(majorId, majorUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _majorService.UpdateMajorAsync(majorId, majorUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateMajorAsync_ShouldThrowArgumentNullException_WhenMajorIdIsWhitespace()
    {
        // Arrange
        string majorId = " ";
        var majorUpdateRequest = _fixture.Create<MajorUpdateRequest>();
        var resultResponse = _fixture.Create<Result<bool>>();
        resultResponse.Content = false;
        _majorRepositoryMock.Setup(x => x.UpdateMajorAsync(majorId, majorUpdateRequest)).ReturnsAsync(resultResponse);

        // Act
        Func<Result<bool>> result = () => _majorService.UpdateMajorAsync(majorId, majorUpdateRequest).Result;

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }
}
