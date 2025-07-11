using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OPMLCore.NET
{
    public enum TraversalStrategy
    {
        DepthFirst,
        BreadthFirst
    }

    public class TraversalOptions
    {
        public TraversalStrategy Strategy { get; set; } = TraversalStrategy.DepthFirst;
        public Func<TraversalEvent, bool> Filter { get; set; } = null;
        public bool AllowEarlyExit { get; set; } = false;
        public string PathDelimiter { get; set; } = "/";
    }

    public class TraversalEvent
    {
        public Outline Node { get; set; }
        public int Level { get; set; }
        public string Path { get; set; } = string.Empty;
        public Outline Parent { get; set; }
        public int SiblingIndex { get; set; }
        public IReadOnlyList<Outline> Siblings { get; set; }
        public object State { get; set; }
    }

    public class TraversalResult
    {
        public int NodeCount { get; set; }
        public int MaxDepth { get; set; }
        public List<Outline> VisitedNodes { get; } = new List<Outline>();
    }

    public static class OpmlTraverser
    {
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
    }

    public static class OutlineTraverser
    {
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
                TraverseDepthFirst(root, visitor, options, result, state, null, 0, "", 0, null);
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
