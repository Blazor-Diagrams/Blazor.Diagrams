﻿using Blazor.Diagrams.Core.Behaviors;
using Blazor.Diagrams.Core.Models;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Blazor.Diagrams.Core.Tests.Behaviors
{
    public class KeyboardShortcutsDefaultsTests
    {
        [Fact]
        public async Task DeleteSelection_ShouldNotDeleteModel_WhenItsLocked()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true,
                Locked = true
            });

            // Act
            await KeyboardShortcutsDefaults.DeleteSelection(diagram);

            // Assert
            diagram.Nodes.Count.Should().Be(1);
        }

        [Fact]
        public async Task DeleteSelection_ShouldTakeIntoAccountGroupConstraint()
        {
            // Arrange
            var funcCalled = false;
            var diagram = new TestDiagram();
            int iFuncCalled = 0;
            diagram.Options.Constraints.ShouldDeleteGroup = _ =>
            {
                iFuncCalled++;
                funcCalled = true;
                return ValueTask.FromResult(true);
            };
            diagram.AddGroup(new GroupModel(Array.Empty<NodeModel>())
            {
                Selected = true
            });
            diagram.AddGroup(new GroupModel(Array.Empty<NodeModel>())
            {
                Locked = true,
                Selected = true
            });

            // Act
            await KeyboardShortcutsDefaults.DeleteSelection(diagram);

            // Assert
            funcCalled.Should().BeTrue();
            diagram.Groups.Count.Should().Be(1);
            iFuncCalled.Should().Be(1);
        }

        [Fact]
        public async Task DeleteSelection_ShouldTakeIntoAccountNodeConstraint()
        {
            // Arrange
            var funcCalled = false;
            var diagram = new TestDiagram();
            int iFuncCalled = 0;
            diagram.Options.Constraints.ShouldDeleteNode = _ =>
            {
                iFuncCalled++;
                funcCalled = true;
                return ValueTask.FromResult(true);
            };
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true
            });
            diagram.Nodes.Add(new NodeModel
            {
                Selected = true,
                Locked=true
            });

            // Act
            await KeyboardShortcutsDefaults.DeleteSelection(diagram);

            // Assert
            funcCalled.Should().BeTrue();
            diagram.Nodes.Count.Should().Be(1);
            iFuncCalled.Should().Be(1);
        }

        [Fact]
        public async Task DeleteSelection_ShouldTakeIntoAccountLinkConstraint()
        {
            // Arrange
            var funcCalled = false;
            var diagram = new TestDiagram();
            int iFuncCalled = 0;
            diagram.Options.Constraints.ShouldDeleteLink = _ =>
            {
                iFuncCalled++;
                funcCalled = true;
                return ValueTask.FromResult(true);
            };
            diagram.Nodes.Add(new NodeModel[]
            {
                new NodeModel(),
                new NodeModel(),
                new NodeModel()
            });
            diagram.Links.Add(new LinkModel(diagram.Nodes[0], diagram.Nodes[1])
            {
                Selected = true
            });
            diagram.Links.Add(new LinkModel(diagram.Nodes[1], diagram.Nodes[2])
            {
                Selected = true,
                Locked = true
            });

            // Act
            await KeyboardShortcutsDefaults.DeleteSelection(diagram);

            // Assert
            funcCalled.Should().BeTrue();
            diagram.Links.Count.Should().Be(1);
            iFuncCalled.Should().Be(1);
        }

        [Fact]
        public async Task DeleteSelection_ShouldResultInSingleRefresh()
        {
            // Arrange
            var diagram = new TestDiagram();
            diagram.Nodes.Add(new NodeModel[]
            {
                new NodeModel { Selected = true },
                new NodeModel { Selected = true }
            });
            diagram.Links.Add(new LinkModel(diagram.Nodes[0], diagram.Nodes[1])
            {
                Selected = true
            });

            var refreshes = 0;
            diagram.Changed += () => refreshes++;

            // Act
            await KeyboardShortcutsDefaults.DeleteSelection(diagram);

            // Assert
            diagram.Nodes.Count.Should().Be(0);
            diagram.Links.Count.Should().Be(0);
            refreshes.Should().Be(1);
        }
    }
}
