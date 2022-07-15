// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using GitHubReleaseChecker.Services;

namespace GitHubReleaseChecker;

/// <inheritdoc/>
public class GitHubAction : IGitHubAction
{
    private readonly IConsoleService gitHubConsoleService;
    private readonly IActionOutputService actionOutputService;
    private readonly IGitHubDataService githubDataService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GitHubAction"/> class.
    /// </summary>
    /// <param name="gitHubConsoleService">Writes to the console.</param>
    /// <param name="actionOutputService">Sets the output data of the action.</param>
    /// <param name="githubDataService">Gets data from GitHub.</param>
    public GitHubAction(
        IConsoleService gitHubConsoleService,
        IActionOutputService actionOutputService,
        IGitHubDataService githubDataService)
    {
        EnsureThat.ParamIsNotNull(gitHubConsoleService);
        EnsureThat.ParamIsNotNull(actionOutputService);
        EnsureThat.ParamIsNotNull(githubDataService);

        this.gitHubConsoleService = gitHubConsoleService;
        this.actionOutputService = actionOutputService;
        this.githubDataService = githubDataService;
    }

    /// <inheritdoc/>
    public async Task Run(ActionInputs inputs, Action onCompleted, Action<Exception> onError)
    {
        ShowWelcomeMessage();

        // var repoOwnerExists =
        try
        {
        }
        catch (Exception e)
        {
            onError(e);
        }

        onCompleted();
    }

    /// <summary>
    /// Shows a welcome message with additional information.
    /// </summary>
    private void ShowWelcomeMessage()
        => this.gitHubConsoleService.WriteLine(
            "Welcome To Release Checker GitHub Action!!",
            false,
            true);
}
