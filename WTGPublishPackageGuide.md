# Publishing WTG.Z.Blazor.Diagrams for WTG usage
This doc explains how the package we use at WTG is updated.

When a PR is merged to master, the GitHub Action 'Create release' should run. It can also be run manually if needed. This action creates a GitHub Release and uploads the package to the release. The version number is also handled by the action. The package is created on build in the action and should contain Blazor.Diagrams.dll, Blazor.Diagrams.Core.dll and SvgPathProperties.dll

## To push package to proget
1. **Find the release**. Most likely the most recent release is the one to use. To find all releases, go to the repo main page then click 'Releases'.
2. **Download the package**. Expand the 'Assets' section of the release and download the *.nupkg file.
3. **Push to proget**. Modify the PushNuget.ps1 script so that it had the correct path to the downloaded *.nupkg file and the API key. Ask another team member if not sure. Once the script has all required information, run the script to push the package.
4. **Update package version in WTG**. In Directory.Packages.Props, update the version of WTG.Z.Blazor.Diagrams to match the package that was just uploaded to proget.


