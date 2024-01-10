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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Blend Shape.
    /// </summary>
    public class BlendShapeNode : CastNode
    {
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
        public BlendShapeNode(ulong hash) : base(CastNodeIdentifier.Model, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlendShapeNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public BlendShapeNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Model, hash, properties, children)
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
        /// Gets the hash of the base shape.
        /// </summary>
        public ulong BaseShapeHash => GetFirstValueOrDefault<ulong>("b", 0);

        /// <summary>
        /// Gets the hashes of the target shapes.
        /// </summary>
        public List<ulong> TargetShapeHashes => GetProperty<CastArrayProperty<ulong>>("t").Values;

        /// <summary>
        /// Gets the weight scales of the target shapes.
        /// </summary>
        public List<float>? TargetWeightScales => GetPropertyOrNull<CastArrayProperty<float>>("ts")?.Values;

        /// <summary>
        /// Gets the base shape.
        /// </summary>
        public MeshNode? BaseShape => Parent?.GetChildByHashOrNull<MeshNode>(BaseShapeHash);

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

            return results.ToArray();
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

                for (int i = 0; i < targets.Count; i++)
                {
                    if (Parent.GetChildByHashOrNull<MeshNode>(targets[i]) is MeshNode targetMesh)
                    {
                        var weight = 1.0f;

                        if (weights is not null && i < weights.Count)
                            weight = weights[i];

                        yield return (targetMesh, weight);
                    }
                }
            }
        }
    }
}
