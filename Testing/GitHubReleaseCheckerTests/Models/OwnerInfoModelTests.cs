// <copyright file="OwnerInfoModelTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using GitHubReleaseChecker.Models;
using GitHubReleaseCheckerTests.Helpers;

namespace GitHubReleaseCheckerTests.Models;

/// <summary>
/// Tests the <see cref="OwnerInfoModel"/> class.
/// </summary>
public class OwnerInfoModelTests
{
    #region Prop Tests
    [Fact]
    public void AutoProperties_WhenSettingValues_ReturnsCorrectValues()
    {
        // Assert
        AssertExtensions.PropertyGetsAndSets<OwnerInfoModel, string>(nameof(OwnerInfoModel.Login), "value");
    }
    #endregion
}
