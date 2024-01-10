﻿// ------------------------------------------------------------------------
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
    /// A class to hold a <see cref="CastNode"/> that contains an constraint.
    /// </summary>
    public class ConstraintNode : CastNode
    {
        /// <summary>
        /// Gets the name of this constraint.
        /// </summary>
        public string Name => GetStringValueOrDefault("n", string.Empty);

        /// <summary>
        /// Gets the constraint type.
        /// </summary>
        public string ConstraintType  => GetStringValueOrDefault("ct", "unknown");

        /// <summary>
        /// Gets the hash of the constraint <see cref="BoneNode"/>.
        /// </summary>
        public ulong ConstraintBoneHash => GetFirstValueOrDefault<ulong>("cb", 0);

        /// <summary>
        /// Gets the hash of the target <see cref="BoneNode"/>.
        /// </summary>
        public ulong TargetBoneHash => GetFirstValueOrDefault<ulong>("tb", 0);

        /// <summary>
        /// Gets if to enable maintain offset.
        /// </summary>
        public bool MaintainOffset  => GetFirstValueOrDefault("tr", (byte)0) == 1;

        /// <summary>
        /// Gets the start <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode? ConstraintBone => Parent?.GetChildByHashOrNull<BoneNode>(ConstraintBoneHash);

        /// <summary>
        /// Gets the target <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode? TargetBone => Parent?.GetChildByHashOrNull<BoneNode>(TargetBoneHash);


        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        public ConstraintNode() : base(CastNodeIdentifier.Constraint) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public ConstraintNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public ConstraintNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public ConstraintNode(ulong hash) : base(CastNodeIdentifier.Model, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public ConstraintNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Model, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public ConstraintNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public ConstraintNode(CastNode source) : base(source) { }
    }
}
