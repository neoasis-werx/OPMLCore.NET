using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPMLCore.NET
{
    /// <summary>
    /// Specifies the traversal strategy for walking an OPML outline tree.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="DepthFirst"/> traverses each branch of the tree as deeply as possible before backtracking.
    /// <see cref="BreadthFirst"/> visits all nodes at the current depth before moving to the next level.
    /// </para>
    /// <para>
    /// Choose the strategy that best fits your processing needs. Depth-first is often used for recursive operations,
    /// while breadth-first is useful for level-based processing.
    /// </para>
    /// </remarks>
    public enum TraversalStrategy
    {
        DepthFirst,
        BreadthFirst
    }

    /// <summary>
    /// Options for controlling OPML outline traversal.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Strategy</b>: Determines whether traversal is depth-first or breadth-first.
    /// <b>Filter</b>: A predicate to include or exclude nodes during traversal.
    /// <b>AllowEarlyExit</b>: Reserved for future use, such as breaking traversal early.
    /// <b>PathDelimiter</b>: The string used to join outline node names in the path (default is "/").
    /// </para>
    /// <para>
    /// These options allow you to tailor traversal to your needs, such as limiting depth, customizing path formatting, or skipping nodes.
    /// </para>
    /// </remarks>
    public class TraversalOptions
    {
        /// <summary>
        /// The traversal strategy to use (depth-first or breadth-first).
        /// </summary>
        public TraversalStrategy Strategy { get; set; } = TraversalStrategy.DepthFirst;

        /// <summary>
        /// Predicate to filter nodes during traversal. If null, all nodes are included.
        /// </summary>
        public Func<TraversalEvent, bool> Filter { get; set; } = null;

        /// <summary>
        /// Reserved for future use. Allows traversal to exit early if set.
        /// </summary>
        public bool AllowEarlyExit { get; set; } = false;

        /// <summary>
        /// The delimiter used to join node names in the path.
        /// </summary>
        public string PathDelimiter { get; set; } = "/";
    }

    /// <summary>
    /// Event data provided to traversal callbacks for each visited outline node.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Node</b>: The current outline node being visited.
    /// <b>Level</b>: The depth of the node in the tree (root is 0).
    /// <b>Path</b>: The full path from the root to this node, using the configured delimiter.
    /// <b>Parent</b>: The parent node (null for root).
    /// <b>SiblingIndex</b>: The index of this node among its siblings.
    /// <b>Siblings</b>: The list of sibling nodes at this level.
    /// <b>State</b>: Optional state object passed through traversal for aggregation or context.
    /// </para>
    /// <para>
    /// Use these properties to implement custom logic, aggregation, or reporting during traversal. For example, you can build a list of all paths, count nodes at each level, or collect specific nodes based on their properties.
    /// </para>
    /// </remarks>
    public class TraversalEvent
    {
        /// <summary>
        /// The current outline node being visited.
        /// </summary>
        public Outline Node { get; set; }

        /// <summary>
        /// The depth of the node in the tree (root is 0).
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// The full path from the root to this node, using the configured delimiter.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// The parent node (null for root).
        /// </summary>
        public Outline Parent { get; set; }

        /// <summary>
        /// The index of this node among its siblings.
        /// </summary>
        public int SiblingIndex { get; set; }

        /// <summary>
        /// The list of sibling nodes at this level.
        /// </summary>
        public IReadOnlyList<Outline> Siblings { get; set; }

        /// <summary>
        /// Optional state object passed through traversal for aggregation or context.
        /// </summary>
        public object State { get; set; }
    }

    /// <summary>
    /// Result object returned from a traversal, containing statistics and visited nodes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>NodeCount</b>: The total number of nodes visited during traversal.
    /// <b>MaxDepth</b>: The maximum depth reached in the tree.
    /// <b>VisitedNodes</b>: The list of all nodes visited, in traversal order.
    /// </para>
    /// <para>
    /// Use this object to analyze the structure or contents of an OPML outline after traversal, such as for reporting, validation, or further processing.
    /// </para>
    /// </remarks>
    public class TraversalResult
    {
        /// <summary>
        /// The total number of nodes visited during traversal.
        /// </summary>
        public int NodeCount { get; set; }

        /// <summary>
        /// The maximum depth reached in the tree.
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// The list of all nodes visited, in traversal order.
        /// </summary>
        public List<Outline> VisitedNodes { get; } = new List<Outline>();
    }

    /// <summary>
    /// Provides static methods for traversing OPML documents and outlines.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="OpmlTraverser"/> class offers entry points for traversing an entire OPML document, either synchronously or asynchronously.
    /// It supports both callback-based and enumerable-based traversal, and allows customization via <see cref="TraversalOptions"/>.
    /// </para>
    /// <para>
    /// Use this class to process all outlines in an OPML document, collect statistics, or perform custom actions on each node.
    /// </para>
    /// </remarks>
    public static class OpmlTraverser
    {
        /// <summary>
        /// Traverses all outlines in the given OPML document using the specified visitor callback.
        /// </summary>
        /// <param name="opml">The OPML document to traverse.</param>
        /// <param name="visitor">A callback invoked for each visited node.</param>
        /// <param name="options">Traversal options (optional).</param>
        /// <param name="state">Optional state object passed to each callback.</param>
        /// <returns>A <see cref="TraversalResult"/> containing statistics and visited nodes.</returns>
        /// <remarks>
        /// <para>
        /// This method traverses all top-level outlines in the OPML body, invoking the visitor for each node.
        /// The traversal strategy and filtering can be customized via <paramref name="options"/>.
        /// </para>
        /// </remarks>
        public static TraversalResult Traverse(
            Opml opml,
            Action<TraversalEvent> visitor,
            TraversalOptions options = null,
            object state = null)
        {
            options = options ?? new TraversalOptions();
            var result = new TraversalResult();
            if (opml?.Body?.Outlines == null) return result;

            foreach (var outline in opml.Body.Outlines)
            {
                OutlineTraverser.Traverse(outline, visitor, options, state);
            }

            return result;
        }

        /// <summary>
        /// Asynchronously traverses all outlines in the given OPML document using the specified visitor callback.
        /// </summary>
        /// <param name="opml">The OPML document to traverse.</param>
        /// <param name="visitor">An asynchronous callback invoked for each visited node.</param>
        /// <param name="options">Traversal options (optional).</param>
        /// <param name="state">Optional state object passed to each callback.</param>
        /// <remarks>
        /// <para>
        /// This method is suitable for scenarios where node processing involves asynchronous operations, such as I/O or network calls.
        /// </para>
        /// </remarks>
        public static async Task TraverseAsync(
            Opml opml,
            Func<TraversalEvent, Task> visitor,
            TraversalOptions options = null,
            object state = null)
        {
            options = options ?? new TraversalOptions();
            if (opml == null || opml.Body == null || opml.Body.Outlines == null) return;
            foreach (var outline in opml.Body.Outlines)
            {
                await OutlineTraverser.TraverseAsync(outline, visitor, options, state);
            }
        }

        /// <summary>
        /// Enumerates all nodes in the OPML document as a flat sequence.
        /// </summary>
        /// <param name="opml">The OPML document to enumerate.</param>
        /// <param name="options">Traversal options (optional).</param>
        /// <returns>An enumerable of all outline nodes in the document.</returns>
        /// <remarks>
        /// <para>
        /// This method allows you to use LINQ or foreach to process all nodes in the OPML document.
        /// The traversal strategy and filtering can be customized via <paramref name="options"/>.
        /// </para>
        /// </remarks>
        public static IEnumerable<Outline> AsEnumerable(Opml opml, TraversalOptions options = null)
        {
            if (opml?.Body?.Outlines == null)
                yield break;
            options = options ?? new TraversalOptions();
            foreach (var outline in opml.Body.Outlines)
            {
                foreach (var node in OutlineTraverser.AsEnumerable(outline, options))
                {
                    yield return node;
                }
            }
        }
    }

    /// <summary>
    /// Provides static methods for traversing outline trees.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="OutlineTraverser"/> class implements the core traversal logic for outline trees, supporting both depth-first and breadth-first strategies.
    /// It offers synchronous, asynchronous, and enumerable traversal methods, and allows customization via <see cref="TraversalOptions"/>.
    /// </para>
    /// <para>
    /// Use this class to process, enumerate, or analyze outline trees at any level of the OPML document.
    /// </para>
    /// </remarks>
    public static class OutlineTraverser
    {
        /// <summary>
        /// Traverses the outline tree rooted at <paramref name="root"/> using the specified visitor callback.
        /// </summary>
        /// <param name="root">The root outline node to traverse.</param>
        /// <param name="visitor">A callback invoked for each visited node.</param>
        /// <param name="options">Traversal options (optional).</param>
        /// <param name="state">Optional state object passed to each callback.</param>
        /// <returns>A <see cref="TraversalResult"/> containing statistics and visited nodes.</returns>
        /// <remarks>
        /// <para>
        /// This method supports both depth-first and breadth-first traversal, as specified in <paramref name="options"/>.
        /// The visitor is called for each node that passes the filter predicate.
        /// </para>
        /// </remarks>
        public static TraversalResult Traverse(
            Outline root,
            Action<TraversalEvent> visitor,
            TraversalOptions options = null,
            object state = null)
        {
            options = options ?? new TraversalOptions();
            var result = new TraversalResult();
            if (options.Strategy == TraversalStrategy.DepthFirst)
            {
                TraverseDepthFirst(root, visitor, options, result, state, null, 0, string.Empty, 0, null);
            }
            else
            {
                TraverseBreadthFirst(root, visitor, options, result, state);
            }
            return result;
        }

        private static void TraverseDepthFirst(
            Outline node,
            Action<TraversalEvent> visitor,
            TraversalOptions options,
            TraversalResult result,
            object state,
            Outline parent,
            int level,
            string path,
            int siblingIndex,
            IReadOnlyList<Outline> siblings)
        {
            if (node == null) return;
            var delimiter = options.PathDelimiter ?? "/";
            var currentPath = string.IsNullOrEmpty(path) ? node.Text : $"{path}{delimiter}{node.Text}";
            var evt = new TraversalEvent
            {
                Node = node,
                Level = level,
                Path = currentPath,
                Parent = parent,
                SiblingIndex = siblingIndex,
                Siblings = siblings,
                State = state
            };
            if (options.Filter == null || options.Filter(evt))
            {
                visitor(evt);
                result.VisitedNodes.Add(node);
                result.NodeCount++;
                result.MaxDepth = Math.Max(result.MaxDepth, level);
            }
            if (node.Outlines != null)
            {
                for (int i = 0; i < node.Outlines.Count; i++)
                {
                    TraverseDepthFirst(node.Outlines[i], visitor, options, result, state, node, level + 1, currentPath, i, node.Outlines);
                }
            }
        }

        private static void TraverseBreadthFirst(
            Outline root,
            Action<TraversalEvent> visitor,
            TraversalOptions options,
            TraversalResult result,
            object state)
        {
            var delimiter = options.PathDelimiter ?? "/";
            var queue = new Queue<(Outline node, int level, string path, Outline parent, int siblingIndex, IReadOnlyList<Outline> siblings)>();
            queue.Enqueue((root, 0, "", null, 0, null));
            while (queue.Count > 0)
            {
                var (node, level, path, parent, siblingIndex, siblings) = queue.Dequeue();
                var currentPath = string.IsNullOrEmpty(path) ? node.Text : $"{path}{delimiter}{node.Text}";
                var evt = new TraversalEvent
                {
                    Node = node,
                    Level = level,
                    Path = currentPath,
                    Parent = parent,
                    SiblingIndex = siblingIndex,
                    Siblings = siblings,
                    State = state
                };
                if (options.Filter == null || options.Filter(evt))
                {
                    visitor(evt);
                    result.VisitedNodes.Add(node);
                    result.NodeCount++;
                    result.MaxDepth = Math.Max(result.MaxDepth, level);
                }
                if (node.Outlines != null)
                {
                    for (int i = 0; i < node.Outlines.Count; i++)
                    {
                        queue.Enqueue((node.Outlines[i], level + 1, currentPath, node, i, node.Outlines));
                    }
                }
            }
        }

        /// <summary>
        /// Asynchronously traverses the outline tree rooted at <paramref name="root"/> using the specified visitor callback.
        /// </summary>
        /// <param name="root">The root outline node to traverse.</param>
        /// <param name="visitor">An asynchronous callback invoked for each visited node.</param>
        /// <param name="options">Traversal options (optional).</param>
        /// <param name="state">Optional state object passed to each callback.</param>
        /// <remarks>
        /// <para>
        /// This method is suitable for scenarios where node processing involves asynchronous operations, such as I/O or network calls.
        /// The traversal strategy and filtering can be customized via <paramref name="options"/>.
        /// </para>
        /// </remarks>
        public static async Task TraverseAsync(
            Outline root,
            Func<TraversalEvent, Task> visitor,
            TraversalOptions options = null,
            object state = null)
        {
            options = options ?? new TraversalOptions();
            if (options.Strategy == TraversalStrategy.DepthFirst)
            {
                await TraverseDepthFirstAsync(root, visitor, options, state, null, 0, "", 0, null);
            }
            else
            {
                await TraverseBreadthFirstAsync(root, visitor, options, state);
            }
        }

        private static async Task TraverseDepthFirstAsync(
            Outline node,
            Func<TraversalEvent, Task> visitor,
            TraversalOptions options,
            object state,
            Outline parent,
            int level,
            string path,
            int siblingIndex,
            IReadOnlyList<Outline> siblings)
        {
            if (node == null) return;
            var delimiter = options.PathDelimiter ?? "/";
            var currentPath = string.IsNullOrEmpty(path) ? node.Text : $"{path}{delimiter}{node.Text}";
            var evt = new TraversalEvent
            {
                Node = node,
                Level = level,
                Path = currentPath,
                Parent = parent,
                SiblingIndex = siblingIndex,
                Siblings = siblings,
                State = state
            };
            if (options.Filter == null || options.Filter(evt))
            {
                await visitor(evt);
            }
            if (node.Outlines != null)
            {
                for (int i = 0; i < node.Outlines.Count; i++)
                {
                    await TraverseDepthFirstAsync(node.Outlines[i], visitor, options, state, node, level + 1, currentPath, i, node.Outlines);
                }
            }
        }

        private static async Task TraverseBreadthFirstAsync(
            Outline root,
            Func<TraversalEvent, Task> visitor,
            TraversalOptions options,
            object state)
        {
            var delimiter = options.PathDelimiter ?? "/";
            var queue = new Queue<(Outline node, int level, string path, Outline parent, int siblingIndex, IReadOnlyList<Outline> siblings)>();
            queue.Enqueue((root, 0, "", null, 0, null));
            while (queue.Count > 0)
            {
                var (node, level, path, parent, siblingIndex, siblings) = queue.Dequeue();
                var currentPath = string.IsNullOrEmpty(path) ? node.Text : $"{path}{delimiter}{node.Text}";
                var evt = new TraversalEvent
                {
                    Node = node,
                    Level = level,
                    Path = currentPath,
                    Parent = parent,
                    SiblingIndex = siblingIndex,
                    Siblings = siblings,
                    State = state
                };
                if (options.Filter == null || options.Filter(evt))
                {
                    await visitor(evt);
                }
                if (node.Outlines != null)
                {
                    for (int i = 0; i < node.Outlines.Count; i++)
                    {
                        queue.Enqueue((node.Outlines[i], level + 1, currentPath, node, i, node.Outlines));
                    }
                }
            }
        }

        /// <summary>
        /// Enumerates all nodes in the outline subtree rooted at <paramref name="root"/> as a flat sequence.
        /// </summary>
        /// <param name="root">The root outline node to enumerate.</param>
        /// <param name="options">Traversal options (optional).</param>
        /// <returns>An enumerable of all outline nodes in the subtree.</returns>
        /// <remarks>
        /// <para>
        /// This method performs a traversal (depth-first or breadth-first, as specified in <paramref name="options"/>)
        /// and yields each node in the order visited. You can use this for LINQ queries, filtering, or to process all nodes
        /// without writing a callback.
        /// </para>
        /// <para>
        /// The <see cref="TraversalOptions.Filter"/> predicate is respected, so only nodes matching the filter are yielded.
        /// The <see cref="TraversalOptions.PathDelimiter"/> is used to build the <c>Path</c> property of each <see cref="TraversalEvent"/>.
        /// </para>
        /// <para>
        /// Example:
        /// <code>
        /// foreach (var outline in OutlineTraverser.AsEnumerable(root, new TraversalOptions { PathDelimiter = "." }))
        /// {
        ///     Console.WriteLine(outline.Text);
        /// }
        /// </code>
        /// </para>
        /// </remarks>
        public static IEnumerable<Outline> AsEnumerable(Outline root, TraversalOptions options = null)
        {
            options = options ?? new TraversalOptions();
            var delimiter = options.PathDelimiter ?? "/";
            var stack = new Stack<(Outline node, int level, string path, Outline parent, int siblingIndex, IReadOnlyList<Outline> siblings)>();
            stack.Push((root, 0, "", null, 0, null));
            while (stack.Count > 0)
            {
                var (node, level, path, parent, siblingIndex, siblings) = stack.Pop();
                var currentPath = string.IsNullOrEmpty(path) ? node.Text : $"{path}{delimiter}{node.Text}";
                var evt = new TraversalEvent
                {
                    Node = node,
                    Level = level,
                    Path = currentPath,
                    Parent = parent,
                    SiblingIndex = siblingIndex,
                    Siblings = siblings
                };
                if (options.Filter == null || options.Filter(evt))
                {
                    yield return node;
                }
                if (node.Outlines != null)
                {
                    for (int i = node.Outlines.Count - 1; i >= 0; i--)
                    {
                        stack.Push((node.Outlines[i], level + 1, currentPath, node, i, node.Outlines));
                    }
                }
            }
        }
    }
}