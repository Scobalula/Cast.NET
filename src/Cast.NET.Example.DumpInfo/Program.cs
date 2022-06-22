using Cast.NET.Nodes;
using System.CodeDom.Compiler;

namespace Cast.NET.Example.DumpInfo
{
    internal class Program
    {
        static void Dump(IndentedTextWriter writer, string name, CastProperty property)
        {
            writer.WriteLine($"Identifier: {property.Identifier}");
            writer.WriteLine($"Name: {name}");
            writer.WriteLine($"Value: {property}:{property.ValueCount}");
        }

        static void Dump(IndentedTextWriter writer, CastNode node)
        {
            writer.WriteLine($"Identifier: {node.Identifier}");
            writer.WriteLine($"Hash: 0x{node.Hash:X}");
            writer.WriteLine($"Properties: {node.Properties.Count}");

            foreach (var (key, value) in node.Properties)
            {
                writer.Indent++;
                Dump(writer, key, value);
                writer.Indent--;
            }

            writer.WriteLine($"Children: {node.Children.Count}");

            foreach (var child in node.Children)
            {
                writer.Indent++;
                Dump(writer, child);
                writer.Indent--;
            }
        }

        static void Dump(IndentedTextWriter writer, Cast cast)
        {
            writer.WriteLine($"Root Nodes: {cast.RootNodes.Count}");

            foreach (var node in cast.RootNodes)
            {
                Dump(writer, node);
            }
        }

        static void Main(string[] args)
        {
            using var indentWriter = new IndentedTextWriter(new StreamWriter(args[0] + ".txt"));
            var cast = CastReader.Load(args[0]);
            Dump(indentWriter, cast);
        }
    }
}