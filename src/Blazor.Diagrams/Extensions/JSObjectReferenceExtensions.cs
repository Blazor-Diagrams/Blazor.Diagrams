using System;
using System.Threading.Tasks;

using Blazor.Diagrams.Core.Geometry;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Diagrams.Extensions;

public static class JSObjectReferenceExtensions
{
    public static async Task<Rectangle> GetBoundingClientRect(this IJSObjectReference jsRuntime, ElementReference element)
    {
        return await jsRuntime.InvokeAsync<Rectangle>("getBoundingClientRect", element);
    }

    public static async Task ObserveResizes<T>(this IJSObjectReference jsRuntime, ElementReference element,
        DotNetObjectReference<T> reference) where T : class
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("observe", element, reference, element.Id);
        }
        catch (ObjectDisposedException)
        {
            // Ignore, DotNetObjectReference was likely disposed
        }
    }

    public static async Task UnobserveResizes(this IJSObjectReference jsRuntime, ElementReference element)
    {
        await jsRuntime.InvokeVoidAsync("unobserve", element, element.Id);
    }
}

