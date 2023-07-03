using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text.Json;

namespace Site.Components.Landing;

public partial class StatisticsLine
{
    private int _stars;
    private int _downloads;
    private string _version = "1.0.0";

    [Inject] private HttpClient HttpClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        (_version, _downloads) = await GetVersionAndDownloads();
        _stars = await GetStars();
    }

    private async Task<int> GetStars()
    {
        var content = await HttpClient.GetFromJsonAsync<JsonDocument>("https://api.github.com/repos/Blazor-Diagrams/Blazor.Diagrams");
        if (content == null)
            return 0;

        return content.RootElement.GetProperty("stargazers_count").GetInt32();
    }

    private async Task<(string, int)> GetVersionAndDownloads()
    {
        var content = await HttpClient.GetFromJsonAsync<JsonDocument>("https://api.nuget.org/v3/index.json");
        if (content != null)
        {
            foreach (var resource in content.RootElement.GetProperty("resources").EnumerateArray())
            {
                if (resource.GetProperty("@type").GetString() == "SearchQueryService")
                {
                    var url = resource.GetProperty("@id").GetString();
                    var packageContent = await HttpClient.GetFromJsonAsync<JsonDocument>(url + "?prerelease=true&q=packageid:Z.Blazor.Diagrams");
                    if (packageContent != null)
                    {
                        foreach (var data in packageContent.RootElement.GetProperty("data").EnumerateArray())
                        {
                            var version = data.GetProperty("version").GetString()!;
                            var downloads = data.GetProperty("totalDownloads").GetInt32();
                            return (version, downloads);
                        }
                    }
                }
            }
        }

        return ("1.0.0", 0);
    }
}
