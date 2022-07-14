﻿// <copyright file="ActionInputTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using CommandLine;
using GitHubReleaseChecker;
using GitHubReleaseCheckerTests.Helpers;
using FluentAssertions;

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
        inputs.Message.Should().BeEmpty();
        typeof(ActionInputs).GetProperty(nameof(ActionInputs.Message)).Should().BeDecoratedWith<OptionAttribute>();
        inputs.GetAttrFromProp<OptionAttribute>(nameof(ActionInputs.Message))
            .AssertOptionAttrProps("message", true, "Prints a message to the console.");
    }
    #endregion
}
