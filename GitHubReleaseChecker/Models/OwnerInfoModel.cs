// <copyright file="OwnerInfoModel.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace GitHubReleaseChecker.Models;

/// <summary>
/// Holds data about a repository owner.
/// </summary>
public record OwnerInfoModel
{
    /// <summary>
    /// Gets or sets the login name.
    /// </summary>
    public string Login { get; set; } = string.Empty;
}
