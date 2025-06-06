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

using System.Numerics;

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains Hair.
    /// </summary>
    public class HairNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name.
        /// </summary>
        public string Name => GetStringValue("n", string.Empty);

        /// <summary>
        /// Gets the raw segment buffer.
        /// </summary>
        public CastProperty SegmentsBuffer => GetProperty("se");

        /// <summary>
        /// Gets the particle buffer.
        /// </summary>
        public CastArrayProperty<Vector3> ParticleBuffer => GetArrayProperty<Vector3>("se");

        /// <summary>
        /// Gets the hash of the <see cref="MaterialNode"/> assigned to this hair.
        /// </summary>
        public ulong MaterialHash => GetFirstValue<ulong>("m", 0);

        /// <summary>
        /// Gets the <see cref="MaterialNode"/> assigned to this mesh.
        /// </summary>
        public MaterialNode? Material => Parent?.TryGetChild<MaterialNode>(MaterialHash, out var node) == true ? node : null;

        /// <summary>
        /// Initializes a new instance of the <see cref="HairNode"/> class.
        /// </summary>
        public HairNode() : base(CastNodeIdentifier.Mesh) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HairNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public HairNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HairNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public HairNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HairNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public HairNode(ulong hash) : base(CastNodeIdentifier.Mesh, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HairNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public HairNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Mesh, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public HairNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HairNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public HairNode(CastNode source) : base(source) { }
    }
}
