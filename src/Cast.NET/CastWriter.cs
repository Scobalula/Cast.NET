// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2026 Philip/Scobalula
// ------------------------------------------------------------------------
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// ------------------------------------------------------------------------
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// ------------------------------------------------------------------------
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ------------------------------------------------------------------------
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Text;
using CastNet.Nodes;

namespace CastNet;

/// <summary>
/// Saves cast files.
/// </summary>
public static class CastWriter
{
    /// <summary>
    /// Saves the cast to the given path.
    /// </summary>
    /// <param name="path">The path of the file.</param>
    /// <param name="cast">The cast to save.</param>
    public static void Save(string path, Cast cast)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1 << 16);
        Save(stream, cast);
    }

    /// <summary>
    /// Saves the cast to the given stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="cast">The cast to save.</param>
    public static void Save(Stream stream, Cast cast) => Save(stream, CollectionsMarshal.AsSpan(cast.Roots));

    /// <summary>
    /// Saves the root node as a cast file to the given path.
    /// </summary>
    /// <param name="path">The path of the file.</param>
    /// <param name="root">The root node to save.</param>
    public static void Save(string path, RootNode root)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1 << 16);
        Save(stream, root);
    }

    /// <summary>
    /// Saves the root node as a cast file to the given stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="root">The root node to save.</param>
    public static void Save(Stream stream, RootNode root) => Save(stream, new ReadOnlySpan<RootNode>(in root));

    private static void Save(Stream stream, ReadOnlySpan<RootNode> roots)
    {
        var nodeSizes = new List<uint>();
        var nodeIndex = 0;
        var text = new byte[256];

        foreach (var root in roots)
            Measure(root, nodeSizes);

        Span<byte> header = stackalloc byte[16];
        BinaryPrimitives.WriteUInt32LittleEndian(header, Cast.Magic);
        BinaryPrimitives.WriteUInt32LittleEndian(header[4..], Cast.Version);
        BinaryPrimitives.WriteInt32LittleEndian(header[8..], roots.Length);
        BinaryPrimitives.WriteUInt32LittleEndian(header[12..], 0);
        stream.Write(header);

        foreach (var root in roots)
            WriteNode(stream, root, nodeSizes, ref nodeIndex, ref text);
    }

    private static uint Measure(CastNode node, List<uint> nodeSizes)
    {
        var slot = nodeSizes.Count;
        var size = 24u;

        nodeSizes.Add(0);

        foreach (var (name, property) in node.Properties)
        {
            var dataSize = property switch
            {
                CastStringProperty text => Encoding.UTF8.GetByteCount(text.Value) + 1,
                CastArrayProperty array => array.AsBytes().Length,
                _ => throw new NotSupportedException($"Property type {property.GetType().Name} is not supported."),
            };

            size += (uint)(8 + Encoding.UTF8.GetByteCount(name) + dataSize);
        }

        for (var i = 0; i < node.Children.Count; i++)
            size += Measure(node.Children[i], nodeSizes);

        nodeSizes[slot] = size;
        return size;
    }

    private static void WriteNode(Stream stream, CastNode node, List<uint> nodeSizes, ref int nodeIndex, ref byte[] text)
    {
        Span<byte> header = stackalloc byte[24];
        BinaryPrimitives.WriteUInt32LittleEndian(header, (uint)node.Identifier);
        BinaryPrimitives.WriteUInt32LittleEndian(header[4..], nodeSizes[nodeIndex++]);
        BinaryPrimitives.WriteUInt64LittleEndian(header[8..], node.Hash);
        BinaryPrimitives.WriteInt32LittleEndian(header[16..], node.Properties.Count);
        BinaryPrimitives.WriteInt32LittleEndian(header[20..], node.Children.Count);
        stream.Write(header);

        Span<byte> propertyHeader = stackalloc byte[8];

        foreach (var (name, property) in node.Properties)
        {
            var nameSize = EncodeText(name, ref text);

            if (nameSize > ushort.MaxValue)
                throw new InvalidDataException($"Property name {name} exceeds {ushort.MaxValue} bytes.");

            BinaryPrimitives.WriteUInt16LittleEndian(propertyHeader, (ushort)property.Type);
            BinaryPrimitives.WriteUInt16LittleEndian(propertyHeader[2..], (ushort)nameSize);
            BinaryPrimitives.WriteInt32LittleEndian(propertyHeader[4..], property.Count);
            stream.Write(propertyHeader);
            stream.Write(text, 0, nameSize);

            if (property is CastStringProperty value)
            {
                stream.Write(text, 0, EncodeText(value.Value, ref text));
                stream.WriteByte(0);
            }
            else
            {
                stream.Write(((CastArrayProperty)property).AsBytes());
            }
        }

        for (var i = 0; i < node.Children.Count; i++)
            WriteNode(stream, node.Children[i], nodeSizes, ref nodeIndex, ref text);
    }

    private static int EncodeText(string value, ref byte[] text)
    {
        var maximumSize = Encoding.UTF8.GetMaxByteCount(value.Length);

        if (text.Length < maximumSize)
            text = new byte[maximumSize];

        return Encoding.UTF8.GetBytes(value, text);
    }
}
