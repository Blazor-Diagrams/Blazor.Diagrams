# Publishing WTG.Z.Blazor.Diagrams for WTG usage

This doc explains how the package we use at WTG is updated.

## Creating Releases with Github Actions
When a PR is merged to master, the GitHub Action 'Create release' should run. This action creates a GitHub Release and uploads the package to the release. The version number is also handled by the action. The package is created on build in the action and should contain Blazor.Diagrams.dll, Blazor.Diagrams.Core.dll and SvgPathProperties.dll. This action also pushes this release to NuGet, see: [WTG.Z.Blazor.Diagrams](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions).

> [!tip]
> If you have a WTG workitem that requires a push to this repo, you will additionally need to update the WTG.Z.Blazor.Diagraqms version in the WTG Dev repo with the steps below.

## Updating WTG Dev Repo
1. Go to [WTG.Z.Blazor.Diagrams on nuget](https://proget.wtg.zone/feeds/Gallery/WTG.Z.Blazor.Diagrams/versions) and find the latest version number.
2. Navigate to [Directory.Packages.Props](https://devops.wisetechglobal.com/wtg/CargoWise/_git/Dev?path=%2FDirectory.Packages.props&version=GBmaster&line=113&lineEnd=113&lineStartColumn=1&lineEndColumn=72&lineStyle=plain&_a=contents) and update the version number in the line `<PackageVersion Include="WTG.Z.Blazor.Diagrams" Version="x.x.x" />` to the latest one.
