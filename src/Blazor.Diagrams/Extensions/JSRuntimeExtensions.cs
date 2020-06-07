using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Extensions
{
    public static class JSRuntimeExtensions
    {
        public static async Task<double[]> GetOffsetWithSize(this IJSRuntime jsRuntime, ElementReference element)
        {
            return await jsRuntime.InvokeAsync<double[]>("getOffsetWithSize", element);
        }

        public static async Task<Rectangle> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
        {
            return await jsRuntime.InvokeAsync<Rectangle>("getBoundingClientRect", element);
        }
    }
}
