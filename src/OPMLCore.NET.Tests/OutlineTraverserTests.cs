using System;
using System.Collections.Generic;
using NUnit.Framework;
using OPMLCore.NET;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace OPMLCore.NET.Tests
{
    public class OutlineTraverserTests
    {
        [Test]
        public void Traverse_DepthFirst_CollectsAllNodesAndPaths()
        {
            // Arrange: create a simple OPML tree
            var root = new Outline { Text = "Root" };
            var child1 = new Outline { Text = "Child 1" };
            var child2 = new Outline { Text = "Child 2" };
            var grandchild = new Outline { Text = "Grandchild" };
            child1.Outlines.Add(grandchild);
            root.Outlines.Add(child1);
            root.Outlines.Add(child2);
            var opml = new Opml { Body = new Body() };
            opml.Body.Outlines.Add(root);

            var visited = new List<string>();

            // Act: Traverse and collect node text with path
            OpmlTraverser.Traverse(opml, evt =>
            {
                visited.Add($"{evt.Path}");
            });

            // Assert: All nodes and paths are visited in depth-first order
            CollectionAssert.AreEqual(
                new[] { "Root", "Root/Child 1", "Root/Child 1/Grandchild", "Root/Child 2" },
                visited
            );
        }

        [Test]
        public async Task TraverseAsync_DepthFirst_CollectsAllNodesAsync()
        {
            // Arrange
            var root = new Outline { Text = "Root" };
            var child1 = new Outline { Text = "Child 1" };
            var child2 = new Outline { Text = "Child 2" };
            var grandchild = new Outline { Text = "Grandchild" };
            child1.Outlines.Add(grandchild);
            root.Outlines.Add(child1);
            root.Outlines.Add(child2);
            var opml = new Opml { Body = new Body() };
            opml.Body.Outlines.Add(root);

            var visited = new List<string>();

            // Act
            await OpmlTraverser.TraverseAsync(opml, async evt =>
            {
                visited.Add(evt.Path);
                await Task.CompletedTask;
            });

            // Assert
            CollectionAssert.AreEqual(
                new[] { "Root", "Root/Child 1", "Root/Child 1/Grandchild", "Root/Child 2" },
                visited
            );
        }
    }
}
