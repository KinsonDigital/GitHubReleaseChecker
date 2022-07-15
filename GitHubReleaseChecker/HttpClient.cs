// <copyright file="HttpClient.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;
using System.Net;
using GitHubReleaseChecker.Exceptions;
using RestSharp;

namespace GitHubReleaseChecker;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public sealed class HttpClient : IHttpClient
{
    private readonly RestClient restClient;
    private string baseUrl = string.Empty;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClient"/> class.
    /// </summary>
    public HttpClient() => this.restClient = new RestClient();

    /// <inheritdoc/>
    public string BaseUrl
    {
        get => this.baseUrl;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrEmptyStringException($"The property '{this.baseUrl}' value cannot be null or empty.");
            }

            while (value.EndsWith('/'))
            {
                value = value.TrimEnd('/');
            }

            this.baseUrl = value;
        }
    }

    /// <inheritdoc/>
    public async Task<(HttpStatusCode statusCode, T? data)> Get<T>(string requestUri)
        where T : class
    {
        if (string.IsNullOrEmpty(requestUri))
        {
            throw new NullOrEmptyStringException($"The argument '{requestUri}' cannot be null or empty.");
        }

        while (requestUri.StartsWith('/'))
        {
            requestUri = requestUri.TrimStart('/');
        }

        var fullUrl = $"{BaseUrl}/{requestUri}";

        this.restClient.AcceptedContentTypes = new[] { "application/vnd.github.v3+json" };
        var request = new RestRequest(fullUrl);

        var response = await this.restClient.ExecuteAsync<T>(request, Method.Get);

        return (response.StatusCode, response.Data);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (this.isDisposed)
        {
            return;
        }

        this.restClient.Dispose();

        this.isDisposed = true;
    }
}
