// <copyright file="GitHubDataService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Net;
using GitHubReleaseChecker.Exceptions;
using GitHubReleaseChecker.Models;

namespace GitHubReleaseChecker.Services;

/// <summary>
/// <inheritdoc/>
/// </summary>
public sealed class GitHubDataService : IGitHubDataService
{
    private const string BaseUrl = "https://api.github.com";
    private readonly IHttpClient client;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubDataService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to make the requests.</param>
    public GitHubDataService(IHttpClient httpClient)
    {
        this.client = httpClient;
        this.client.BaseUrl = BaseUrl;
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    ///     Occurs if the <see cref="repoOwner"/> is null or empty.
    /// </exception>
    public async Task<bool> OwnerExists(string repoOwner)
    {
        if (string.IsNullOrEmpty(repoOwner))
        {
            throw new NullOrEmptyStringException($"The '{nameof(repoOwner)}' value cannot be null or empty.");
        }

        var resourceUri = $"users/{repoOwner}";
        var response = await this.client.Get<OwnerInfoModel>(resourceUri);

        return response.statusCode == HttpStatusCode.OK &&
            response.data is not null &&
            string.Equals(response.data.Login, repoOwner, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    ///     Occurs if the <see cref="repoOwner"/> or <see cref="repoName"/> are <b>null</b> or empty.
    /// </exception>
    public async Task<bool> RepoExists(string repoOwner, string repoName)
    {
        if (string.IsNullOrEmpty(repoOwner))
        {
            throw new NullOrEmptyStringException($"The '{nameof(repoOwner)}' value cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(repoName))
        {
            throw new NullOrEmptyStringException($"The '{nameof(repoName)}' value cannot be null or empty.");
        }

        var requestUri = $"repos/{repoOwner}/{repoName}";

        var response = await this.client.Get<RepoModel>(requestUri);

        return response.statusCode == HttpStatusCode.OK &&
               response.data is not null &&
               string.Equals(response.data.Owner.Login, repoOwner, StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(response.data.Name, repoName, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.client.Dispose();
        this.isDisposed = true;
    }
}
