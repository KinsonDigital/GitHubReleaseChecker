// <copyright file="GitHubDataServiceTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Net;
using FluentAssertions;
using GitHubReleaseChecker;
using GitHubReleaseChecker.Exceptions;
using GitHubReleaseChecker.Models;
using GitHubReleaseChecker.Services;
using GitHubReleaseCheckerTests.Helpers;
using Moq;

namespace GitHubReleaseCheckerTests.Services;

/// <summary>
/// Tests the <see cref="GitHubDataService"/> class.
/// </summary>
public class GitHubDataServiceTests
{
    private readonly Mock<IHttpClient> mockHttpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubDataServiceTests"/> class.
    /// </summary>
    public GitHubDataServiceTests() => this.mockHttpClient = new Mock<IHttpClient>();

    #region Method Tests
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void OwnerExists_WithNullOrEmptyRepoOwner_ThrowsException(string repoOwner)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.OwnerExists(repoOwner);

        // Assert
        await act.Should().ThrowAsync<NullOrEmptyStringException>()
            .WithMessage($"The '{nameof(repoOwner)}' value cannot be null or empty.");
    }

    [Fact]
    public async void OwnerExists_WithNullDataResult_ReturnsFalse()
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string requestUri = $"users/{repoOwner}";
        MockRequestResult<OwnerInfoModel>(HttpStatusCode.OK, null, requestUri);

        var service = CreateService();

        // Act
        var actual = await service.OwnerExists(repoOwner);

        // Assert
        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, HttpStatusCode.OK, false)]
    [InlineData("", HttpStatusCode.OK, false)]
    [InlineData("other-owner", HttpStatusCode.BadRequest, false)]
    [InlineData("test-owner", HttpStatusCode.OK, true)]
    public async void OwnerExists_WhenInvoked_ReturnsCorrectResult(
        string login,
        HttpStatusCode statusCode,
        bool expectedResult)
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string requestUri = $"users/{repoOwner}";
        var model = new OwnerInfoModel() { Login = login, };

        MockRequestResult(statusCode, model, requestUri);

        var service = CreateService();

        // Act
        var actual = await service.OwnerExists(repoOwner);

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public async void OwnerExists_WhenInvokedMoreThanOnce_CachesResult()
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string requestUri = $"users/{repoOwner}";
        var model = new OwnerInfoModel() { Login = "test-owner", };

        MockRequestResult(HttpStatusCode.OK, model, requestUri);

        var service = CreateService();

        // Act
        await service.OwnerExists(repoOwner);
        await service.OwnerExists(repoOwner);

        // Assert
        this.mockHttpClient.VerifyOnce(m => m.Get<OwnerInfoModel>(requestUri));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void RepoExists_WithNullOrEmptyRepoOwner_ThrowsException(string repoOwner)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.RepoExists(repoOwner, It.IsAny<string>());

        // Assert
        await act.Should().ThrowAsync<NullOrEmptyStringException>()
            .WithMessage($"The '{nameof(repoOwner)}' value cannot be null or empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void RepoExists_WithNullOrEmptyRepoName_ThrowsException(string repoName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.RepoExists("test-owner", repoName);

        // Assert
        await act.Should().ThrowAsync<NullOrEmptyStringException>()
            .WithMessage($"The '{nameof(repoName)}' value cannot be null or empty.");
    }

    [Fact]
    public async void RepoExists_WithNullDataResult_ReturnsFalse()
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string requestUri = $"users/{repoOwner}";
        MockRequestResult<RepoModel>(HttpStatusCode.OK, null, requestUri);

        var service = CreateService();

        // Act
        var actual = await service.RepoExists("test-owner", "test-repo");

        // Assert
        actual.Should().BeFalse();
    }

    [Theory]
    [InlineData("test-owner", "test-repo", HttpStatusCode.BadRequest, false)]
    [InlineData("other-owner", "test-repo", HttpStatusCode.OK, false)]
    [InlineData("test-owner", "other-repo", HttpStatusCode.OK, false)]
    [InlineData("test-owner", "test-repo", HttpStatusCode.OK, true)]
    public async void RepoExists_WhenInvoked_ReturnsCorrectResult(
        string login,
        string repoNameResult,
        HttpStatusCode statusCode,
        bool expectedResult)
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string repoName = "test-repo";
        const string ownerExistsRequestUri = $"users/{repoOwner}";
        const string repoExistsRequestUri = $"repos/{repoOwner}/{repoName}";

        var ownerModel = new OwnerInfoModel { Login = login };
        var repoModel = new RepoModel { Name = repoNameResult, Owner = ownerModel };

        MockRequestResult(statusCode, ownerModel, ownerExistsRequestUri);
        MockRequestResult(statusCode, repoModel, repoExistsRequestUri);

        var service = CreateService();

        // Act
        var actual = await service.RepoExists(repoOwner, repoName);

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public async void RepoExists_WhenInvokedMoreThanOnce_CachesResults()
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string repoName = "test-repo";
        const string ownerExistsRequestUri = $"users/{repoOwner}";
        const string repoExistsRequestUri = $"repos/{repoOwner}/{repoName}";

        var ownerModel = new OwnerInfoModel { Login = "test-owner" };
        var repoModel = new RepoModel { Name = "test-repo", Owner = ownerModel };

        MockRequestResult(HttpStatusCode.OK, ownerModel, ownerExistsRequestUri);
        MockRequestResult(HttpStatusCode.OK, repoModel, repoExistsRequestUri);

        var service = CreateService();

        // Act
        await service.RepoExists(repoOwner, repoName);
        await service.RepoExists(repoOwner, repoName);

        // Assert that each request was only made once because the request results should of been cached.
        this.mockHttpClient.VerifyOnce(m => m.Get<OwnerInfoModel>(ownerExistsRequestUri));
        this.mockHttpClient.VerifyOnce(m => m.Get<RepoModel>(repoExistsRequestUri));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void ReleaseExists_WithNullOrEmptyRepoOwner_ThrowsException(string repoOwner)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.ReleaseExists(repoOwner, It.IsAny<string>(), It.IsAny<string>());

        // Assert
        await act.Should().ThrowAsync<NullOrEmptyStringException>()
            .WithMessage($"The '{nameof(repoOwner)}' value cannot be null or empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void ReleaseExists_WithNullOrEmptyRepoName_ThrowsException(string repoName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.ReleaseExists("test-owner", repoName, It.IsAny<string>());

        // Assert
        await act.Should().ThrowAsync<NullOrEmptyStringException>()
            .WithMessage($"The '{nameof(repoName)}' value cannot be null or empty.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void ReleaseExists_WithNullOrEmptyReleaseName_ThrowsException(string releaseName)
    {
        // Arrange
        var service = CreateService();

        // Act
        var act = () => service.ReleaseExists("test-owner", "test-repo", releaseName);

        // Assert
        await act.Should().ThrowAsync<NullOrEmptyStringException>()
            .WithMessage($"The '{nameof(releaseName)}' value cannot be null or empty.");
    }

    [Fact]
    public async void ReleaseExists_WithNullDataResult_ReturnsFalse()
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string repoName = "test-repo";
        const string requestUri = $"repos/{repoOwner}{repoName}/releases";
        MockRequestResult<ReleaseModel>(HttpStatusCode.OK, null, requestUri);

        var service = CreateService();

        // Act
        var actual = await service.ReleaseExists("test-owner", "test-repo", "test-release");

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public async void ReleaseExists_WhenInvokedMoreThanOnceAndWithExistingOwnerButNonExistingRepo_ReturnsFalse()
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string repoName = "test-repo";
        const string releaseName = "test-release";
        const string ownerInfoRequestUri = $"users/{repoOwner}";
        const string repoRequestUri = $"repos/{repoOwner}/{repoName}";

        var service = CreateService();
        var ownerInfoModel = new OwnerInfoModel { Login = repoOwner };
        var repoModel = new RepoModel { Name = repoName, Owner = ownerInfoModel };

        MockRequestResult(HttpStatusCode.OK, ownerInfoModel, ownerInfoRequestUri);
        MockRequestResult(HttpStatusCode.BadRequest, repoModel, repoRequestUri);

        // Act
        var actualFirstInvoke = await service.ReleaseExists(repoOwner, repoName, releaseName);
        var actualSecondInvoke = await service.ReleaseExists(repoOwner, repoName, releaseName);

        // Assert
        actualFirstInvoke.Should().BeFalse();
        actualSecondInvoke.Should().BeFalse();
    }

    [Fact]
    public async void ReleaseExists_WhenInvokedMoreThanOnce_CachesResult()
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string repoName = "test-repo";
        const string releaseName = "test-release";
        const string ownerInfoRequestUri = $"users/{repoOwner}";
        const string repoRequestUri = $"repos/{repoOwner}/{repoName}";
        const string releaseRequestUri = $"repos/{repoOwner}/{repoName}/releases";

        var service = CreateService();
        var ownerInfoModel = new OwnerInfoModel { Login = repoOwner };
        var repoModel = new RepoModel { Name = repoName, Owner = ownerInfoModel };
        var releaseModel = new ReleaseModel { Name = releaseName };

        MockRequestResult(HttpStatusCode.OK, ownerInfoModel, ownerInfoRequestUri);
        MockRequestResult(HttpStatusCode.OK, repoModel, repoRequestUri);
        MockRequestResult(HttpStatusCode.OK, releaseModel, releaseRequestUri);

        // Act
        await service.ReleaseExists(repoOwner, repoName, releaseName);
        await service.ReleaseExists(repoOwner, repoName, releaseName);

        // Assert
        this.mockHttpClient.VerifyOnce(m => m.Get<ReleaseModel>(releaseRequestUri));
    }

    [Fact]
    public void Dispose_WhenInvoked_DisposesOfService()
    {
        // Arrange
        var service = CreateService();

        // Act
        service.Dispose();
        service.Dispose();

        // Assert
        this.mockHttpClient.VerifyOnce(m => m.Dispose());
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="GitHubDataService"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private GitHubDataService CreateService() => new (this.mockHttpClient.Object);

    /// <summary>
    /// Mocks a get request using the given <paramref name="statusCode"/> and <paramref name="requestUri"/>
    /// and returns the given <paramref name="model"/> data.
    /// </summary>
    /// <param name="statusCode">The status code to mock.</param>
    /// <param name="model">The model for the mock to return.</param>
    /// <param name="requestUri">The request URI to mock.</param>
    /// <typeparam name="T">The model type to return.</typeparam>
    private void MockRequestResult<T>(HttpStatusCode statusCode, T model, string? requestUri = null)
        where T : class
    {
        this.mockHttpClient.Setup(m => m.Get<T>(string.IsNullOrEmpty(requestUri) ? It.IsAny<string>() : requestUri))
            .Returns(Task.FromResult((statusCode, model)) !);
    }
}
