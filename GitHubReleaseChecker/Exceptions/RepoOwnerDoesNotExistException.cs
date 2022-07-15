// <copyright file="RepoOwnerDoesNotExistException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace GitHubReleaseChecker.Exceptions;

/// <summary>
/// Occurs when a string is null or empty.
/// </summary>
public class RepoOwnerDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RepoOwnerDoesNotExistException"/> class.
    /// </summary>
    public RepoOwnerDoesNotExistException()
        : base("The repository owner does not exist.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepoOwnerDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public RepoOwnerDoesNotExistException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RepoOwnerDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public RepoOwnerDoesNotExistException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
