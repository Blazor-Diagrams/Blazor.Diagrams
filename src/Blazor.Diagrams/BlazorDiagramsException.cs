using System;

namespace Blazor.Diagrams;

public class BlazorDiagramsException : Exception
{
    public BlazorDiagramsException(string? message) : base(message)
    {
    }
}