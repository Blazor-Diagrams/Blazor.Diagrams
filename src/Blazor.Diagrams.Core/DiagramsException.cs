using System;

namespace Blazor.Diagrams.Core;

public class DiagramsException : Exception
{
    public DiagramsException(string? message) : base(message)
    {
    }
}
