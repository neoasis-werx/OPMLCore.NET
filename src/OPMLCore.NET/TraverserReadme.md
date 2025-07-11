# OPML Outline Traverser

The OPML Outline Traverser provides a flexible, extensible way to traverse OPML outline trees in .NET. It supports depth-first and breadth-first traversal, custom path delimiters, filtering, and both synchronous and asynchronous operations.

## Features

- Depth-first and breadth-first traversal
- Customizable visitor/callback pattern
- Path tracking with customizable delimiter
- Level/depth tracking
- Parent and sibling context
- Filtering support
- Synchronous and asynchronous traversal
- LINQ-style enumeration

## Basic Usage

### Synchronous Traversal

```csharp
var opml = ... // Load or construct your OPML object
var options = new TraversalOptions { PathDelimiter = "." };
OpmlTraverser.Traverse(opml, evt =>
{
    Console.WriteLine($"{evt.Path} (Level: {evt.Level})");
}, options);
```

### Asynchronous Traversal

```csharp
await OpmlTraverser.TraverseAsync(opml, async evt =>
{
    await SomeAsyncOperation(evt.Node);
});
```

### Filtering Nodes

```csharp
var options = new TraversalOptions
{
    Filter = evt => evt.Level <= 2 // Only visit nodes up to level 2
};
OpmlTraverser.Traverse(opml, evt =>
{
    Console.WriteLine(evt.Path);
}, options);
```

### LINQ Enumeration (All Outlines in OPML)

```csharp
foreach (var outline in OpmlTraverser.AsEnumerable(opml))
{
    Console.WriteLine(outline.Text);
}
```

### LINQ Enumeration (Single Outline Subtree)

```csharp
foreach (var outline in OutlineTraverser.AsEnumerable(opml.Body.Outlines[0]))
{
    Console.WriteLine(outline.Text);
}
```

## TraversalEvent Properties

- `Node`: The current `Outline` node
- `Level`: The depth in the tree (root is 0)
- `Path`: The full path from root to this node (delimiter is configurable)
- `Parent`: The parent `Outline` (or null for root)
- `SiblingIndex`: Index among siblings
- `Siblings`: List of sibling nodes
- `State`: Optional state object passed through traversal

## TraversalOptions Properties

- `Strategy`: `DepthFirst` or `BreadthFirst`
- `Filter`: Predicate to determine if a node should be visited
- `AllowEarlyExit`: (reserved for future use)
- `PathDelimiter`: String used to separate path segments (default: "/")

## Example: Collecting All Paths

```csharp
var paths = new List<string>();
OpmlTraverser.Traverse(opml, evt => paths.Add(evt.Path));
```

## Example: Custom Path Delimiter

```csharp
var options = new TraversalOptions { PathDelimiter = " > " };
OpmlTraverser.Traverse(opml, evt => Console.WriteLine(evt.Path), options);
```

---

For more advanced scenarios, see the unit tests in `OutlineTraverserTests.cs`.
