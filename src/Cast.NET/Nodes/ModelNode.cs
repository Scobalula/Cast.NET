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

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Model.
    /// </summary>
    public class ModelNode : CastNode
    {
        /// <summary>
        /// Gets the skeleton assigned to this model, if none is assigned, null is returned.
        /// </summary>
        public SkeletonNode? Skeleton => TryGetFirstChild<SkeletonNode>(out var node) ? node : null;

        /// <summary>
        /// Gets all the materials stored within this model.
        /// </summary>
        public MaterialNode[] Materials => GetChildrenOfType<MaterialNode>();

        /// <summary>
        /// Gets all the meshes stored within this model.
        /// </summary>
        public MeshNode[] Meshes => GetChildrenOfType<MeshNode>();

        /// <summary>
        /// Gets all the Blend Shapes stored within this model.
        /// </summary>
        public BlendShapeNode[] BlendShapes => GetChildrenOfType<BlendShapeNode>();

        /// <summary>
        /// Gets all the hairs stored within this model.
        /// </summary>
        public HairNode[] Hairs => GetChildrenOfType<HairNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNode"/> class.
        /// </summary>
        public ModelNode() : base(CastNodeIdentifier.Model) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public ModelNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public ModelNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public ModelNode(ulong hash) : base(CastNodeIdentifier.Model, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public ModelNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Model, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public ModelNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public ModelNode(CastNode source) : base(source) { }

        /// <summary>
        /// Enumerates through all meshes within this model.
        /// </summary>
        /// <returns>An enumerable collection of meshes within this model.</returns>
        public IEnumerable<MeshNode> EnumerateMeshes() => EnumerateChildrenOfType<MeshNode>();

        /// <summary>
        /// Enumerates through all materials within this model.
        /// </summary>
        /// <returns>An enumerable collection of materials within this model.</returns>
        public IEnumerable<MaterialNode> EnumerateMaterials() => EnumerateChildrenOfType<MaterialNode>();
    }
}
