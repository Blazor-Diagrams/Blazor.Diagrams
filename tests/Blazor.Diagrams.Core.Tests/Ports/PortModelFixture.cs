using System;
using System.Collections.Generic;
using System.Text;
using Blazor.Diagrams.Core.Models;

namespace Blazor.Diagrams.Core.Tests.Ports
{
    public class PortModelFixture
    {
        public readonly  NodeModel PortModelParent;
        public readonly PortModel PortModel;
        public PortModelFixture()
        {
            PortModelParent = new NodeModel();
            PortModelParent.AddPort(PortAlignment.Top);
            PortModel = new PortModel(PortModelParent);
        }

        public PortModel CreatePortModel()
        {
            return new PortModel(new NodeModel());
        }
    }
}
