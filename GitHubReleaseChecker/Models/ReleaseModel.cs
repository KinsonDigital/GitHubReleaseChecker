namespace GitHubReleaseChecker.Models;

public record ReleaseModel
{
    public string HTMLURL { get; set; }
    public string TagName { get; set; }
    public string Name { get; set; }
    public bool Draft { get; set; }
    public bool PreRelease { get; set; }
}


/*
 TODO: Delete this when finished.  This is the required JSON data from the GitHub API

{
    "html_url": "https://github.com/KinsonDigital/Velaptor/releases/tag/v1.0.0-preview.9",
    "tag_name": "v1.0.0-preview.9",
    "name": "v1.0.0-preview.9",
    "draft": false,
    "prerelease": true,
}
 */
