using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.Diagrams.Core.Models.Tests.Groups
{
    public class GroupModelFixture : IDisposable
    {
        public GroupModel GroupModel;
        public readonly NodeModel[] Nodes;
        public bool ChangeEventTriggered;

        public GroupModelFixture()
        {
            Nodes = new List<NodeModel>()
            {
                new NodeModel(),
                new NodeModel()
            }.ToArray();

            GroupModel = new GroupModel(Nodes);
            GroupModel.Changed += OnGroupModelChanged;
        }
        void OnGroupModelChanged()
        {
            ChangeEventTriggered = true;
        }

        public void Dispose()
        {
            GroupModel.Changed -= OnGroupModelChanged;
        }
    }
}
