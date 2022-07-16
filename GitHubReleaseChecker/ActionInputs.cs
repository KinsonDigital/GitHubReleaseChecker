// <copyright file="ActionInputs.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable PropertyCanBeMadeInitOnly.Global
namespace GitHubReleaseChecker;

/// <summary>
/// Holds all of the GitHub actions inputs.
/// </summary>
public class ActionInputs
{
    /// <summary>
    /// Gets or sets the owner of the repository.
    /// </summary>
    [Option(
        "repo-owner",
        Required = true,
        HelpText = "The owner of the repository.")]
    public string RepoOwner { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the repository.
    /// </summary>
    [Option(
        "repo-name",
        Required = true,
        HelpText = "The name of the repository.")]
    public string RepoName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name of the release.
    /// </summary>
    [Option(
        "release-name",
        Required = true,
        HelpText = "The name of the release.")]
    public string ReleaseName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether or not the action will fail if the release is not found.
    /// </summary>
    [Option(
        "fail-when-not-found",
        Required = false,
        HelpText = "If true, will fail the workflow when the release is not found.  Default value of true.")]
    public bool? FailWhenNotFound { get; set; } = true;
}
