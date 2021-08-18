﻿using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Blazor.Diagrams.Extensions
{
    public static class JSRuntimeExtensions
    {
        public static async Task<Rectangle> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
        {
            return await jsRuntime.InvokeAsync<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", element);
        }

        public static async Task ObserveResizes<T>(this IJSRuntime jsRuntime, ElementReference element, 
            DotNetObjectReference<T> reference) where T : class
        {
            await jsRuntime.InvokeVoidAsync("ZBlazorDiagrams.observe", element, reference, element.Id);
        }

        public static async Task UnobserveResizes(this IJSRuntime jsRuntime, ElementReference element)
        {
            await jsRuntime.InvokeVoidAsync("ZBlazorDiagrams.unobserve", element, element.Id);
        }

        public static async Task<Size> GetSizeForLabel(this IJSRuntime jsRuntime, string content)
        {
            return await jsRuntime.InvokeAsync<Size>("ZBlazorDiagrams.getSizeForLabel", content);
        }
    }
}
