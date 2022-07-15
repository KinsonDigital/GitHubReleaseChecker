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
    Task<bool> OwnerExists(string repoOwner);

    /// <summary>
    /// Returns a value indicating whether or not a repository that matches the given <paramref name="repoName"/>
    /// exists for the given <paramref name="repoOwner"/>.
    /// </summary>
    /// <returns>An asynchronous bool result of <b>true</b> if the repository exists.</returns>
    /// <param name="repoOwner">The owner of the repository.</param>
    /// <param name="repoName">The name of the repository.</param>
    /// <exception cref="InvalidOperationException">
    ///     Occurs if the <see cref="repoOwner"/> or <see cref="repoName"/> are <b>null</b> or empty.
    /// </exception>
    Task<bool> RepoExists(string repoOwner, string repoName);
}
