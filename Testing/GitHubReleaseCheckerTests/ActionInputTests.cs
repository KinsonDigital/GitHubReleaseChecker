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
    #region Prop Tests
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
    }
    #endregion
}
