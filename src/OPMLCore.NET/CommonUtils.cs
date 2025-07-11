namespace OPMLCore.NET;

using System;
using System.Globalization;

public static class CommonUtils
{
    public static readonly CultureInfo MyCultureInfo = new("en-US");

    public static DateTime? ParseDateTime(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        if (DateTime.TryParse(value, MyCultureInfo, DateTimeStyles.None, out var result))
            return result;
        return null;
    }

    /// <summary>
    /// Performs a depth-first in-order traversal of the Outline tree, applying the given action to each node, with level and path tracking.
    /// </summary>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public static void TraverseOutlineDepthFirst(Outline outline, Action<Outline, int, string> action, int level = 0, string path = "")
    {
        if (outline == null || action == null) return;

        var currentPath = string.IsNullOrEmpty(path) ? outline.Text : $"{path}/{outline.Text}";

        action(outline, level, currentPath);

        if (outline.Outlines == null) return;

        foreach (var child in outline.Outlines)
        {
            TraverseOutlineDepthFirst(child, action, level + 1, currentPath);
        }
    }

    /// <summary>
    /// Performs a depth-first in-order traversal of the Outline tree, applying the given action to each node, using a TraversalEvent parameter.
    /// </summary>
    /// <exception cref="Exception">A delegate callback throws an exception.</exception>
    public static void TraverseOutlineDepthFirst(Outline outline, Action<TraversalEvent> action, int level = 0, string path = "")
    {
        if (outline == null || action == null) return;
        var currentPath = string.IsNullOrEmpty(path) ? outline.Text : $"{path}/{outline.Text}";
        var evt = new TraversalEvent { Outline = outline, Level = level, Path = currentPath };

        // Invoke the action with the current event
        // This allows the action to handle the event as needed
        action(evt);

        // Recursively traverse child outlines
        if (outline.Outlines == null) return;

        foreach (var child in outline.Outlines)
        {
            TraverseOutlineDepthFirst(child, action, level + 1, currentPath);
        }
    }

    /// <summary>
    /// Traverses all Outline entries in the Body of the given OPML document, invoking the action for each node as a TraversalEvent.
    /// </summary>
    public static void TraverseOpmlOutline(Opml opml, Action<TraversalEvent> action)
    {
        if (opml?.Body?.Outlines == null || action == null) return;

        foreach (var outline in opml.Body.Outlines)
        {
            TraverseOutlineDepthFirst(outline, action);
        }
    }

    public class TraversalEvent
    {
        public Outline Outline { get; set; }
        public int Level { get; set; }
        public string Path { get; set; }
    }

    public static void PrintAction(Outline outline, int level, string path)
    {
        var indent = new string(' ', level * 2);
        Console.WriteLine($"{indent}{outline.Text} (Level: {level}, Path: {path})");
    }

    public static void PrintEventAction(TraversalEvent evt)
    {
        var indent = new string(' ', evt.Level * 2);
        Console.WriteLine($"{indent}{evt.Outline.Text} (Level: {evt.Level}, Path: {evt.Path})");
    }
}
