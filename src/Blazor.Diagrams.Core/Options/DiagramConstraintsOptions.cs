using System;
using System.Threading.Tasks;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core.Options;

public class DiagramConstraintsOptions
{
    public Func<NodeModel, ValueTask<bool>> ShouldDeleteNode { get; set; } = _ => ValueTask.FromResult(true);
    public Func<BaseLinkModel, ValueTask<bool>> ShouldDeleteLink { get; set; } = _ => ValueTask.FromResult(true);
    public Func<GroupModel, ValueTask<bool>> ShouldDeleteGroup { get; set; } = _ => ValueTask.FromResult(true);
}