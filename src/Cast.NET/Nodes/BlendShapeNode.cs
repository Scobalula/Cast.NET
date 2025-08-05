// ------------------------------------------------------------------------
// Cast.NET - A .NET Library for reading and writing Cast files.
// Copyright(c) 2025 Philip/Scobalula
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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Blend Shape.
    /// </summary>
    public class BlendShapeNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of this blend shape.
        /// </summary>
        public string Name { get => GetStringValue("n", string.Empty); set => AddString("n", value); }

        /// <summary>
        /// Gets the hash of the base shape.
        /// </summary>
        public ulong BaseShapeHash { get => GetFirstValue<ulong>("b"); set => AddValue("b", value); }

        /// <summary>
        /// Gets or Sets the base <see cref="MeshNode"/>.
        /// </summary>
        public MeshNode BaseShape { get => Parent?.TryGetChild<MeshNode>(BaseShapeHash, out var node) == true ? node : throw new KeyNotFoundException(); set { BaseShapeHash = value.Hash; } }

        /// <summary>
        /// Gets or Sets the raw vertex index buffer.
        /// </summary>
        public CastProperty TargetShapeVertexIndices { get => GetProperty("vi"); set => Properties["vi"] = value; }

        /// <summary>
        /// Gets or Sets the raw vertex positions buffer stored within this blend shape.
        /// </summary>
        public CastArrayProperty<Vector3> TargetShapeVertexPositions { get => GetArrayProperty<Vector3>("vp"); set => Properties["vp"] = value; }

        /// <summary>
        /// Gets or Sets the weight of this constraint.
        /// </summary>
        public float Weight { get => GetFirstValue("ts", 1.0f); set => AddValue("ts", value); }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendShapeNode"/> class.
        /// </summary>
        public BlendShapeNode() : base(CastNodeIdentifier.BlendShape) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendShapeNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public BlendShapeNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendShapeNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public BlendShapeNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendShapeNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public BlendShapeNode(ulong hash) : base(CastNodeIdentifier.BlendShape, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendShapeNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public BlendShapeNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.BlendShape, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public BlendShapeNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendShapeNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public BlendShapeNode(CastNode source) : base(source) { }

        /// <summary>
        /// Enumerates through vertex indices.
        /// </summary>
        /// <returns>An enumerable collection of vertex indices.</returns>
        /// <exception cref="NotImplementedException">Thrown if the underlying index buffer not supported.</exception>
        public IEnumerable<int> EnumerateVertexIndices()
        {
            return TargetShapeVertexIndices switch
            {
                CastArrayProperty<byte> { Values: var v } => v.Select(x => (int)x),
                CastArrayProperty<ushort> { Values: var v } => v.Select(x => (int)x),
                CastArrayProperty<uint> { Values: var v } => v.Select(x => (int)x),
                _ => throw new NotImplementedException($"Unsupported buffer type {TargetShapeVertexIndices.GetType()}")
            };
        }

        /// <summary>
        /// Enumerates through vertex indices and their corrosponding positions.
        /// </summary>
        /// <returns>An enumerable collection of vertex indices and their corrosponding positions.</returns>
        /// <exception cref="DataMisalignedException">Thrown if the index and position buffer have different value counts.</exception>
        /// <exception cref="NotImplementedException">Thrown if the underlying index buffer not supported.</exception>
        public IEnumerable<(int, Vector3)> EnumerateVertices()
        {
            var targetShapeVertexIndices = TargetShapeVertexIndices;
            var targetShapeVertexPositions = TargetShapeVertexPositions;

            if (targetShapeVertexIndices.ValueCount != targetShapeVertexPositions.ValueCount)
                throw new DataMisalignedException($"TargetShapeVertexIndices and TargetShapeVertexPositions have different value counts.");

            return targetShapeVertexIndices switch
            {
                CastArrayProperty<byte> { Values: var v } => v.Select(x => (int)x).Zip(targetShapeVertexPositions.Values),
                CastArrayProperty<ushort> { Values: var v } => v.Select(x => (int)x).Zip(targetShapeVertexPositions.Values),
                CastArrayProperty<uint> { Values: var v } => v.Select(x => (int)x).Zip(targetShapeVertexPositions.Values),
                _ => throw new NotImplementedException($"Unsupported buffer type {TargetShapeVertexIndices.GetType()}")
            };
        }

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}
