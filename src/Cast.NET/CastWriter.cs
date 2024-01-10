// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2024 Philip/Scobalula
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
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace Cast.NET
{
    /// <summary>
    /// A static class that provides methods for writing <see cref="Cast"/> instances to binary streams and files.
    /// </summary>
    public static class CastWriter
    {
        /// <summary>
        /// Saves the property to the <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="writer">The writer that the cast instance is being written to.</param>
        /// <param name="name">Name of the property being written.</param>
        /// <param name="prop">The property being written.</param>
        /// <exception cref="NotSupportedException">Thrown if the property provided is of an unsupported type.</exception>
        public static void SaveProperty(BinaryWriter writer, string name, CastProperty prop)
        {
            writer.Write((ushort)prop.Identifier);
            writer.Write((ushort)name.Length);
            writer.Write(prop.ValueCount);
            writer.Write(name.AsSpan());

            switch(prop.Identifier)
            {
                case CastPropertyIdentifier.Byte     : writer.Write(MemoryMarshal.Cast<byte, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<byte>)prop).Values))); break;
                case CastPropertyIdentifier.Short    : writer.Write(MemoryMarshal.Cast<ushort, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<ushort>)prop).Values))); break;
                case CastPropertyIdentifier.Integer32: writer.Write(MemoryMarshal.Cast<uint, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<uint>)prop).Values))); break;
                case CastPropertyIdentifier.Integer64: writer.Write(MemoryMarshal.Cast<ulong, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<ulong>)prop).Values))); break;
                case CastPropertyIdentifier.Float    : writer.Write(MemoryMarshal.Cast<float, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<float>)prop).Values))); break;
                case CastPropertyIdentifier.Double   : writer.Write(MemoryMarshal.Cast<double, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<double>)prop).Values))); break;
                case CastPropertyIdentifier.Vector2  : writer.Write(MemoryMarshal.Cast<Vector2, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<Vector2>)prop).Values))); break;
                case CastPropertyIdentifier.Vector3  : writer.Write(MemoryMarshal.Cast<Vector3, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<Vector3>)prop).Values))); break;
                case CastPropertyIdentifier.Vector4  : writer.Write(MemoryMarshal.Cast<Vector4, byte>(CollectionsMarshal.AsSpan(((CastArrayProperty<Vector4>)prop).Values))); break;
                case CastPropertyIdentifier.String   : writer.Write(((CastStringProperty)prop).Value.AsSpan()); writer.Write((byte)0); break;
                default                              : throw new NotSupportedException();
            };
        }

        /// <summary>
        /// Saves the <see cref="CastNode"/> to the <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="writer">The writer that the cast instance is being written to.</param>
        /// <param name="node">The node being written.</param>
        public static void SaveNode(BinaryWriter writer, CastNode node)
        {
            writer.Write((uint)node.Identifier);
            writer.Write(node.DataSize);
            writer.Write(node.Hash);
            writer.Write(node.Properties.Count);
            writer.Write(node.Children.Count);

            foreach (var prop in node.Properties)
                SaveProperty(writer, prop.Key, prop.Value);
            foreach (var child in node.Children)
                SaveNode(writer, child);
        }

        /// <summary>
        /// Saves the <see cref="Cast"/> instance to the provided file path.
        /// </summary>
        /// <param name="filePath">The file to save the cast instance to.</param>
        /// <param name="cast">The cast instance being written.</param>
        public static void Save(string filePath, Cast cast)
        {
            using var stream = File.Create(filePath);
            Save(stream, cast);
        }

        /// <summary>
        /// Saves the <see cref="Cast"/> instance to the provided <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream the cast instance is being written to.</param>
        /// <param name="cast">The cast instance being written.</param>
        public static void Save(Stream stream, Cast cast)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, true);
            Save(writer, cast);
        }

        /// <summary>
        /// Saves the <see cref="Cast"/> instance to the provided <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="writer">The writer that the cast instance is being written to.</param>
        /// <param name="cast">The cast instance being written.</param>
        public static void Save(BinaryWriter writer, Cast cast)
        {
            writer.Write(0x74736163);
            writer.Write(0x1);
            writer.Write(cast.RootNodes.Count);
            writer.Write(0);

            foreach (var node in cast.RootNodes)
            {
                SaveNode(writer, node);
            }
        }

        /// <summary>
        /// Saves the <see cref="Cast"/> instance to the provided file path.
        /// </summary>
        /// <param name="filePath">The file to save the cast instance to.</param>
        /// <param name="root">The root node of the cast instance being written.</param>
        public static void Save(string filePath, CastNode root)
        {
            using var stream = File.Create(filePath);
            Save(stream, root);
        }

        /// <summary>
        /// Saves the <see cref="Cast"/> instance to the provided <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream the cast instance is being written to.</param>
        /// <param name="root">The root node of the cast instance being written.</param>
        public static void Save(Stream stream, CastNode root)
        {
            using var writer = new BinaryWriter(stream, Encoding.Default, true);
            Save(writer, root);
        }

        /// <summary>
        /// Saves the <see cref="Cast"/> instance to the provided <see cref="BinaryWriter"/>.
        /// </summary>
        /// <param name="writer">The writer that the cast instance is being written to.</param>
        /// <param name="root">The root node of the cast instance being written.</param>
        public static void Save(BinaryWriter writer, CastNode root)
        {
            writer.Write(0x74736163);
            writer.Write(0x1);
            writer.Write(0x1);
            writer.Write(0);
            SaveNode(writer, root);
        }
    }
}
