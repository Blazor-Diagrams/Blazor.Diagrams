using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Core.Geometry;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.Diagrams.Extensions;

public static class JSRuntimeExtensions
{
    public static async Task<Rectangle> GetBoundingClientRect(this IJSRuntime jsRuntime, ElementReference element)
    {
        try
        {
            return await jsRuntime.InvokeAsync<Rectangle>("ZBlazorDiagrams.getBoundingClientRect", element);
        }
        catch (JSDisconnectedException)
        {
            // The Blazor Server circuit is gone, so the element can no longer be measured or rendered.
            // An empty rectangle is harmless because the component will not render again.
            return Rectangle.Zero;
        }
    }

    public static async Task ObserveResizes<T>(this IJSRuntime jsRuntime, ElementReference element,
        DotNetObjectReference<T> reference) where T : class
    {
        try
        {

            await jsRuntime.InvokeVoidAsync("ZBlazorDiagrams.observe", element, reference, element.Id);
        }
        catch (Exception ex) when (ex is ObjectDisposedException || ex is JSException)
        {
            // Ignore, element/reference was likely disposed
        }
    }

    public static async Task UnobserveResizes(this IJSRuntime jsRuntime, ElementReference element)
    {
        try
        {
            await jsRuntime.InvokeVoidAsync("ZBlazorDiagrams.unobserve", element, element.Id);
        }
        catch (JSDisconnectedException)
        {
            // Ignore, the Blazor Server circuit is gone and with it the browser side observer.
            // This is typically called while disposing, where a disconnected circuit is expected.
        }
    }
}