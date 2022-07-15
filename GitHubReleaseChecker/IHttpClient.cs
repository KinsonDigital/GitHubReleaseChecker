// <copyright file="IHttpClient.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Net;

namespace GitHubReleaseChecker;

/// <summary>
/// Provides a class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
/// </summary>
public interface IHttpClient : IDisposable
{
    /// <summary>
    /// Gets or sets the BaseUrl property for requests made by this client instance.
    /// </summary>
    string BaseUrl { get; set; }

    /// <summary>
    /// Execute the request using GET HTTP method. Exception will be thrown if the request does not succeed.
    /// The response data is deserialized to the Data property of the returned response object.
    /// </summary>
    /// <param name="requestUri">The URI that the request is sent to.</param>
    /// <typeparam name="T">Target deserialization type.</typeparam>
    /// <returns>
    /// The following tuple:
    /// <list type="bullet">
    ///     <item><c>HttpStatusCode:</c> The HTTP status code of the request.</item>
    ///     <item><c>T?:</c> The data returned by the request.</item>
    /// </list>
    /// </returns>
    Task<(HttpStatusCode statusCode, T? data)> Get<T>(string requestUri)
        where T : class;
}
