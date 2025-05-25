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
        /// Gets or Sets the name.
        /// </summary>
        public string Name => GetStringValue("n", string.Empty);

        /// <summary>
        /// Gets or Sets the reference file hash.
        /// </summary>
        public ulong ReferenceFileHash => GetFirstValue<ulong>("rf");

        /// <summary>
        /// Gets the position of this instance.
        /// </summary>
        public Vector3 Position => GetFirstValue<Vector3>("p");

        /// <summary>
        /// Gets the rotation of this instance.
        /// </summary>
        public Vector4 Rotation => GetFirstValue<Vector4>("r");

        /// <summary>
        /// Gets the scale of this instance.
        /// </summary>
        public Vector3 Scale => GetFirstValue<Vector3>("s");

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        public InstanceNode() : base(CastNodeIdentifier.Mesh) { }

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
        public InstanceNode(ulong hash) : base(CastNodeIdentifier.Mesh, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public InstanceNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Mesh, hash, properties, children)
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
    }
}
