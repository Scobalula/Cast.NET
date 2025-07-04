﻿// ------------------------------------------------------------------------
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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Blend Shape.
    /// </summary>
    public class BlendShapeNode : CastNode
    {
        /// <summary>
        /// Gets the hash of the base shape.
        /// </summary>
        public ulong BaseShapeHash => GetFirstValue<ulong>("b", 0);

        /// <summary>
        /// Gets the hashes of the target shapes.
        /// </summary>
        public CastArrayProperty<ulong> TargetShapeHashes => GetArrayProperty<ulong>("t");

        /// <summary>
        /// Gets the weight scales of the target shapes.
        /// </summary>
        public CastArrayProperty<float>? TargetWeightScales => TryGetArrayProperty<float>("ts", out var array) ? array : null;

        /// <summary>
        /// Gets the base shape.
        /// </summary>
        public MeshNode? BaseShape => Parent?.TryGetChild<MeshNode>(BaseShapeHash, out var node) == true ? node : null;

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
        /// Gets all target shapes within this blend shape.
        /// </summary>
        /// <returns>Target shapes with their weight.</returns>
        public (MeshNode, float)[] GetTargetShapes()
        {
            var results = new List<(MeshNode, float)>();

            foreach (var item in EnumerateTargetShapes())
            {
                results.Add(item);
            }

            return [.. results];
        }

        /// <summary>
        /// Enumerates through all target shapes within this blend shape.
        /// </summary>
        /// <returns>An enumerable collection of target shapes with their weight.</returns>
        public IEnumerable<(MeshNode, float)> EnumerateTargetShapes()
        {
            if (Parent != null)
            {
                var targets = TargetShapeHashes;
                var weights = TargetWeightScales;

                for (int i = 0; i < targets.Values.Count; i++)
                {
                    if (Parent.TryGetChild<MeshNode>(targets.Values[i], out var meshNode))
                    {
                        var weight = 1.0f;

                        if (weights is not null && i < weights.Values.Count)
                            weight = weights.Values[i];

                        yield return (meshNode, weight);
                    }
                }
            }
        }
    }
}
