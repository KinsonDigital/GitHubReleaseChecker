// <copyright file="RepoModel.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace GitHubReleaseChecker.Models;

/// <summary>
/// Holds data about a repository.
/// </summary>
public record RepoModel
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owner information.
    /// </summary>
    public OwnerInfoModel Owner { get; set; } = null!;
}
