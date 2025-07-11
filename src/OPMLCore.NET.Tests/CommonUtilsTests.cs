namespace OPMLCore.NET.Tests
{
    public class CommonUtilsTests
    {
        [Test]
        public void TraverseOutlineDepthFirst_WithLevelAndPath_PrintsIndentedTextAndPath()
        {
            // Arrange: create a simple outline tree
            var root = new Outline { Text = "Root" };
            var child1 = new Outline { Text = "Child 1" };
            var child2 = new Outline { Text = "Child 2" };
            var grandchild = new Outline { Text = "Grandchild" };
            child1.Outlines.Add(grandchild);
            root.Outlines.Add(child1);
            root.Outlines.Add(child2);

            // Act: traverse and print indented .Text and path

            CommonUtils.TraverseOutlineDepthFirst(root, CommonUtils.PrintAction);
            Assert.Pass();

            // No Assert: this test is for demonstration/visual verification
        }

        [Test]
        public void TraverseOutlineDepthFirst_WithTraversalEvent_PrintsIndentedTextAndPath()
        {
            // Arrange: create a simple outline tree
            var root = new Outline { Text = "Root" };
            var child1 = new Outline { Text = "Child 1" };
            var child2 = new Outline { Text = "Child 2" };
            var grandchild = new Outline { Text = "Grandchild" };
            child1.Outlines.Add(grandchild);
            root.Outlines.Add(child1);
            root.Outlines.Add(child2);

            // Act: traverse and print indented .Text and path using TraversalEvent
            CommonUtils.TraverseOutlineDepthFirst(root, CommonUtils.PrintEventAction);
            Assert.Pass();
            // No Assert: this test is for demonstration/visual verification
        }
    }
}
