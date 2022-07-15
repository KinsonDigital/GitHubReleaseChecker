﻿// <copyright file="GitHubConsoleService.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace GitHubReleaseChecker.Services;

/// <inheritdoc/>
[ExcludeFromCodeCoverage]
public class GitHubConsoleService : ConsoleService
{
    /// <inheritdoc/>
    public override void StartGroup(string name)
    {
#if DEBUG
        base.StartGroup(name);
#else
        Console.WriteLine($"::group::{(string.IsNullOrEmpty(name) ? "Group" : name)}");
#endif
    }

    /// <inheritdoc/>
    public override void EndGroup()
    {
#if DEBUG
        base.EndGroup();
#else
        Console.WriteLine("::endgroup::");
#endif
    }

    /// <inheritdoc/>
    public override void WriteError(string value)
    {
#if DEBUG
        base.WriteError(value);
#else
        Console.WriteLine($"::error::{value}");
#endif
    }
}
