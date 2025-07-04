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
    /// A class to hold a <see cref="CastNode"/> that contains a Bone.
    /// </summary>
    public class BoneNode : CastNode
    {
        /// <summary>
        /// Gets or Sets the name of this bone.
        /// </summary>
        public string Name => GetStringValue("n", string.Empty);

        /// <summary>
        /// Gets the index of the this bone's parent. If this bone has no parent, then -1 is returned.
        /// </summary>
        public int ParentIndex => (int)GetFirstValue("p", uint.MaxValue);

        /// <summary>
        /// Gets if segment scale compensation is enabled for this bone.
        /// </summary>
        public bool SegmentScaleCompensate => GetFirstValue("ssc", (byte)0) == 1;

        /// <summary>
        /// Gets the bone's local position.
        /// </summary>
        public Vector3 LocalPosition => GetFirstValue("lp", Vector3.Zero);

        /// <summary>
        /// Gets the bone's local rotation.
        /// </summary>
        public Quaternion LocalRotation => CastHelpers.CreateQuaternionFromVector4(GetFirstValue("lr", Vector4.UnitW));

        /// <summary>
        /// Gets the bone's world position.
        /// </summary>
        public Vector3 WorldPosition => GetFirstValue("wp", Vector3.Zero);

        /// <summary>
        /// Gets the bone's world rotation.
        /// </summary>
        public Quaternion WorldRotation => CastHelpers.CreateQuaternionFromVector4(GetFirstValue("wr", Vector4.UnitW));

        /// <summary>
        /// Gets the bone's scale.
        /// </summary>
        public Vector3 Scale => GetFirstValue("s", Vector3.One);

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

        /// <summary>
        /// Attempts to get the local position of this <see cref="BoneNode"/>.
        /// </summary>
        /// <param name="localPosition">Local position if found.</param>
        /// <returns>True if found, otherwise false.</returns>
        public bool TryGetLocalPosition(out Vector3 localPosition)
        {
            if (Properties.TryGetValue("lp", out var prop) && prop is CastArrayProperty<Vector3> arrayProp && arrayProp.ValueCount > 0)
            {
                localPosition = arrayProp.Values[0];
                return true;
            }

            localPosition = Vector3.Zero;
            return false;
        }

        /// <summary>
        /// Attempts to get the local rotation of this <see cref="BoneNode"/>.
        /// </summary>
        /// <param name="localRotation">Local rotation if found.</param>
        /// <returns>True if found, otherwise false.</returns>
        public bool TryGetLocalRotation(out Quaternion localRotation)
        {
            if (Properties.TryGetValue("lr", out var prop) && prop is CastArrayProperty<Vector4> arrayProp && arrayProp.ValueCount > 0)
            {
                localRotation = CastHelpers.CreateQuaternionFromVector4(arrayProp.Values[0]);
                return true;
            }

            localRotation = Quaternion.Identity;
            return false;
        }

        /// <summary>
        /// Attempts to get the world position of this <see cref="BoneNode"/>.
        /// </summary>
        /// <param name="worldPosition">world position if found.</param>
        /// <returns>True if found, otherwise false.</returns>
        public bool TryGetWorldPosition(out Vector3 worldPosition)
        {
            if (Properties.TryGetValue("wp", out var prop) && prop is CastArrayProperty<Vector3> arrayProp && arrayProp.ValueCount > 0)
            {
                worldPosition = arrayProp.Values[0];
                return true;
            }

            worldPosition = Vector3.Zero;
            return false;
        }

        /// <summary>
        /// Attempts to get the world rotation of this <see cref="BoneNode"/>.
        /// </summary>
        /// <param name="worldRotation">World rotation if found.</param>
        /// <returns>True if found, otherwise false.</returns>
        public bool TryGetWorldRotation(out Quaternion worldRotation)
        {
            if (Properties.TryGetValue("wr", out var prop) && prop is CastArrayProperty<Vector4> arrayProp && arrayProp.ValueCount > 0)
            {
                worldRotation = CastHelpers.CreateQuaternionFromVector4(arrayProp.Values[0]);
                return true;
            }

            worldRotation = Quaternion.Identity;
            return false;
        }
    }
}
