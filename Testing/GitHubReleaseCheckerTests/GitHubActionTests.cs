// <copyright file="GitHubActionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using GitHubReleaseChecker;
using GitHubReleaseChecker.Services;
using GitHubReleaseCheckerTests.Helpers;
using Moq;

namespace GitHubReleaseCheckerTests;

public class GitHubActionTests
{
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
    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ActionInputs"/> for the purpose of testing.
    /// </summary>
    /// <returns>The instance to test.</returns>
    private static ActionInputs CreateInputs() => new ()
    {
        RepoOwner = "test-owner",
        RepoName = "test-repo",
        ReleaseName = "test-release-name",
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
