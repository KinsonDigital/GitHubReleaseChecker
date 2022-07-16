// <copyright file="ReleaseModelTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using GitHubReleaseChecker.Models;
using GitHubReleaseCheckerTests.Helpers;

namespace GitHubReleaseCheckerTests.Models;

/// <summary>
/// Tests the <see cref="ReleaseModel"/> class.
/// </summary>
public class ReleaseModelTests
{
    #region Prop Tests
    [Fact]
    public void AutoProperties_WhenSettingValues_ReturnsCorrectResults()
    {
        // Assert
        AssertExtensions.PropertyGetsAndSets<ReleaseModel, string>(nameof(ReleaseModel.HtmlUrl), "value");
        AssertExtensions.PropertyGetsAndSets<ReleaseModel, string>(nameof(ReleaseModel.TagName), "value");
        AssertExtensions.PropertyGetsAndSets<ReleaseModel, string>(nameof(ReleaseModel.Name), "value");
        AssertExtensions.PropertyGetsAndSets<ReleaseModel, bool>(nameof(ReleaseModel.Draft), true);
        AssertExtensions.PropertyGetsAndSets<ReleaseModel, bool>(nameof(ReleaseModel.PreRelease), true);
    }
    #endregion
}
