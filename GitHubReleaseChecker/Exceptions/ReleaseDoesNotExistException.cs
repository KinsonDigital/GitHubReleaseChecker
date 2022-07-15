// <copyright file="ReleaseDoesNotExistException.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace GitHubReleaseChecker.Exceptions;

/// <summary>
/// Occurs when a release does not exist.
/// </summary>
public class ReleaseDoesNotExistException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReleaseDoesNotExistException"/> class.
    /// </summary>
    public ReleaseDoesNotExistException()
        : base("The release does not exist.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReleaseDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ReleaseDoesNotExistException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReleaseDoesNotExistException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The <see cref="Exception"/> instance that caused the current exception.
    /// </param>
    public ReleaseDoesNotExistException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
