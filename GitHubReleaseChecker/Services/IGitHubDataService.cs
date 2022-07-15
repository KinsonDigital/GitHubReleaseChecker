// <copyright file="IGitHubDataService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace GitHubReleaseChecker.Services;

/// <summary>
/// Provides access to various kinds of GitHub data.
/// </summary>
public interface IGitHubDataService : IDisposable
{
    /// <summary>
    /// Returns a value indicating whether or not the repository owner
    /// with the name <see cref="repoOwner"/> exists.
    /// </summary>
    /// <param name="repoOwner">The owner of the repository.</param>
    /// <returns>An asynchronous bool result of <b>true</b> if the owner exists.</returns>
    /// <remarks>
    ///     The <paramref name="repoOwner"/> can also be the GitHub organization name if
    ///     the repository exists in a GitHub organization.
    /// </remarks>
    Task<bool> OwnerExists(string repoOwner);

    /// <summary>
    /// Returns a value indicating whether or not a repository that matches the given <paramref name="repoName"/>
    /// exists for the given <paramref name="repoOwner"/>.
    /// </summary>
    /// <param name="repoOwner">The owner of the repository.</param>
    /// <param name="repoName">The name of the repository.</param>
    /// <returns>An asynchronous bool result of <b>true</b> if the repository exists.</returns>
    /// <remarks>
    ///     The <paramref name="repoOwner"/> can also be the GitHub organization name if
    ///     the repository exists in a GitHub organization.
    /// </remarks>
    Task<bool> RepoExists(string repoOwner, string repoName);

    /// <summary>
    /// Returns a value indicating whether or not a release matching the given <paramref name="releaseName"/>,
    /// for a repository matching the given <paramref name="repoName"/>, owned by an owner that matches
    /// the given <paramref name="repoOwner"/> exists.
    /// </summary>
    /// <param name="repoOwner">The owner of the repository.</param>
    /// <param name="repoName">The name of the repository.</param>
    /// <param name="releaseName">The name of the release.</param>
    /// <returns>An asynchronous bool result of <b>true</b> if the release exists.</returns>
    /// <remarks>
    ///     The <paramref name="repoOwner"/> can also be the GitHub organization name if
    ///     the repository exists in a GitHub organization.
    /// </remarks>
    Task<bool> ReleaseExists(string repoOwner, string repoName, string releaseName);
}
