using System.CodeDom.Compiler;

namespace CastNet.Examples.DumpInfo;

/// <summary>
/// The entry point of the example.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Runs the example.
    /// </summary>
    /// <param name="args">The files to process.</param>
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: Cast.NET.Examples.DumpInfo <file.cast>...");
            return;
        }

        foreach (var path in args)
        {
            using var writer = new IndentedTextWriter(new StreamWriter(path + ".txt"));
            var file = CastReader.Load(path);

            writer.WriteLine($"Roots: {file.Roots.Count}");

            foreach (var root in file.Roots)
                DumpNode(writer, root);

            Console.WriteLine($"Dumped: {path}.txt");
        }
    }

    private static void DumpNode(IndentedTextWriter writer, CastNode node)
    {
        writer.WriteLine($"{node.Identifier} (Hash: 0x{node.Hash:X16}, Type: {node.GetType().Name})");
        writer.Indent++;

        foreach (var (name, property) in node.Properties)
            writer.WriteLine($"{name}: {property}");

        foreach (var child in node.Children)
            DumpNode(writer, child);

        writer.Indent--;
    }
}
