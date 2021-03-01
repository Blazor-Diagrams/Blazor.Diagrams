using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Diagrams.Core.Models.Tests.Links
{
    public class LinkModelTestFixture
    {
        public NodeModel SourceNode;
        public NodeModel AnotherNode;

        public NodeModel TargetNode;

        public LinkModelTestFixture()
        {
            SourceNode = new NodeModel();
            SourceNode.AddPort();

            AnotherNode = new NodeModel();
            AnotherNode.AddPort();

            TargetNode = new NodeModel();
            TargetNode.AddPort();
        }

        public LinkModel CreateLink()
        {
            return new LinkModel(SourceNode.GetPort(PortAlignment.Bottom), TargetNode.GetPort(PortAlignment.Bottom));
        }
    }
}
