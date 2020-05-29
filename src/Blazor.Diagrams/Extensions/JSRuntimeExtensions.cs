using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Extensions
{
    public static class JSRuntimeExtensions
    {
        public static async Task<double[]> GetOffset(this IJSRuntime jsRuntime, ElementReference element)
        {
            return await jsRuntime.InvokeAsync<double[]>("getOffset", element);
        }
    }
}
