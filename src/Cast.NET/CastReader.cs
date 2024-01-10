
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
using System.Text;
using Cast.NET.Nodes;

namespace Cast.NET
{
    /// <summary>
    /// A static class that provides methods for reading <see cref="Cast"/> instances from binary streams and files.
    /// </summary>
    public static class CastReader
    {
        /// <summary>
        /// Loads the <see cref="CastProperty"/> from the <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="reader">The reader the cast instance is being read from.</param>
        /// <returns>The name and instance of the property.</returns>
        /// <exception cref="NotSupportedException">Thrown if the property provided is of an unsupported type.</exception>
        public static (string, CastProperty) LoadProperty(BinaryReader reader)
        {
            var type = (CastPropertyIdentifier)reader.ReadUInt16();
            var nameSize = reader.ReadUInt16();
            var valueCount = reader.ReadInt32();
            var name = new string(reader.ReadChars(nameSize));

            // Strings are special case, single value
            if(type == CastPropertyIdentifier.String)
            {
                var str = new StringBuilder();
                int byteRead;

                while ((byteRead = reader.BaseStream.ReadByte()) != 0x0)
                    str.Append(Convert.ToChar(byteRead));

                return (name, new CastStringProperty(str));
            }
            else
            {
                return type switch
                {
                    CastPropertyIdentifier.Byte      => (name, new CastArrayProperty<byte>(CastHelpers.ReadList<byte>(reader, valueCount))),
                    CastPropertyIdentifier.Short     => (name, new CastArrayProperty<ushort>(CastHelpers.ReadList<ushort>(reader, valueCount))),
                    CastPropertyIdentifier.Integer32 => (name, new CastArrayProperty<uint>(CastHelpers.ReadList<uint>(reader, valueCount))),
                    CastPropertyIdentifier.Integer64 => (name, new CastArrayProperty<ulong>(CastHelpers.ReadList<ulong>(reader, valueCount))),
                    CastPropertyIdentifier.Float     => (name, new CastArrayProperty<float>(CastHelpers.ReadList<float>(reader, valueCount))),
                    CastPropertyIdentifier.Double    => (name, new CastArrayProperty<double>(CastHelpers.ReadList<double>(reader, valueCount))),
                    CastPropertyIdentifier.Vector2   => (name, new CastArrayProperty<Vector2>(CastHelpers.ReadList<Vector2>(reader, valueCount))),
                    CastPropertyIdentifier.Vector3   => (name, new CastArrayProperty<Vector3>(CastHelpers.ReadList<Vector3>(reader, valueCount))),
                    CastPropertyIdentifier.Vector4   => (name, new CastArrayProperty<Vector4>(CastHelpers.ReadList<Vector4>(reader, valueCount))),
                    _                                => throw new NotSupportedException(),
                };
            }
        }

        /// <summary>
        /// Loads the <see cref="CastNode"/> from the <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="reader">The reader the cast instance is being read from.</param>
        /// <returns>The node read from the reader.</returns>
        /// <exception cref="InvalidDataException">Thrown if the data read does not match the size of the node.</exception>
        public static CastNode LoadNode(BinaryReader reader)
        {
            // Store for validation
            var currentPosition = reader.BaseStream.Position;

            var identifier = (CastNodeIdentifier)reader.ReadUInt32();
            var nodeSize = reader.ReadUInt32();
            var nodeHash = reader.ReadUInt64();
            var propCount = reader.ReadInt32();
            var childCount = reader.ReadInt32();

            var newNode = identifier switch
            {
                CastNodeIdentifier.Root                    => new CastNode(identifier, nodeHash),
                CastNodeIdentifier.Curve                   => new CurveNode(identifier, nodeHash),
                CastNodeIdentifier.Bone                    => new BoneNode(identifier, nodeHash),
                CastNodeIdentifier.IKHandle                => new IKHandleNode(identifier, nodeHash),
                CastNodeIdentifier.Constraint              => new ConstraintNode(identifier, nodeHash),
                CastNodeIdentifier.Skeleton                => new SkeletonNode(identifier, nodeHash),
                CastNodeIdentifier.Model                   => new ModelNode(identifier, nodeHash),
                CastNodeIdentifier.Mesh                    => new MeshNode(identifier, nodeHash),
                CastNodeIdentifier.BlendShape              => new BlendShapeNode(identifier, nodeHash),
                CastNodeIdentifier.Animation               => new AnimationNode(identifier, nodeHash),
                CastNodeIdentifier.NotificationTrack       => new NotificationTrackNode(identifier, nodeHash),
                CastNodeIdentifier.Material                => new MaterialNode(identifier, nodeHash),
                CastNodeIdentifier.File                    => new FileNode(identifier, nodeHash),
                _                                          => new CastNode(identifier, nodeHash),
            };

            for (uint i = 0; i < propCount; i++)
            {
                var (name, prop) = LoadProperty(reader);
                newNode.Properties[name] = prop;
            }

            for (uint i = 0; i < childCount; i++)
            {
                LoadNode(reader).Parent = newNode;
            }

            if (reader.BaseStream.Position != currentPosition + nodeSize)
                throw new InvalidDataException("The stream position does not match the node end position.");

            return newNode;
        }

        /// <summary>
        /// Loads the <see cref="CastNode"/> from the file path.
        /// </summary>
        /// <param name="filePath">The file path to read the cast instance from.</param>
        /// <returns>The cast instance read.</returns>
        public static Cast Load(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            return Load(stream);
        }

        /// <summary>
        /// Loads the <see cref="CastNode"/> from the <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to read the cast instance from.</param>
        /// <returns>The cast instance read.</returns>
        public static Cast Load(Stream stream)
        {
            using var reader = new BinaryReader(stream, Encoding.Default, true);
            return Load(reader);
        }

        /// <summary>
        /// Loads the <see cref="CastNode"/> from the <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="reader">The reader to read the cast instance from.</param>
        /// <returns>The cast instance read.</returns>
        /// <exception cref="NotSupportedException">Thrown if the magic or version number does not match.</exception>
        public static Cast Load(BinaryReader reader)
        {
            var magic     = reader.ReadInt32();
            var version   = reader.ReadInt32();
            var rootCount = reader.ReadInt32();
            var _         = reader.ReadUInt32();

            if (magic != 0x74736163)
                throw new NotSupportedException($"Invalid cast file magic: {magic}");
            if (version > 0x1)
                throw new NotSupportedException($"Invalid cast file version: {version}");

            var rootNodes = new List<CastNode>(rootCount);

            for (int i = 0; i < rootCount; i++)
            {
                rootNodes.Add(LoadNode(reader));
            }

            return new Cast(rootNodes);
        }
    }
}
