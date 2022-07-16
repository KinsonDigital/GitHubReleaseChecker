// <copyright file="ReleaseModel.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace GitHubReleaseChecker.Models;

/// <summary>
/// Holds data about a single release.
/// </summary>
public record ReleaseModel
{
    /// <summary>
    /// Gets or sets the URL to the release page.
    /// </summary>
    public string HtmlUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the tag used for the release.
    /// </summary>
    public string TagName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the release.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether or not the release is a draft release.
    /// </summary>
    public bool Draft { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the release is a pre-release.
    /// </summary>
    public bool PreRelease { get; set; }
}
