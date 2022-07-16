// <copyright file="GitHubActionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using GitHubReleaseChecker;
using GitHubReleaseChecker.Exceptions;
using GitHubReleaseChecker.Services;
using GitHubReleaseCheckerTests.Helpers;
using Moq;

namespace GitHubReleaseCheckerTests;

public class GitHubActionTests
{
    private const string ReleaseExistsOutputName = "release-exists";
    private readonly Mock<IConsoleService> mockConsoleService;
    private readonly Mock<IActionOutputService> mockActionOutputService;
    private readonly Mock<IGitHubDataService> mockGitHubDataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubActionTests"/> class.
    /// </summary>
    public GitHubActionTests()
    {
        this.mockConsoleService = new Mock<IConsoleService>();
        this.mockActionOutputService = new Mock<IActionOutputService>();

        this.mockGitHubDataService = new Mock<IGitHubDataService>();
        this.mockGitHubDataService.Setup(m => m.OwnerExists(It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        this.mockGitHubDataService.Setup(m => m.RepoExists(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(true));
        this.mockGitHubDataService.Setup(m
                => m.ReleaseExists(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool?>()))
            .Returns(Task.FromResult(true));
    }

    #region Constructor Tests
    [Fact]
    public void Ctor_WithNullGitHubConsoleServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GitHubAction(
                null,
                this.mockActionOutputService.Object,
                this.mockGitHubDataService.Object);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'gitHubConsoleService')");
    }

    [Fact]
    public void Ctor_WithNullActionOutputServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GitHubAction(
                this.mockConsoleService.Object,
                null,
                this.mockGitHubDataService.Object);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'actionOutputService')");
    }

    [Fact]
    public void Ctor_WithNullGitHubDataServiceParam_ThrowsException()
    {
        // Arrange & Act
        var act = () =>
        {
            _ = new GitHubAction(
                this.mockConsoleService.Object,
                this.mockActionOutputService.Object,
                null);
        };

        // Assert
        act.Should()
            .Throw<ArgumentNullException>()
            .WithMessage("The parameter must not be null. (Parameter 'gitHubDataService')");
    }
    #endregion

    #region Method Tests
    [Fact]
    public async void Run_WhenInvoked_ShowsWelcomeMessage()
    {
        // Arrange
        var inputs = CreateInputs();
        var action = CreateAction();

        // Act
        await action.Run(inputs, () => { }, _ => { });

        // Assert
        this.mockConsoleService.VerifyOnce(m => m.WriteLine("Welcome To Release Checker GitHub Action!!", false, true));
    }

    [Fact]
    public async void Run_WhenRepoOwnerDoesNotExistAndFailIsSetToTrue_ThrowsException()
    {
        // Arrange
        this.mockGitHubDataService.Setup(m => m.OwnerExists(It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        var inputs = CreateInputs(failWhenNotFound: true);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, () => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<RepoOwnerDoesNotExistException>()
            .WithMessage($"The repository owner '{inputs.RepoOwner}' does not exist.");
        this.mockGitHubDataService.VerifyOnce(m => m.OwnerExists(inputs.RepoOwner));
    }

    [Fact]
    public async void Run_WhenRepoOwnerDoesNotExistAndFailIsSetToFalse_DoesNotThrowException()
    {
        // Arrange
        this.mockGitHubDataService.Setup(m => m.OwnerExists(It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        var inputs = CreateInputs(failWhenNotFound: false);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, () => { }, e => throw e);

        // Assert
        await act.Should().NotThrowAsync();
        this.mockGitHubDataService.VerifyOnce(m => m.OwnerExists(inputs.RepoOwner));
        this.mockConsoleService.VerifyOnce(m
            => m.WriteLine($"The repository owner '{inputs.RepoOwner}' does not exist.", true, true));
        this.mockActionOutputService.VerifyOnce(m => m.SetOutputValue(ReleaseExistsOutputName, "false"));
    }

    [Fact]
    public async void Run_WhenRepoNameDoesNotExistAndFailIsSetToTrue_ThrowsException()
    {
        // Arrange
        this.mockGitHubDataService.Setup(m => m.RepoExists(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        var inputs = CreateInputs(failWhenNotFound: true);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, () => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<RepoDoesNotExistException>()
            .WithMessage($"The repository '{inputs.RepoName}' does not exist.");
        this.mockGitHubDataService.VerifyOnce(m => m.RepoExists(inputs.RepoOwner, inputs.RepoName));
    }

    [Fact]
    public async void Run_WhenRepoNameDoesNotExistAndFailIsSetToFalse_DoesNotThrowException()
    {
        // Arrange
        this.mockGitHubDataService.Setup(m => m.RepoExists(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.FromResult(false));
        var inputs = CreateInputs(failWhenNotFound: false);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, () => { }, e => throw e);

        // Assert
        await act.Should().NotThrowAsync();
        this.mockGitHubDataService.VerifyOnce(m => m.RepoExists(inputs.RepoOwner, inputs.RepoName));
        this.mockConsoleService.VerifyOnce(m
            => m.WriteLine($"The repository '{inputs.RepoName}' does not exist.", true, true));
        this.mockActionOutputService.VerifyOnce(m => m.SetOutputValue(ReleaseExistsOutputName, "false"));
    }

    [Fact]
    public async void Run_WhenReleaseDoesNotExistAndFailIsSetToTrue_ThrowsException()
    {
        // Arrange
        this.mockGitHubDataService.Setup(m
            => m.ReleaseExists(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool?>()))
            .Returns(Task.FromResult(false));
        var inputs = CreateInputs(failWhenNotFound: true);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, () => { }, e => throw e);

        // Assert
        await act.Should().ThrowAsync<ReleaseDoesNotExistException>()
            .WithMessage($"The release '{inputs.ReleaseName}' does not exist.");
        this.mockGitHubDataService.VerifyOnce(m
            => m.ReleaseExists(inputs.RepoOwner, inputs.RepoName, inputs.ReleaseName, inputs.CheckPreReleases));
    }

    [Fact]
    public async void Run_WhenEverythingExists_WritesCorrectlyToConsole()
    {
        // Arrange
        var inputs = CreateInputs(failWhenNotFound: false);
        var action = CreateAction();

        // Act
        await action.Run(inputs, () => { }, _ => { });

        // Assert
        this.mockConsoleService.VerifyOnce(m => m.Write($"Checking if the repository owner '{inputs.RepoOwner}' exists . . ."));
        this.mockConsoleService.VerifyOnce(m
            => m.WriteLine($" the repository owner exists.", false, true));

        this.mockConsoleService.VerifyOnce(m => m.Write($"Checking if the repository '{inputs.RepoName}' exists . . ."));
        this.mockConsoleService.VerifyOnce(m
            => m.WriteLine($" the repository exists.", false, true));

        this.mockConsoleService.VerifyOnce(m => m.Write($"Checking if the release '{inputs.ReleaseName}' exists . . ."));
        this.mockConsoleService.VerifyOnce(m
            => m.WriteLine($" the release exists.", false, true));
    }

    [Fact]
    public async void Run_WhenReleaseNameDoesNotExistAndFailIsSetToFalse_SetsOutputToCorrectValue()
    {
        // Arrange
        this.mockGitHubDataService.Setup(m
                => m.ReleaseExists(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool?>()))
            .Returns(Task.FromResult(false));

        var inputs = CreateInputs(failWhenNotFound: false);
        var action = CreateAction();

        // Act
        var act = () => action.Run(inputs, () => { }, _ => { });

        // Assert
        await act.Should().NotThrowAsync();
        this.mockGitHubDataService.VerifyOnce(m
            => m.ReleaseExists(inputs.RepoOwner, inputs.RepoName, inputs.ReleaseName, inputs.CheckPreReleases));
        this.mockConsoleService.VerifyOnce(m
            => m.WriteLine($"The release '{inputs.ReleaseName}' does not exist.", true, true));
        this.mockActionOutputService.VerifyOnce(m => m.SetOutputValue(ReleaseExistsOutputName, "false"));
    }
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ActionInputs"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static ActionInputs CreateInputs(
        string repoOwner = "test-owner",
        string repoName = "test-repo",
        string releaseName = "test-release-name",
        bool checkPreReleases = true,
        bool failWhenNotFound = true) => new ()
    {
        RepoOwner = repoOwner,
        RepoName = repoName,
        ReleaseName = releaseName,
        CheckPreReleases = checkPreReleases,
        FailWhenNotFound = failWhenNotFound,
    };

    /// <summary>
    /// Creates a new instance of <see cref="GitHubAction"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private GitHubAction CreateAction()
        => new (
            this.mockConsoleService.Object,
            this.mockActionOutputService.Object,
            this.mockGitHubDataService.Object);
}
