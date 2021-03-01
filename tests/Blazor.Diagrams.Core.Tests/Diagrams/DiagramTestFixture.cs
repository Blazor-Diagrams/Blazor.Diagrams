using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Diagrams.Core.Tests.Diagrams
{
    public class DiagramTestFixture :IDisposable
    {
        public Diagram Diagram;

        public bool NodeAddedEventTriggered;
        public bool NodeRemovedEventTriggered;       
        public bool LinkAddedEventTriggered;
        public bool LinkRemovedEventTriggered;
        public bool GroupAddedEventTriggered;
        public bool GroupRemovedEventTriggered;
        public DiagramTestFixture()
        {
            Diagram = new Diagram(new DiagramOptions());
            Diagram.Nodes.Added += Nodes_Added;
            Diagram.Nodes.Removed += Nodes_Removed;
            Diagram.Links.Added += Links_Added;
            Diagram.Links.Removed += Links_Removed;
            Diagram.GroupAdded += Diagram_GroupAdded;
            Diagram.GroupRemoved += Diagram_GroupRemoved;
        }

        private void Diagram_GroupRemoved(GroupModel obj)
        {
            GroupRemovedEventTriggered = true;
        }

        private void Diagram_GroupAdded(GroupModel obj)
        {
            GroupAddedEventTriggered = true;
        }

        private void Links_Removed(Models.Base.BaseLinkModel[] obj)
        {
            LinkRemovedEventTriggered = true;
        }

        private void Links_Added(Models.Base.BaseLinkModel[] obj)
        {
            LinkAddedEventTriggered = true;
        }

        private void Nodes_Removed(NodeModel[] obj)
        {
            NodeRemovedEventTriggered = true;
        }

        private void Nodes_Added(NodeModel[] obj)
        {
            NodeAddedEventTriggered = true;
        }

        public void Dispose()
        {
            Diagram.Nodes.Added -= Nodes_Added;
            Diagram.Nodes.Removed -= Nodes_Removed;
            Diagram.Links.Added -= Links_Added;
            Diagram.Links.Removed -= Links_Removed;
            Diagram.GroupAdded -= Diagram_GroupAdded;
            Diagram.GroupRemoved -= Diagram_GroupRemoved;
        }
    }
}
