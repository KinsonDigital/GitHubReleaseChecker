// <copyright file="GitHubAction.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

using GitHubReleaseChecker.Exceptions;
using GitHubReleaseChecker.Services;

namespace GitHubReleaseChecker;

/// <inheritdoc/>
public class GitHubAction : IGitHubAction
{
    private const string ReleaseExistsOutputName = "release-exists";
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

        try
        {
            this.gitHubConsoleService.Write($"Checking if the repository owner '{inputs.RepoOwner}' exists . . .");
            var repoOwnerExists = await this.githubDataService.OwnerExists(inputs.RepoOwner);

            if (repoOwnerExists is false)
            {
                var message = $"The repository owner '{inputs.RepoOwner}' does not exist.";

                if (inputs.FailWhenNotFound is true)
                {
                    throw new RepoOwnerDoesNotExistException(message);
                }

                this.gitHubConsoleService.WriteLine(message, true, true);
                this.actionOutputService.SetOutputValue(ReleaseExistsOutputName, repoOwnerExists.ToString().ToLower());

                return;
            }

            this.gitHubConsoleService.WriteLine(" the repository owner exists.", false, true);

            this.gitHubConsoleService.Write($"Checking if the repository '{inputs.RepoName}' exists . . .");
            var repoExists = await this.githubDataService.RepoExists(inputs.RepoOwner, inputs.RepoName);

            if (repoExists is false)
            {
                var message = $"The repository '{inputs.RepoName}' does not exist.";

                if (inputs.FailWhenNotFound is true)
                {
                    throw new RepoDoesNotExistException(message);
                }

                this.gitHubConsoleService.WriteLine(message, true, true);
                this.actionOutputService.SetOutputValue(ReleaseExistsOutputName, repoExists.ToString().ToLower());

                return;
            }

            this.gitHubConsoleService.WriteLine(" the repository exists.", false, true);

            this.gitHubConsoleService.Write($"Checking if the release '{inputs.ReleaseName}' exists . . .");
            var releaseExists = await this.githubDataService.ReleaseExists(
                inputs.RepoOwner,
                inputs.RepoName,
                inputs.ReleaseName,
                inputs.CheckPreReleases);

            if (releaseExists is false)
            {
                var message = $"The release '{inputs.ReleaseName}' does not exist.";

                if (inputs.FailWhenNotFound is true)
                {
                    throw new ReleaseDoesNotExistException(message);
                }

                this.gitHubConsoleService.WriteLine(message, true, true);
            }

            this.gitHubConsoleService.WriteLine(" the release exists.", false, true);
            this.actionOutputService.SetOutputValue(ReleaseExistsOutputName, releaseExists.ToString().ToLower());
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
