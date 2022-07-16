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
    private readonly Dictionary<string, bool> repoOwnersRequested = new ();
    private readonly Dictionary<string, bool> repoNamesRequested = new ();
    private readonly Dictionary<string, bool> releaseNamesRequested = new ();
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
    /// <exception cref="NullOrEmptyStringException">
    ///     Occurs if the <paramref name="repoOwner"/> is null or empty.
    /// </exception>
    public async Task<bool> OwnerExists(string repoOwner)
    {
        if (string.IsNullOrEmpty(repoOwner))
        {
            throw new NullOrEmptyStringException($"The '{nameof(repoOwner)}' value cannot be null or empty.");
        }

        // If already requested
        if (this.repoOwnersRequested.ContainsKey(repoOwner))
        {
            return this.repoOwnersRequested[repoOwner];
        }

        var resourceUri = $"users/{repoOwner}";
        var response = await this.client.Get<OwnerInfoModel>(resourceUri);

        var repoOwnerExists = response.statusCode == HttpStatusCode.OK &&
            response.data is not null &&
            string.Equals(response.data.Login, repoOwner, StringComparison.CurrentCultureIgnoreCase);

        // Cache the request result
        this.repoOwnersRequested.Add(repoOwner, repoOwnerExists);

        return repoOwnerExists;
    }

    /// <inheritdoc/>
    /// <exception cref="NullOrEmptyStringException">
    ///     Occurs if the <paramref name="repoOwner"/> or <paramref name="repoName"/> are <b>null</b> or empty.
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

        var repoOwnerResult = this.repoOwnersRequested.ContainsKey(repoOwner)
            ? this.repoOwnersRequested[repoOwner]
            : await OwnerExists(repoOwner);

        if (repoOwnerResult is false)
        {
            return false;
        }

        var repoNameKey = $"{repoOwner}:{repoName}";

        // If already requested
        if (this.repoNamesRequested.ContainsKey(repoNameKey))
        {
            return this.repoNamesRequested[repoNameKey];
        }

        var requestUri = $"repos/{repoOwner}/{repoName}";
        var response = await this.client.Get<RepoModel>(requestUri);

        var repoNameExists = response.statusCode == HttpStatusCode.OK &&
               response.data is not null &&
               string.Equals(response.data.Owner.Login, repoOwner, StringComparison.CurrentCultureIgnoreCase) &&
               string.Equals(response.data.Name, repoName, StringComparison.CurrentCultureIgnoreCase);

        // Cache the request result
        this.repoNamesRequested.Add(repoNameKey, repoNameExists);

        return repoNameExists;
    }

    /// <inheritdoc/>
    /// <exception cref="NullOrEmptyStringException">
    ///     Occurs if the <paramref name="repoOwner"/>, <paramref name="repoName"/>, or <paramref name="releaseName"/> are <b>null</b> or empty.
    /// </exception>
    public async Task<bool> ReleaseExists(string repoOwner, string repoName, string releaseName, bool? checkPreReleases)
    {
        if (string.IsNullOrEmpty(repoOwner))
        {
            throw new NullOrEmptyStringException($"The '{nameof(repoOwner)}' value cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(repoName))
        {
            throw new NullOrEmptyStringException($"The '{nameof(repoName)}' value cannot be null or empty.");
        }

        if (string.IsNullOrEmpty(releaseName))
        {
            throw new NullOrEmptyStringException($"The '{nameof(releaseName)}' value cannot be null or empty.");
        }

        var repoOwnerResult = this.repoOwnersRequested.ContainsKey(repoOwner)
            ? this.repoOwnersRequested[repoOwner]
            : await OwnerExists(repoOwner);

        if (repoOwnerResult is false)
        {
            return false;
        }

        var repoNameKey = $"{repoOwner}:{repoName}";
        var repoNameResult = this.repoNamesRequested.ContainsKey(repoNameKey)
            ? this.repoNamesRequested[repoNameKey]
            : await RepoExists(repoOwner, repoName);

        if (repoNameResult is false)
        {
            return false;
        }

        var releaseNameKey = $"{repoOwner}:{repoName}:{releaseName}";

        if (this.releaseNamesRequested.ContainsKey(releaseNameKey))
        {
            return this.releaseNamesRequested[releaseNameKey];
        }

        var requestUri = $"repos/{repoOwner}/{repoName}/releases";
        var response = await this.client.Get<ReleaseModel[]>(requestUri);

        if (response.statusCode != HttpStatusCode.OK)
        {
            return false;
        }

        var releaseExists = response.data?.Any(release =>
        {
            if (checkPreReleases is true)
            {
                return string.Equals(release.Name, releaseName, StringComparison.CurrentCultureIgnoreCase) &&
                       release.PreRelease;
            }

            return string.Equals(release.Name, releaseName, StringComparison.CurrentCultureIgnoreCase);
        }) ?? false;

        // Cache the request result
        this.releaseNamesRequested.Add(releaseNameKey, releaseExists);

        return releaseExists;
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
