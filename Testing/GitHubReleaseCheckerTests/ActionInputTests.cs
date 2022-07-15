// <copyright file="ActionInputTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using CommandLine;
using FluentAssertions;
using GitHubReleaseChecker;
using GitHubReleaseCheckerTests.Helpers;

// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable PossibleMultipleEnumeration
namespace GitHubReleaseCheckerTests;

/// <summary>
/// Tests the <see cref="ActionInputs"/> class.
/// </summary>
public class ActionInputTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WhenConstructed_PropsHaveCorrectDefaultValuesAndDecoratedWithAttributes()
    {
        // Arrange & Act
        var inputs = new ActionInputs();

        // Assert
        inputs.RepoOwner.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.RepoOwner)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.RepoOwner))
            .AssertOptionAttrProps("repo-owner", true, "The owner of the repository.");

        inputs.RepoName.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.RepoName)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.RepoName))
            .AssertOptionAttrProps("repo-name", true, "The name of the repository.");

        inputs.ReleaseName.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.ReleaseName)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.ReleaseName))
            .AssertOptionAttrProps("release-name", true, "The name of the release.");

        inputs.FailWhenNotFound.Should().BeTrue();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.FailWhenNotFound)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.FailWhenNotFound))
            .AssertOptionAttrProps("fail-when-not-found", false, "If true, will fail the workflow when the release is not found.  Default value of true.");
    }
    #endregion

    #region Prop Tests
    [Fact]
    public void RepoOwner_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();

        // Act
        inputs.RepoOwner = "test-owner";

        // Assert
        inputs.RepoOwner.Should().Be("test-owner");
    }

    [Fact]
    public void RepoName_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();

        // Act
        inputs.RepoName = "test-name";

        // Assert
        inputs.RepoName.Should().Be("test-name");
    }

    [Fact]
    public void ReleaseName_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();

        // Act
        inputs.ReleaseName = "test-name";

        // Assert
        inputs.ReleaseName.Should().Be("test-name");
    }

    [Fact]
    public void FailWhenNotValid_WhenSettingValue_ReturnsCorrectResult()
    {
        // Arrange
        var inputs = new ActionInputs();

        // Act
        var expected = !inputs.FailWhenNotFound;
        inputs.FailWhenNotFound = !inputs.FailWhenNotFound;

        // Assert
        inputs.FailWhenNotFound.Should().Be(expected);
    }
    #endregion
}
