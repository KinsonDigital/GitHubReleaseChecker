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
        this.mockHttpClient.Setup(m => m.Get<OwnerInfoModel>($"users/{repoOwner}"))
            .Returns(Task.FromResult<(HttpStatusCode, OwnerInfoModel)>((HttpStatusCode.OK, null) !) !);

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
        var model = new OwnerInfoModel() { Login = login, };
        this.mockHttpClient.Setup(m => m.Get<OwnerInfoModel>($"users/{repoOwner}"))
            .Returns(Task.FromResult((statusCode, model)) !);

        var service = CreateService();

        // Act
        var actual = await service.OwnerExists(repoOwner);

        // Assert
        actual.Should().Be(expectedResult);
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
        this.mockHttpClient.Setup(m => m.Get<RepoModel>($"users/{repoOwner}"))
            .Returns(Task.FromResult<(HttpStatusCode, RepoModel)>((HttpStatusCode.OK, null) !) !);

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
    public async void RepoExists_WhenInvokedWithOKStatusCode_ReturnsCorrectResult(
        string login,
        string repoNameResult,
        HttpStatusCode statusCode,
        bool expectedResult)
    {
        // Arrange
        const string repoOwner = "test-owner";
        const string repoName = "test-repo";
        var ownerModel = new OwnerInfoModel { Login = login };
        var model = new RepoModel { Name = repoNameResult, Owner = ownerModel };
        this.mockHttpClient.Setup(m => m.Get<RepoModel>($"repos/{repoOwner}/{repoName}"))
            .Returns(Task.FromResult((statusCode, model)) !);

        var service = CreateService();

        // Act
        var actual = await service.RepoExists(repoOwner, repoName);

        // Assert
        actual.Should().Be(expectedResult);
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
}
