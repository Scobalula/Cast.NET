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
    /// A class to hold a <see cref="CastNode"/> that contains a skeleton.
    /// </summary>
    public class SkeletonNode : CastNode
    {
        /// <summary>
        /// Gets all the bones stored within this skeleton.
        /// </summary>
        public BoneNode[] Bones => GetChildrenOfType<BoneNode>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        public SkeletonNode() : base(CastNodeIdentifier.Skeleton) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        public SkeletonNode(CastNodeIdentifier identifier) : base(identifier) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        public SkeletonNode(CastNodeIdentifier identifier, ulong hash) : base(identifier, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        public SkeletonNode(ulong hash) : base(CastNodeIdentifier.Skeleton, hash) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public SkeletonNode(ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(CastNodeIdentifier.Skeleton, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CastNode"/> class.
        /// </summary>
        /// <param name="identifier">Node identifier.</param>
        /// <param name="hash">Optional hash value for lookups.</param>
        /// <param name="properties">Properties to assign to this node..</param>
        /// <param name="children">Children to assign to this node..</param>
        public SkeletonNode(CastNodeIdentifier identifier, ulong hash, Dictionary<string, CastProperty>? properties, List<CastNode>? children) :
            base(identifier, hash, properties, children)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkeletonNode"/> class.
        /// </summary>
        /// <param name="source">Node to copy from. A shallow copy is performed and references to the source are stored.</param>
        public SkeletonNode(CastNode source) : base(source) { }

        /// <summary>
        /// Enumerates through all bones within this skeleton.
        /// </summary>
        /// <returns>An enumerable collection of bones within this skeleton.</returns>
        public IEnumerable<BoneNode> EnumerateBones() => EnumerateChildrenOfType<BoneNode>();

        /// <summary>
        /// Calculates the local positions of all bones within this skeleton.
        /// </summary>
        public void CalculateLocalTransforms()
        {
            foreach (var bone in EnumerateBones())
            {
                if(bone.ParentIndex == -1)
                {
                    bone.AddValue("lp", bone.WorldPosition);
                    bone.AddValue("lr", CastHelpers.CreateVector4FromQuaternion(bone.WorldRotation));
                }
                else
                {
                    var parent = GetChild<BoneNode>(bone.ParentIndex);

                    bone.AddValue("lr", CastHelpers.CreateVector4FromQuaternion(Quaternion.Conjugate(parent.WorldRotation) * bone.WorldRotation));
                    bone.AddValue("lp", Vector3.Transform(bone.WorldPosition - parent.WorldPosition, Quaternion.Conjugate(parent.WorldRotation)));
                }
            }
        }

        /// <summary>
        /// Calculates the world positions of all bones within this skeleton.
        /// </summary>
        public void CalculateWorldTransforms()
        {
            foreach (var bone in EnumerateBones())
            {
                if (bone.ParentIndex == -1)
                {
                    bone.AddValue("wp", bone.LocalPosition);
                    bone.AddValue("wr", CastHelpers.CreateVector4FromQuaternion(bone.LocalRotation));
                }
                else
                {
                    var parent = GetChild<BoneNode>(bone.ParentIndex);

                    bone.AddValue("wr", CastHelpers.CreateVector4FromQuaternion(parent.WorldRotation * bone.LocalRotation));
                    bone.AddValue("wp", Vector3.Transform(bone.WorldPosition, parent.WorldRotation) + parent.WorldPosition);
                }
            }
        }
    }
}
