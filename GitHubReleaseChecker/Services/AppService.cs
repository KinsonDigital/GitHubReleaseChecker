﻿// <copyright file="AppService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace GitHubReleaseChecker.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class AppService : IAppService
{
    private readonly IConsoleService gitHubConsoleService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppService"/> class.
    /// </summary>
    /// <param name="gitHubConsoleService">Writes to the console.</param>
    public AppService(IConsoleService gitHubConsoleService) => this.gitHubConsoleService = gitHubConsoleService;

    /// <inheritdoc/>
    public void Exit(int code)
    {
#if DEBUG // Kept here to pause console for debugging purposes
        this.gitHubConsoleService.PauseConsole();
#endif
        Environment.Exit(code);
    }

    /// <inheritdoc/>
    public void ExitWithNoError() => Exit(0);

    /// <inheritdoc/>
    public void ExitWithException(Exception exception)
    {
        this.gitHubConsoleService.WriteError(exception.Message);
        Exit(exception.HResult);
    }
}
