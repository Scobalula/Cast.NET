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
using System.Numerics;

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a Bone.
    /// </summary>
    public class BoneNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of this bone.
        /// </summary>
        public string Name => GetStringValueOrDefault("n", string.Empty);

        /// <summary>
        /// Gets the index of the this bone's parent. If this bone has no parent, then -1 is returned.
        /// </summary>
        public int ParentIndex => (int)GetFirstValueOrDefault("p", uint.MaxValue);

        /// <summary>
        /// Gets if segment scale compensation is enabled for this bone.
        /// </summary>
        public bool SegmentScaleCompensate => GetFirstValueOrDefault("ssc", (byte)0) == 1;

        /// <summary>
        /// Gets the bone's local position.
        /// </summary>
        public Vector3 LocalPosition => GetFirstValueOrDefault("lp", Vector3.Zero);

        /// <summary>
        /// Gets the bone's local rotation.
        /// </summary>
        public Quaternion LocalRotation => CastHelpers.CreateQuaternionFromVector4(GetFirstValueOrDefault("lr", Vector4.UnitW));

        /// <summary>
        /// Gets the bone's world position.
        /// </summary>
        public Vector3 WorldPosition => GetFirstValueOrDefault("wp", Vector3.Zero);

        /// <summary>
        /// Gets the bone's world rotation.
        /// </summary>
        public Quaternion WorldRotation => CastHelpers.CreateQuaternionFromVector4(GetFirstValueOrDefault("wr", Vector4.UnitW));

        /// <summary>
        /// Gets the bone's scale.
        /// </summary>
        public Vector3 Scale => GetFirstValueOrDefault("s", Vector3.One);

        /// <summary>
        /// Initializes a new instance of the <see cref="BoneNode"/> class.
        /// </summary>
        public BoneNode() : base(CastNodeIdentifier.Bone) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoneNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public BoneNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoneNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public BoneNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoneNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public BoneNode(ulong hash) : base(CastNodeIdentifier.Bone, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoneNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public BoneNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Bone, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public BoneNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoneNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public BoneNode(CastNode source) : base(source) { }
    }
}
