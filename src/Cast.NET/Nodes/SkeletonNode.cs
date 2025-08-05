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
using System.Data;
using System.Numerics;
using System.Reflection.Metadata;

namespace Cast.NET.Nodes
{
    /// <summary>
    /// A class to hold a <see cref="CastNode"/> that contains a skeleton.
    /// </summary>
    public class SkeletonNode : CastNode
    {
        /// <summary>
        /// Gets or Sets all the bones stored within this skeleton. Setting this will remove all existing from the children and you will need to update each bone's parent index.
        /// </summary>
        public BoneNode[] Bones
        {
            get => GetChildrenOfType<BoneNode>();
            set
            {
                Children.RemoveAll(x => x is BoneNode);
                Children.AddRange(value);
            }
        }

        /// <summary>
        /// Gets or Sets all the IK handles stored within this skeleton. Setting this will remove all existing from the children.
        /// </summary>
        public IKHandleNode[] IKHandles
        {
            get => GetChildrenOfType<IKHandleNode>();
            set
            {
                Children.RemoveAll(x => x is IKHandleNode);
                Children.AddRange(value);
            }
        }

        /// <summary>
        /// Gets or Sets all the constraints stored within this skeleton. Setting this will remove all existing from the children.
        /// </summary>
        public ConstraintNode[] Constraints
        {
            get => GetChildrenOfType<ConstraintNode>();
            set
            {
                Children.RemoveAll(x => x is ConstraintNode);
                Children.AddRange(value);
            }
        }

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
        /// Enumerates through all IK handles within this skeleton.
        /// </summary>
        /// <returns>An enumerable collection of IK handles within this skeleton.</returns>
        public IEnumerable<IKHandleNode> EnumerateIKHandles() => EnumerateChildrenOfType<IKHandleNode>();

        /// <summary>
        /// Enumerates through all constraints within this skeleton.
        /// </summary>
        /// <returns>An enumerable collection of constraints within this skeleton.</returns>
        public IEnumerable<ConstraintNode> EnumerateConstraints() => EnumerateChildrenOfType<ConstraintNode>();

        /// <summary>
        /// Gets the bone at the provided index.
        /// </summary>
        /// <param name="index">The index of the bone within the list of child odes.</param>
        /// <returns>Resulting bone node.</returns>
        /// <exception cref="NotSupportedException">Thrown if the object at the index is not a <see cref="BoneNode"/>.</exception>
        public BoneNode GetBone(int index)
        {
            if (Children[index] is BoneNode bone)
                return bone;

            throw new NotSupportedException($"Node at index {index} is of type: {Children[index].GetType()}");
        }

        /// <summary>
        /// Calculates the local positions of all bones within this skeleton.
        /// </summary>
        public void CalculateLocalTransforms()
        {
            foreach (var bone in EnumerateBones())
            {
                var parentIndex = bone.ParentIndex;

                if (parentIndex == -1)
                {
                    bone.AddValue("lp", bone.WorldPosition);
                    bone.AddValue("lr", CastHelpers.CreateVector4FromQuaternion(bone.WorldRotation));
                }
                else
                {
                    var parent = GetChild<BoneNode>(parentIndex);

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
                var parentIndex = bone.ParentIndex;

                if (parentIndex == -1)
                {
                    bone.AddValue("wp", bone.LocalPosition);
                    bone.AddValue("wr", CastHelpers.CreateVector4FromQuaternion(bone.LocalRotation));
                }
                else
                {
                    var parent = GetChild<BoneNode>(parentIndex);

                    bone.AddValue("wr", CastHelpers.CreateVector4FromQuaternion(parent.WorldRotation * bone.LocalRotation));
                    bone.AddValue("wp", Vector3.Transform(bone.WorldPosition, parent.WorldRotation) + parent.WorldPosition);
                }
            }
        }
    }
}
