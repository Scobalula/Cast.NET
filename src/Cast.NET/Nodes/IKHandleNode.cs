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
    /// A class to hold a <see cref="CastNode"/> that contains an IK handle.
    /// </summary>
    public class IKHandleNode : CastNode
    {
        /// <summary>
        /// Gets the hash of the start <see cref="BoneNode"/>.
        /// </summary>
        public ulong StartBoneHash => GetFirstValue<ulong>("sb", 0);

        /// <summary>
        /// Gets the hash of the end <see cref="BoneNode"/>.
        /// </summary>
        public ulong EndBoneHash => GetFirstValue<ulong>("eb", 0);

        /// <summary>
        /// Gets the hash of the target <see cref="BoneNode"/>.
        /// </summary>
        public ulong TargetBoneHash => GetFirstValue<ulong>("tb", 0);

        /// <summary>
        /// Gets the hash of the pole vector <see cref="BoneNode"/>.
        /// </summary>
        public ulong PoleVectorBoneHash => GetFirstValue<ulong>("pv", 0);

        /// <summary>
        /// Gets the hash of the pole (twist) <see cref="BoneNode"/>.
        /// </summary>
        public ulong PoleBoneHash => GetFirstValue<ulong>("pb", 0);

        /// <summary>
        /// Gets if target rotation effects the chain.
        /// </summary>
        public bool UseTargetRotation => GetFirstValue("tr", (byte)0) == 1;

        /// <summary>
        /// Gets the start <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode? StartBone => Parent?.TryGetChild<BoneNode>(StartBoneHash, out var node) == true ? node : null;

        /// <summary>
        /// Gets the end <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode? EndBone => Parent?.TryGetChild<BoneNode>(EndBoneHash, out var node) == true ? node : null;

        /// <summary>
        /// Gets the target <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode? TargetBone => Parent?.TryGetChild<BoneNode>(TargetBoneHash, out var node) == true ? node : null;

        /// <summary>
        /// Gets the pole vector <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode? PoleVectorBone => Parent?.TryGetChild<BoneNode>(PoleVectorBoneHash, out var node) == true ? node : null;

        /// <summary>
        /// Gets the pole <see cref="BoneNode"/>.
        /// </summary>
        public BoneNode? PoleBone => Parent?.TryGetChild<BoneNode>(PoleBoneHash, out var node) == true ? node : null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IKHandleNode"/> class.
        /// </summary>
        public IKHandleNode() : base(CastNodeIdentifier.IKHandle) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IKHandleNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public IKHandleNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IKHandleNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public IKHandleNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IKHandleNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public IKHandleNode(ulong hash) : base(CastNodeIdentifier.Model, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IKHandleNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public IKHandleNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Model, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public IKHandleNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IKHandleNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public IKHandleNode(CastNode source) : base(source) { }
    }
}
