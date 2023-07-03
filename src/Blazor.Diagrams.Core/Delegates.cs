using Blazor.Diagrams.Core.Anchors;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;

namespace Blazor.Diagrams.Core;

public delegate BaseLinkModel? LinkFactory(Diagram diagram, ILinkable source, Anchor targetAnchor);

public delegate Anchor AnchorFactory(Diagram diagram, BaseLinkModel link, ILinkable model);

public delegate GroupModel GroupFactory(Diagram diagram, NodeModel[] children);
