// <copyright file="RepoModelTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using GitHubReleaseChecker.Models;
using GitHubReleaseCheckerTests.Helpers;

namespace GitHubReleaseCheckerTests.Models;

/// <summary>
/// Tests the <see cref="RepoModel"/> class.
/// </summary>
public class RepoModelTests
{
    #region Prop Tests
    [Fact]
    public void AutoProperties_WhenSettingValues_ReturnsCorrectValues()
    {
        // Assert
        AssertExtensions.PropertyGetsAndSets<RepoModel, string>(nameof(RepoModel.Name), "value");
        AssertExtensions.PropertyGetsAndSets<RepoModel, OwnerInfoModel>(nameof(RepoModel.Owner), new OwnerInfoModel());
    }
    #endregion
}
