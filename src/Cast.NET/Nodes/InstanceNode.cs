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
    /// A class to hold a <see cref="CastNode"/> that contains Instance information.
    /// </summary>
    public class InstanceNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of the instance.
        /// </summary>
        public string Name { get => GetStringValue("n", string.Empty); set => AddString("n", value); }

        /// <summary>
        /// Gets or Sets the reference file hash.
        /// </summary>
        public ulong ReferenceFileHash { get => GetFirstValue<ulong>("rf"); set => AddValue("rf", value); }

        /// <summary>
        /// Gets or Sets the reference file.
        /// </summary>
        public FileNode ReferenceFile { get => Parent?.TryGetChild<FileNode>(ReferenceFileHash, out var node) == true ? node : throw new KeyNotFoundException(nameof(ReferenceFileHash)); set => ReferenceFileHash = value.Hash; }

        /// <summary>
        /// Gets or Sets the instance's position.
        /// </summary>
        public Vector3 Position { get => GetFirstValue("p", Vector3.Zero); set => AddValue("p", value); }

        /// <summary>
        /// Gets or Sets the instance's rotation.
        /// </summary>
        public Quaternion Rotation { get => CastHelpers.CreateQuaternionFromVector4(GetFirstValue("r", Vector4.Zero)); set => AddValue("r", CastHelpers.CreateVector4FromQuaternion(value)); }

        /// <summary>
        /// Gets or Sets the instance's position.
        /// </summary>
        public Vector3 Scale { get => GetFirstValue("s", Vector3.Zero); set => AddValue("s", value); }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        public InstanceNode() : base(CastNodeIdentifier.Instance) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public InstanceNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public InstanceNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public InstanceNode(ulong hash) : base(CastNodeIdentifier.Instance, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public InstanceNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Instance, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public InstanceNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public InstanceNode(CastNode source) : base(source) { }

        /// <inheritdoc/>
        public override string ToString() => $"{Name} = {ReferenceFile}";
    }
}
