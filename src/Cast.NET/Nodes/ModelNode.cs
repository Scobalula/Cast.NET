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
    /// A class to hold a <see cref="CastNode"/> that contains a Model.
    /// </summary>
    public class ModelNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of this model.
        /// </summary>
        public string Name => GetStringValue("n", string.Empty);

        /// <summary>
        /// Gets or Sets the model's position.
        /// </summary>
        public Vector3 Position { get => GetFirstValue("p", Vector3.Zero); set => AddValue("p", value); }

        /// <summary>
        /// Gets or Sets the model's rotation.
        /// </summary>
        public Quaternion Rotation { get => CastHelpers.CreateQuaternionFromVector4(GetFirstValue("r", Vector4.Zero)); set => AddValue("r", CastHelpers.CreateVector4FromQuaternion(value)); }

        /// <summary>
        /// Gets or Sets the model's position.
        /// </summary>
        public Vector3 Scale { get => GetFirstValue("s", Vector3.Zero); set => AddValue("s", value); }

        /// <summary>
        /// Gets or Sets the skeleton assigned to this model. When setting a new skeleton, all existing instances of a skeleton are removed from this.
        /// </summary>
        public SkeletonNode? Skeleton
        {
            get => TryGetFirstChild<SkeletonNode>(out var node) ? node : null;
            set
            {
                Children.RemoveAll(x => x is SkeletonNode);
                if (value is not null)
                    AddNode(value);
            }
        }

        /// <summary>
        /// Gets all the materials stored within this model. Setting this will remove all existing from the children.
        /// </summary>
        public MaterialNode[] Materials
        {
            get => GetChildrenOfType<MaterialNode>();
            set
            {
                Children.RemoveAll(x => x is MaterialNode);
                Children.AddRange(value);
            }
        }

        /// <summary>
        /// Gets or Sets all the meshes stored within this model. Setting this will remove all existing from the children.
        /// </summary>
        public MeshNode[] Meshes
        {
            get => GetChildrenOfType<MeshNode>();
            set
            {
                Children.RemoveAll(x => x is MeshNode);
                Children.AddRange(value);
            }
        }

        /// <summary>
        /// Gets or Sets all the blend shapes stored within this model. Setting this will remove all existing from the children.
        /// </summary>
        public BlendShapeNode[] BlendShapes
        {
            get => GetChildrenOfType<BlendShapeNode>();
            set
            {
                Children.RemoveAll(x => x is BlendShapeNode);
                Children.AddRange(value);
            }
        }

        /// <summary>
        /// Gets or Sets all the hairs stored within this model. Setting this will remove all existing from the children.
        /// </summary>
        public HairNode[] Hairs
        {
            get => GetChildrenOfType<HairNode>();
            set
            {
                Children.RemoveAll(x => x is HairNode);
                Children.AddRange(value);
            }
        }

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

        /// <summary>
        /// Enumerates through all blendshapes within this model.
        /// </summary>
        /// <returns>An enumerable collection of blendshapes within this model.</returns>
        public IEnumerable<BlendShapeNode> EnumerateBlendShapes() => EnumerateChildrenOfType<BlendShapeNode>();

        /// <summary>
        /// Enumerates through all hairs within this model.
        /// </summary>
        /// <returns>An enumerable collection of hairs within this model.</returns>
        public IEnumerable<HairNode> EnumerateHairs() => EnumerateChildrenOfType<HairNode>();

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}
