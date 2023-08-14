using Blazor.Diagrams.Core.Models;

namespace SharedDemo.Demos.CustomLink;

public class ThickLink : LinkModel
{
    public ThickLink(PortModel sourcePort, PortModel targetPort = null) : base(sourcePort, targetPort) { }
}
