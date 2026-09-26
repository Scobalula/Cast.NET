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
using System.Text;
using CastNet.Nodes;

namespace CastNet;

/// <summary>
/// Loads cast files.
/// </summary>
public static class CastReader
{
    private const int MaximumDepth = 256;

    /// <summary>
    /// Loads a cast file from the given path.
    /// </summary>
    /// <param name="path">The path of the file.</param>
    /// <returns>The loaded cast.</returns>
    /// <exception cref="InvalidDataException">Thrown if the file is not a valid cast file.</exception>
    public static Cast Load(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 1 << 16, FileOptions.SequentialScan);
        return Load(stream);
    }

    /// <summary>
    /// Loads a cast file from the given stream.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <returns>The loaded cast.</returns>
    /// <exception cref="InvalidDataException">Thrown if the stream does not contain a valid cast file.</exception>
    public static Cast Load(Stream stream)
    {
        Span<byte> header = stackalloc byte[16];
        stream.ReadExactly(header);

        var magic = BinaryPrimitives.ReadUInt32LittleEndian(header);
        var version = BinaryPrimitives.ReadUInt32LittleEndian(header[4..]);
        var rootCount = BinaryPrimitives.ReadUInt32LittleEndian(header[8..]);

        if (magic != Cast.Magic)
            throw new InvalidDataException($"Invalid cast file magic: 0x{magic:X8}.");
        if (version != Cast.Version)
            throw new InvalidDataException($"Unsupported cast file version: {version}.");

        var cast = new Cast();
        var text = new byte[256];

        for (var i = 0; i < rootCount; i++)
            cast.Roots.Add(ReadNode(stream, ref text, 0, out _) as RootNode ?? throw new InvalidDataException($"Root node {i} is not a root node."));

        return cast;
    }

    private static CastNode ReadNode(Stream stream, ref byte[] text, int depth, out uint size)
    {
        if (depth > MaximumDepth)
            throw new InvalidDataException($"Nodes are nested deeper than {MaximumDepth} levels.");

        Span<byte> header = stackalloc byte[24];
        stream.ReadExactly(header);

        var identifier = (CastNodeIdentifier)BinaryPrimitives.ReadUInt32LittleEndian(header);
        var expectedSize = BinaryPrimitives.ReadUInt32LittleEndian(header[4..]);
        var hash = BinaryPrimitives.ReadUInt64LittleEndian(header[8..]);
        var propertyCount = BinaryPrimitives.ReadUInt32LittleEndian(header[16..]);
        var childCount = BinaryPrimitives.ReadUInt32LittleEndian(header[20..]);

        CastNode node = identifier switch
        {
            CastNodeIdentifier.Root => new RootNode(hash),
            CastNodeIdentifier.Model => new ModelNode(hash),
            CastNodeIdentifier.Mesh => new MeshNode(hash),
            CastNodeIdentifier.Hair => new HairNode(hash),
            CastNodeIdentifier.BlendShape => new BlendShapeNode(hash),
            CastNodeIdentifier.Skeleton => new SkeletonNode(hash),
            CastNodeIdentifier.Bone => new BoneNode(hash),
            CastNodeIdentifier.IKHandle => new IKHandleNode(hash),
            CastNodeIdentifier.Constraint => new ConstraintNode(hash),
            CastNodeIdentifier.Animation => new AnimationNode(hash),
            CastNodeIdentifier.Curve => new CurveNode(hash),
            CastNodeIdentifier.CurveModeOverride => new CurveModeOverrideNode(hash),
            CastNodeIdentifier.NotificationTrack => new NotificationTrackNode(hash),
            CastNodeIdentifier.Material => new MaterialNode(hash),
            CastNodeIdentifier.File => new FileNode(hash),
            CastNodeIdentifier.Color => new ColorNode(hash),
            CastNodeIdentifier.Instance => new InstanceNode(hash),
            CastNodeIdentifier.Metadata => new MetadataNode(hash),
            _ => new CastNode(identifier, hash),
        };

        size = (uint)header.Length;
        Span<byte> propertyHeader = stackalloc byte[8];

        for (var i = 0; i < propertyCount; i++)
        {
            stream.ReadExactly(propertyHeader);

            var type = (CastPropertyType)BinaryPrimitives.ReadUInt16LittleEndian(propertyHeader);
            var nameSize = BinaryPrimitives.ReadUInt16LittleEndian(propertyHeader[2..]);
            var count = BinaryPrimitives.ReadInt32LittleEndian(propertyHeader[4..]);

            if (text.Length < nameSize)
                text = new byte[nameSize];

            stream.ReadExactly(text, 0, nameSize);

            var name = Encoding.UTF8.GetString(text, 0, nameSize);
            size += (uint)(propertyHeader.Length + nameSize);

            if (type == CastPropertyType.String)
            {
                var length = 0;
                int value;

                while ((value = stream.ReadByte()) > 0)
                {
                    if (length == text.Length)
                        Array.Resize(ref text, length * 2);

                    text[length++] = (byte)value;
                }

                if (value < 0)
                    throw new EndOfStreamException();

                node.Properties[name] = new CastStringProperty(Encoding.UTF8.GetString(text, 0, length));
                size += (uint)length + 1;
                continue;
            }

            if (!Enum.IsDefined(type) || count < 0)
                throw new InvalidDataException($"Property {name} has an invalid type 0x{(ushort)type:X4} or count {count}.");

            var property = new CastArrayProperty(type, count);
            property.Resize(count);
            stream.ReadExactly(property.AsBytes());
            node.Properties[name] = property;
            size += (uint)property.AsBytes().Length;
        }

        for (var i = 0; i < childCount; i++)
        {
            node.AddNode(ReadNode(stream, ref text, depth + 1, out var childSize));
            size += childSize;
        }

        if (size != expectedSize)
            throw new InvalidDataException($"Node {identifier} declares a size of {expectedSize} bytes but {size} bytes were read.");

        return node;
    }
}
