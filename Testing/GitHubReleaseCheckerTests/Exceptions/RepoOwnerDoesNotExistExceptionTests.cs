// <copyright file="RepoOwnerDoesNotExistExceptionTests.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using FluentAssertions;
using GitHubReleaseChecker.Exceptions;

namespace GitHubReleaseCheckerTests.Exceptions;

/// <summary>
/// Tests the <see cref="RepoOwnerDoesNotExistException"/> class.
/// </summary>
public class RepoOwnerDoesNotExistExceptionTests
{
    #region Constructor Tests
    [Fact]
    public void Ctor_WithNoParam_CorrectlySetsExceptionMessage()
    {
        // Act
        var exception = new RepoOwnerDoesNotExistException();

        // Assert
        exception.Message.Should().Be("The repository owner does not exist.");
    }

    [Fact]
    public void Ctor_WhenInvokedWithSingleMessageParam_CorrectlySetsMessage()
    {
        // Act
        var exception = new RepoOwnerDoesNotExistException("test-message");

        // Assert
        exception.Message.Should().Be("test-message");
    }

    [Fact]
    public void Ctor_WhenInvokedWithMessageAndInnerException_ThrowsException()
    {
        // Arrange
        var innerException = new Exception("inner-exception");

        // Act
        var deviceException = new RepoOwnerDoesNotExistException("test-exception", innerException);

        // Assert
        deviceException.InnerException.Message.Should().Be("inner-exception");
        deviceException.Message.Should().Be("test-exception");
    }
    #endregion
}
